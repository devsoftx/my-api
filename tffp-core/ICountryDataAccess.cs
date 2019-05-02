using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tffp_domain;

namespace tffp_core
{
    public interface ICountryDataAccess
    {
        Task<bool> Validate(IEnumerable<CountryItem> countries);
        Task<IReadOnlyCollection<Country>> GetAll();
        Task<IReadOnlyCollection<Country>> GetCountries(string nmEn);
    }
}
