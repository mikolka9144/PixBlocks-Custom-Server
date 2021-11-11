using System;
namespace Pix_API.Base.Utills
{
    public interface IAPIServerResolver
    {
        CommandResult Execute_API_Method(string method_name, string[] parameters,string body);
    }
    public class FromBody : Attribute {
    }
    public class CommandResult
    {
        public readonly bool wasFound;
        public readonly string result = "";

        public CommandResult(string result)
        {
            wasFound = true;
            this.result = result;
        }
        public CommandResult()
        {
        }
    }
}
