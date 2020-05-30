using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace KeyVault.Operations
{
    public class KeyVaultOps
    {
        private static string _dns = "{0}:KeyVault:DNS";
        private static string _clientId = "{0}:KeyVault:ClientId";
        private static string _clientSecrete = "{0}:KeyVault:ClientSecrete";

        public static IConfigurationRoot RefreshKeyVaults(string appName)
        {
            try
            {
                LoadAppVariables(appName);

                // create IConfigurationRoot to read appsetting.json
                IConfigurationRoot _setting = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                // create IConfigurationRoot to read Azure key vault
                IConfigurationRoot config = new ConfigurationBuilder()
                                                          .AddAzureKeyVault(
                                                             _setting[_dns],
                                                            _setting[_clientId],
                                                            _setting[_clientSecrete],
                                                             new DefaultKeyVaultSecretManager())
                                                          .Build();

                return config;
            }
            catch(Exception e)
            {
                throw e;
            }
            
        }

        private static void LoadAppVariables(string appName)
        {
            _dns = string.Format(_dns, appName);
            _clientId = string.Format(_clientId, appName);
            _clientSecrete = string.Format(_clientSecrete, appName);
        }
    }
}
