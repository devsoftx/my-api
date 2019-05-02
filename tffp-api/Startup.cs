using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using tffp_core;
using tffp_domain;
using tffp_services;

namespace tffp_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddProtectWebApiWithMicrosoftIdentityPlatformV2(Configuration);

            services.Configure<KeyVaultAd>(options => Configuration.GetSection(nameof(KeyVaultAd)).Bind(options));
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes =
                    ResponseCompressionDefaults.MimeTypes.Concat(
                        new[] { "text/json" });
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.AddMemoryCache();
            services.AddCors();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                 .AllowAnyMethod()
                                 .AllowAnyHeader().WithExposedHeaders();
                      });
            });

            var keyVaultAd = new KeyVaultAd();
            Configuration.GetSection(nameof(KeyVaultAd)).Bind(keyVaultAd); 
            var handler = new HttpClientHandler
            {
                Proxy = new WebProxy
                {
                    Address = new Uri(keyVaultAd.ProxyUrl),
                    BypassProxyOnLocal = false,
                    UseDefaultCredentials = false,

                    // *** These creds are given to the proxy server, not the web server ***
                    Credentials = new NetworkCredential(keyVaultAd.ProxyUsername, keyVaultAd.ProxyPassword)
                }
            };

            var kv = new KeyVaultClient(
                async (string authority, string resource, string scope) =>
                {
                    var authContext = new AuthenticationContext(authority);
                    var clientId = Configuration.GetSection("AzureAd").GetValue<string>("ClientId");
                    var clientSecret = Configuration.GetSection("AzureAd").GetValue<string>("ClientSecret");
                    ClientCredential clientCred = new ClientCredential(clientId, clientSecret);
                    AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);
                    if (result == null)
                    {
                        throw new InvalidOperationException("Failed to retrieve access token for Key Vault");
                    }

                    return result.AccessToken;
                }, new HttpClient(handler, true)
            );

            

            services.AddSingleton<IKeyVaultClient>(kv);

            // Register DA
            services.AddTransient<ICountryDataAccess, CountryDataAccess>();

            // Register Services
            services.AddTransient<ICountryService, CountryService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                //app.UseHsts();
            }

            app.UseHttpsRedirection();
            //app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseCors("AllowAllHeaders");
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
