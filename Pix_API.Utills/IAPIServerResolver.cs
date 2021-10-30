using System;
namespace Pix_API.Base.Utills
{
    public interface IAPIServerResolver
    {
        string Execute_API_Method(string method_name, string[] parameters,string body);
    }
    public class FromBody : Attribute {
    }
}
