using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using tffp_domain;

namespace tffp_services
{
    public interface ICountryService
    {
        Task<bool> Validate(IEnumerable<CountryItem> countries);
        Task<IReadOnlyCollection<Country>> GetAll();
        Task<IReadOnlyCollection<Country>> GetCountries(string nmEn);
    }
}
