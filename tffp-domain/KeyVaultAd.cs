using System;
using System.Collections.Generic;
using System.Text;

namespace tffp_domain
{
    public class KeyVaultAd
    {
        public string Instance { get; set; }
        public string SecretName { get; set; }
        public bool Enabled { get; set; }
        public string DefaultConnectionString { get; set; }
        public string ProxyUrl { get; set; }
        public string ProxyUsername { get; set; }
        public string ProxyPassword { get; set; }
    }
}
