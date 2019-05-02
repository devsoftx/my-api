using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tffp_domain;
using tffp_services;

namespace tffp_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService countryService;
        public CountriesController(ICountryService countryService)
        {
            this.countryService = countryService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<Country>), 200)]
        public async Task<IActionResult> Get()
        {
            var countries = await this.countryService.GetAll();
            return this.Ok(countries);
        }

        [HttpGet("find")]
        [ProducesResponseType(typeof(IReadOnlyList<Country>), 200)]
        public async Task<IActionResult> GetCountries([FromQuery] string nmEn)
        {
            if (!string.IsNullOrEmpty(nmEn))
            {
                var countries = await this.countryService.GetCountries(nmEn);
                return this.Ok(countries);
            }

            return this.BadRequest();
        }
    }
}
