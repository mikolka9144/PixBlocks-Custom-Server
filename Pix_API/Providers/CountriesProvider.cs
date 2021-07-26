using System;
using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;
namespace Pix_API.Providers
{
    public class CountriesProvider:ICountriesProvider
    {


        public List<Countrie> GetAllCountries()
        {
            return new List<Countrie>()
            {
                new Countrie(1,"test country","PL"),
                new Countrie(2,"Świnoujście","US")
            };
        }
    }
    public interface ICountriesProvider
    {
        List<Countrie> GetAllCountries();
    }
}
