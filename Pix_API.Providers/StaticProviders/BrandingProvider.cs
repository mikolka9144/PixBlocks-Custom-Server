using System;
using System.IO;
using Pix_API.Interfaces;
namespace Pix_API.Providers.StaticProviders
{
    public class BrandingProvider:IBrandingProvider
    {
        public BrandingProvider()
        {
        }

        public string GetBase64LogoForUser(int userId)
        {
            if (File.Exists("./logo.png")) return Convert.ToBase64String(File.ReadAllBytes("./logo.png"));
            Console.WriteLine("No logo.png file. Skipping branding.");
            return null;
        }
    }
}
