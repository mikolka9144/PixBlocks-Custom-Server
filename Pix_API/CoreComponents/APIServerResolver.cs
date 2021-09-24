using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Pix_API.Interfaces;
using PixBlocks.Server.DataModels.DataModels;
using Pix_API.Utills;
using NLog;

namespace Pix_API
{
    public class APIServerResolver
    {
        private readonly ICommandRepository logic;

        private readonly IUserDatabaseProvider userDatabaseProvider;
        private readonly ServerUtills security;
        private readonly List<IAbstractUser> abstractUsers;
        private readonly ILogger log;
        private readonly Type logic_type;

        public APIServerResolver(ICommandRepository logic, IUserDatabaseProvider userDatabaseProvider, ServerUtills security,List<IAbstractUser> abstractUsers,LogFactory log)
        {
            this.logic = logic;
            this.userDatabaseProvider = userDatabaseProvider;
            this.security = security;
            this.abstractUsers = abstractUsers;
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

            if (method.GetParameters().Last().ParameterType == typeof(AuthorizeData))
            {
                AuthorizeData authorizeData = array.Last() as AuthorizeData;
                if (authorizeData.UserId < 0)
                {
                    var index = authorizeData.UserId + 1;
                    var abstract_user = abstractUsers[-index];
                    if (abstract_user.password != authorizeData.PasswordMD5) return "";

                    var user_method = abstract_user.GetType().GetMethod(name);
                    if(user_method is null)
                    {
                        log.Warn("method " + name + " not implemented for " + abstract_user.GetType().Name);
                        return "";
                    }
                    return ExecuteMethod(abstract_user,user_method, array);
                }
                if (!security.IsAuthorizeValid(authorizeData)) return "";
            }

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
