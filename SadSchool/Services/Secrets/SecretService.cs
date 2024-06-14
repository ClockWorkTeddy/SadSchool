// <copyright file="SecretService.cs" company="ClockWorkTeddy">
// Written by ClockWorkTeddy.
// </copyright>

namespace SadSchool.Services.Secrets
{
    using Azure.Identity;
    using Azure.Security.KeyVault.Secrets;

    /// <summary>
    /// Manages redis secrets.
    /// </summary>
    public class SecretService : ISecretService
    {
        private readonly string destVariableName = "EnvVarKeys";
        private readonly ConfigurationManager confManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretService"/> class.
        /// </summary>
        /// <param name="configurationManager">configuration manager.</param>
        public SecretService(ConfigurationManager configurationManager)
        {
            this.confManager = configurationManager;
        }

        /// <summary>
        /// Gets the secret data.
        /// </summary>
        /// <param name="keyName">Key name.</param>
        /// <returns>String with redis conn string.</returns>
        public string? GetSecret(string keyName)
        {
            var varKeyVault = this.confManager[this.destVariableName];

            if (string.IsNullOrEmpty(varKeyVault))
            {
                return null;
            }

            var keyVaultDest = Environment.GetEnvironmentVariable(varKeyVault);
            var redisSecretName = this.confManager[keyName];

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
