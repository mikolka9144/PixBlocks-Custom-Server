using System.Collections.Generic;
using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.PixBlocks.Interfaces
{
	public interface ICountriesProvider
	{
		List<Countrie> GetAllCountries();
	}
}
