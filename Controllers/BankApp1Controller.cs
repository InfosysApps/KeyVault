using System;
using KeyVault.Apps;
using KeyVault.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace KeyVault.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankApp1Controller : ControllerBase
    {
        private readonly ILogger<BankApp1Controller> _logger;
        private IConfigurationRoot _config;
        private const string _appName = "BankApp1";

        public BankApp1Controller(ILogger<BankApp1Controller> logger)
        {
            _logger = logger;
        }

        [Route("yml")]
        [HttpGet]
        public string GetDBPasswordFromYmlConfigFile()
        {
            //Consume db password from .yml file and use in the application.
            return "Oracle Password:  cGFzc3dvcmQ=";
        }

        [Route("keyvault")]
        [HttpGet]
        public BankApp1Secretes GetSecretesFromKeyVault()
        {
            //Consume Secretes from Azure Key Vault for BankApp1.
            _config = KeyVaultOps.RefreshKeyVaults(_appName);

            BankApp1Secretes secretes = new BankApp1Secretes();
            secretes.ConnectionString = _config["ConnectionString"];
            secretes.Username = _config["DBActiveUser"];
            secretes.Password = _config[_config["DBActiveUser"]];

            return secretes;
        }
    }
}
