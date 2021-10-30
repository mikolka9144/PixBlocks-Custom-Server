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
    public class APIServerResolver:IAPIServerResolver
    {
        private readonly ICommandRepository logic;

        private readonly ILogger log;
        private readonly Type logic_type;

        public APIServerResolver(ICommandRepository logic,LogFactory log)
        {
            this.logic = logic;
            this.log = log.GetLogger(LogsNames.COMMAND_RESOLVER);
            logic_type = logic.GetType();
        }

        public string Execute_API_Method(string name, string[] parameters,string body)
        {
            MethodInfo method = logic_type.GetMethod(name);
            if (method is null)
            {
                log.Warn("method " + name + " not implemented");
                return "";
            }
            object[] array = ParseParameters(parameters,body, method.GetParameters());
            return ExecuteMethod(logic,method, array).Replace("\n","");
        }

        private string ExecuteMethod(ICommandRepository repositoryObject,MethodInfo method, object[] array)
        {
            Stopwatch stopwatch = new Stopwatch();
            log.Info("method " + method.Name + " in "+ method.ReflectedType.Name +" was called");
            stopwatch.Start();
            object value = method.Invoke(repositoryObject, array);
            stopwatch.Stop();
            log.Info($"method {method.Name} done in {stopwatch.ElapsedMilliseconds} ms");
            return JsonConvert.SerializeObject(value);
        }

        private object[] ParseParameters(string[] parameters,string body, ParameterInfo[] parameters_types)
        {
            List<object> list = new List<object>();
            for (int i = 0; i < parameters_types.Length; i++)
            {
                Type parameterType = parameters_types[i].ParameterType;
                if (parameterType != typeof(string)) throw new InvalidCastException("argument MUST be 'String'");
                if (parameters_types[i].CustomAttributes.Any(s => s.AttributeType == typeof(FromBody))){
                    list.Add(body);
                }
                else list.Add(parameters[i]);
               
            }
            return list.ToArray();
        }
    }
}
