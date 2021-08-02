﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Reflection;
using PixBlocks.Server.DataModels.DataModels;
using Pix_API.Providers;

namespace Pix_API
{
    internal class APIServerResolver
    {
        private readonly Main_Logic logic;
        private readonly IUserDatabaseProvider userDatabaseProvider;
        private readonly Type logic_type;

        public APIServerResolver(Main_Logic logic,IUserDatabaseProvider userDatabaseProvider)
        {
            this.logic = logic;
            this.userDatabaseProvider = userDatabaseProvider;
            logic_type = logic.GetType();
        }
        public string Execute_API_Method(string name,string[] parameters)
        {

            if (logic_type.GetMethod(name) == null)
            {
                Console.WriteLine($"method {name} not implemented");
                return "";
            }
            Console.WriteLine($"method {name} was issued to be called");

            var method = logic_type.GetMethod(name);
            var deserialized_parameters = ParseParameters(parameters,method.GetParameters());

            if (!CallerhasPermissionToRun(method,deserialized_parameters)) return "";

            Console.WriteLine($"method {name} was called");
            var object_response = method.Invoke(logic,deserialized_parameters);

            var object_json = JsonConvert.SerializeObject(object_response);
            return object_json;
        }

        private bool CallerhasPermissionToRun(MethodInfo method,object[] method_arguments)
        {
            var IsReuiredAuthentication = Attribute.IsDefined(method, typeof(RequiresAuthentication));
            if (IsReuiredAuthentication)
            {
                var auth = method_arguments.Last() as AuthorizeData;
                var server_user = userDatabaseProvider.GetUser(auth.UserId);

                var IsAuthenticationValid = auth.PasswordMD5 == server_user.Md5Password;
                return IsAuthenticationValid;
            }
            return true;
        }



        private object[] ParseParameters(string[] parameters,ParameterInfo[] parameters_types)
        {
            var output = new List<object>();
            for (int i = 0; i < parameters_types.Length; i++)
            {
                var parameter_type = parameters_types[i].ParameterType;
                var deserialised_object = 
                JsonConvert.DeserializeObject(parameters[i],parameter_type);
                output.Add(deserialised_object);
            }
            return output.ToArray();
        }
    }
    public class RequiresAuthentication : Attribute { }

}