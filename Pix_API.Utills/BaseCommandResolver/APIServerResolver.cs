using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using NLog;
using System;
using Newtonsoft.Json;
using System.Linq;
using Pix_API.Base.Utills;

namespace Pix_API.Base.CommandResolver
{
    public class APIServerResolver : IAPIServerResolver
    {
        private readonly ICommandRepository logic;

        private readonly ILogger log;
        private readonly Type logic_type;

        public APIServerResolver(ICommandRepository logic, LogFactory log)
        {
            this.logic = logic;
            this.log = log.GetLogger(LogsNames.COMMAND_RESOLVER);
            logic_type = logic.GetType();
        }

        public CommandResult Execute_API_Method(string name, string[] parameters, string body)
        {
            MethodInfo method = logic_type.GetMethod(name);
            if (method is null)
            {
                log.Warn("method " + name + " not implemented");
                return new CommandResult();
            }
            object[] array = ParseParameters(parameters, body, method.GetParameters());
            return new CommandResult(ExecuteMethod(logic, method, array).Replace("\n", ""));
        }

        private string ExecuteMethod(ICommandRepository repositoryObject, MethodInfo method, object[] array)
        {
            Stopwatch stopwatch = new Stopwatch();
            log.Info("method " + method.Name + " in " + method.ReflectedType.Name + " was called");
            stopwatch.Start();
            object value = method.Invoke(repositoryObject, array);
            stopwatch.Stop();
            log.Info($"method {method.Name} done in {stopwatch.ElapsedMilliseconds} ms");
            return JsonConvert.SerializeObject(value);
        }

        private object[] ParseParameters(string[] parameters, string body, ParameterInfo[] parameters_types)
        {
            List<object> list = new List<object>();
            for (int i = 0; i < parameters_types.Length; i++)
            {
                Type parameterType = parameters_types[i].ParameterType;

                if (parameters_types[i].CustomAttributes.Any(s => s.AttributeType == typeof(FromBody)))
                {
                    list.Add(JsonConvert.DeserializeObject(body, parameterType));
                }
                else if (parameterType == typeof(string)) list.Add(parameters[i]);
                else if (parameterType == typeof(int)) list.Add(Convert.ToInt32(parameters[i]));
                else throw new InvalidCastException("argument MUST be 'String' OR 'Int'");
            }
            return list.ToArray();
        }
    }
}
