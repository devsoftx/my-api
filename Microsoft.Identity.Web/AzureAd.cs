using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Identity.Web
{
    public class AzureAd
    {
        //
        // Summary:
        //     Gets or sets the client Id.
        public string ClientId { get; set; }
        //
        // Summary:
        //     Gets or sets the client secret.
        public string ClientSecret { get; set; }
        //
        // Summary:
        //     Gets or sets the tenant Id.
        public string TenantId { get; set; }
        //
        // Summary:
        //     Gets or sets the Azure Active Directory instance.
        public string Instance { get; set; }
        //
        // Summary:
        //     Gets or sets the domain of the Azure Active Directory tennant.
        public string Domain { get; set; }

        //
        // Summary:
        //     Gets or sets the Audience
        public string Audience { get; set; }
    }
}
