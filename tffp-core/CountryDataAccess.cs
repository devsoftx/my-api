using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using tffp_domain;

namespace tffp_core
{
    public class CountryDataAccess : ICountryDataAccess
    {
        private readonly KeyVaultAd keyVaultOptions;
        private readonly IKeyVaultClient keyVaultClient;
        public CountryDataAccess(IOptions<KeyVaultAd> keyVaultOptions,
            IKeyVaultClient keyVaultClient)
        {
            this.keyVaultOptions = keyVaultOptions.Value;
            this.keyVaultClient = keyVaultClient;
        }

        public async Task<IReadOnlyCollection<Country>> GetAll()
        {
            var countries = new List<Country>();
            try
            {
                var connectionString = this.keyVaultOptions.DefaultConnectionString;
                if (keyVaultOptions.Enabled)
                {
                    var secret = await keyVaultClient.GetSecretAsync($"{this.keyVaultOptions.Instance}secrets/{this.keyVaultOptions.SecretName}")
                            .ConfigureAwait(false);
                    connectionString = secret.Value;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("dbo.GetAllCountries", connection)
                    {
                        CommandType = CommandType.StoredProcedure,
                    };

                    var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        var country = new Country
                        {
                            CountryCode = await reader.IsDBNullAsync(0) ? null : reader.GetString(0),
                            IdbCode = await reader.IsDBNullAsync(1) ? null : reader.GetString(1),
                            NameEnglish = await reader.IsDBNullAsync(2) ? null : reader.GetString(2)
                        };

                        countries.Add(country);
                    }
                }
            }
            catch (SqlException sqlException)
            {
                throw sqlException;
            }
            catch (KeyVaultErrorException keyVaultException)
            {
                throw keyVaultException;
            }

            return countries;
        }

        public async Task<IReadOnlyCollection<Country>> GetCountries(string nmEn)
        {
            var countries = new List<Country>();
            try
            {
                var connectionString = this.keyVaultOptions.DefaultConnectionString;
                if (keyVaultOptions.Enabled)
                {
                    var secret = await keyVaultClient.GetSecretAsync($"{this.keyVaultOptions.Instance}secrets/{this.keyVaultOptions.SecretName}")
                            .ConfigureAwait(false);
                    connectionString = secret.Value;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    SqlCommand command = new SqlCommand("dbo.GetCountries", connection)
                    {
                        CommandType = CommandType.StoredProcedure,
                    };

                    command.Parameters.AddWithValue("@nmEn", nmEn);

                    var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        var country = new Country
                        {
                            CountryCode = await reader.IsDBNullAsync(0) ? null : reader.GetString(0),
                            IdbCode = await reader.IsDBNullAsync(1) ? null : reader.GetString(1),
                            NameEnglish = await reader.IsDBNullAsync(2) ? null : reader.GetString(2)
                        };

                        countries.Add(country);
                    }
                }
            }
            catch (SqlException sqlException)
            {
                throw sqlException;
            }
            catch (KeyVaultErrorException keyVaultException)
            {
                throw keyVaultException;
            }

            return countries;
        }

        public async Task<bool> Validate(IEnumerable<CountryItem> countries)
        {
            try
            {
                //if (keyVaultOption.Enabled)
                //{
                //    var secret = await keyVaultClient.GetSecretAsync($"{this.keyVaultOption.Instance}secrets/{this.keyVaultOption.SecretName}")
                //            .ConfigureAwait(false);

                //    using (SqlConnection connection = new SqlConnection(secret.Value))
                //    {
                //        await connection.OpenAsync();
                //        SqlCommand command = new SqlCommand("", connection)
                //        {
                //            CommandType = CommandType.StoredProcedure
                //        };

                //        command.Parameters.Add(new SqlParameter("", ""));
                //        await command.ExecuteNonQueryAsync();
                //    }
                //}
            }
            catch (KeyVaultErrorException keyVaultException)
            {
                throw keyVaultException;
            }

            return await Task.FromResult<bool>(true);
        }
    }
}
