using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API
{
    public class APIServerResolver
    {
        private readonly ICommandRepository logic;

        private readonly IUserDatabaseProvider userDatabaseProvider;
        private readonly SecurityChecks security;
        private readonly Type logic_type;

        public APIServerResolver(ICommandRepository logic, IUserDatabaseProvider userDatabaseProvider, SecurityChecks security)
        {
            this.logic = logic;
            this.userDatabaseProvider = userDatabaseProvider;
            this.security = security;
            logic_type = logic.GetType();
        }

        public string Execute_API_Method(string name, string[] parameters)
        {
            if (logic_type.GetMethod(name) == null)
            {
                Console.WriteLine("method " + name + " not implemented");
                return "";
            }
            MethodInfo method = logic_type.GetMethod(name);
            object[] array = ParseParameters(parameters, method.GetParameters());
            if (!CallerhasPermissionToRun(method, array))
            {
                return "";
            }
            Stopwatch stopwatch = new Stopwatch();
            Console.Write("method " + name + " was called");
            stopwatch.Start();
            object value = method.Invoke(logic, array);
            stopwatch.Stop();
            Console.WriteLine($" and done in {stopwatch.ElapsedMilliseconds} ms");
            return JsonConvert.SerializeObject(value);
        }

        private bool CallerhasPermissionToRun(MethodInfo method, object[] method_arguments)
        {
            if (method.GetParameters().Last().ParameterType == typeof(AuthorizeData))
            {
                AuthorizeData authorizeData = method_arguments.Last() as AuthorizeData;
                return security.IsAuthorizeValid(authorizeData);
            }
            return true;
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
