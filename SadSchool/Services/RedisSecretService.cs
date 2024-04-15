// <copyright file="RedisSecretService.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Services
{
    using Azure.Identity;
    using Azure.Security.KeyVault.Secrets;

    /// <summary>
    /// Manages redis secrets.
    /// </summary>
    public class RedisSecretService : ISecretService
    {
        private readonly string destVariableName = "EnvVarKeys";
        private readonly string keyName = "RedisSecretName";

        /// <summary>
        /// Gets the secret data.
        /// </summary>
        /// <returns>String with redis conn string.</returns>
        public string? GetSecret()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var confManager = new ConfigurationManager();
            confManager.AddConfiguration(configuration);

            var varKeyVault = confManager[this.destVariableName];

            if (string.IsNullOrEmpty(varKeyVault))
            {
                return null;
            }

            var keyVaultDest = Environment.GetEnvironmentVariable(varKeyVault);
            var redisSecretName = confManager[this.keyName];

            if (string.IsNullOrEmpty(keyVaultDest) || string.IsNullOrEmpty(redisSecretName))
            {
                return null;
            }

            var keyVaultClient = new SecretClient(new Uri(keyVaultDest), new DefaultAzureCredential());

            try
            {
                var redisSecret = keyVaultClient.GetSecret(redisSecretName);
                return redisSecret.Value.Value;
            }
            catch
            {
                return null;
            }
        }
    }
}
