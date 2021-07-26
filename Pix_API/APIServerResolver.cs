using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pix_API
{
    internal class APIServerResolver
    {
        private readonly Main_Logic logic;
        private readonly Type logic_type;

        public APIServerResolver(Main_Logic logic)
        {
            this.logic = logic;
            logic_type = logic.GetType();
        }
        public string Execute_API_Method(string name,string[] parameters)
        {

            if (logic_type.GetMethod(name) == null)
            {
                Console.WriteLine($"method {name} not implemented");
                return "";
            }
            Console.WriteLine($"method {name} was called");

            var object_response = logic_type.GetMethod(name)
            .Invoke(logic,new[] { parameters });

            var object_json = JsonConvert.SerializeObject(object_response);
            return object_json;
        }
    }
}