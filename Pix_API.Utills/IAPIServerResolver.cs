using System;
namespace Pix_API.CoreComponents
{
    public interface IAPIServerResolver
    {
        string Execute_API_Method(string method_name, string[] parameters);
    }
}
