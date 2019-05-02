using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tffp_core;
using tffp_domain;

namespace tffp_services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryDataAccess countryDataAccess;
        public CountryService(ICountryDataAccess countryDataAccess)
        {
            this.countryDataAccess = countryDataAccess;
        }

        public async Task<IReadOnlyCollection<Country>> GetAll()
        {
            return await this.countryDataAccess.GetAll();
        }

        public async Task<IReadOnlyCollection<Country>> GetCountries(string nmEn)
        {
            return await this.countryDataAccess.GetCountries(nmEn);
        }

        public async Task<bool> Validate(IEnumerable<CountryItem> countries)
        {
            return await this.countryDataAccess.Validate(countries);
        }
    }
}
