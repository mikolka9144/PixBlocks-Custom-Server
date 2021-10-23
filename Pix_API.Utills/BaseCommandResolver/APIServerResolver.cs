using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Pix_API.Interfaces;
using Pix_API.Utills;
using NLog;
using System;
using Pix_API.CoreComponents;
using Newtonsoft.Json;

namespace Pix_API
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

        public string Execute_API_Method(string name, string[] parameters)
        {
            MethodInfo method = logic_type.GetMethod(name);
            if (method is null)
            {
                log.Warn("method " + name + " not implemented");
                return "";
            }
            object[] array = ParseParameters(parameters, method.GetParameters());
            return ExecuteMethod(logic,method, array);
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

        private object[] ParseParameters(string[] parameters, ParameterInfo[] parameters_types)
        {
            List<object> list = new List<object>();
            for (int i = 0; i < parameters_types.Length; i++)
            {
                Type parameterType = parameters_types[i].ParameterType;
                object item = JsonConvert.DeserializeObject(parameters[i], parameterType);
                list.Add(item);
            }
            return list.ToArray();
        }
    }
}
