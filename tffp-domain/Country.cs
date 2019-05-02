using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace tffp_domain
{
    public class Country
    {
        [JsonProperty("cd")]
        public string CountryCode { get; set; }
        [JsonProperty("idbCd")]
        public string IdbCode { get; set; }
        [JsonProperty("nmEn")]
        public string NameEnglish { get; set; }
    }
}
