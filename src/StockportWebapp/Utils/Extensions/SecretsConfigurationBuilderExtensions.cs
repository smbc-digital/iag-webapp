using Amazon.SecretsManager.Model;
using StockportWebapp.Configuration;

namespace StockportWebapp.Utils.Extensions
{
    public static class SecretsConfigurationBuilderExtensions
    {
        public static WebApplicationBuilder AddSecrets(this WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.WebHost.ConfigureAppConfiguration((hostingContext, configBuilder) =>
            {
                if (hostingContext.HostingEnvironment.IsDevelopment() || hostingContext.HostingEnvironment.EnvironmentName.Equals("local"))
                {
                    Log.Logger.Information($"USING LOCAL SECRETS");

                    // Use local secrets for development - need to work out what the difference is between the name/format of local json secrets string
                    configBuilder.AddUserSecrets<Program>();
                }
                else
                {
                    Log.Logger.Information($"USING AWS SECRETS MANAGER");
                    // In AWS
                    // secrets will take the form of {env}/{group}/{secret__name} e.g. int/iag/MySecret__AccessKey = MySecret:AccessKey in dev secrets
                    configBuilder.AddAwsSecrets(hostingContext);
                }
            });

            return webApplicationBuilder;
        }

        public static IConfigurationBuilder AddAwsSecrets(this IConfigurationBuilder configurationBuilder, WebHostBuilderContext hostingContext)
        {
            IConfiguration partialConfig = configurationBuilder.Build();

            var secretConfig = new AWSSecretsManagerConfiguration();
            partialConfig
                .GetSection("AWSSecretsManagerConfiguration")
                .Bind(secretConfig);

            var allowedPrefixes = GetSecretPrefixes(secretConfig, hostingContext.HostingEnvironment.EnvironmentName);

            configurationBuilder.AddSecretsManager(configurator: opts =>
            {
                opts.SecretFilter = entry => HasPrefix(allowedPrefixes, entry);
                opts.KeyGenerator = (entry, key) => GenerateKey(allowedPrefixes, key);
                opts.PollingInterval = TimeSpan.FromMinutes(30);
            });

            return configurationBuilder;
        }

        public static List<string> GetSecretPrefixes(AWSSecretsManagerConfiguration secretConfig, string env)
        {
            // Gets a list of required prefixes for this env based on the value in the appsettings e.g. "int/iag/",
            // i.e. secrets that are specific to the env and group/application
            var allowedPrefixes = secretConfig.SecretGroups
                                    .Select(grp => $"{env}/{grp}/").ToList();

                // The intention for this is to allow secrets to be shared between groups of environments
                // e.g. pre-prod/iag - could be secrets shared between int/qa
                if (!string.IsNullOrEmpty(secretConfig.EnvironmentGroupPrefix))
                    secretConfig.SecretGroups.ToList().ForEach(grp => allowedPrefixes.Add($"{secretConfig.EnvironmentGroupPrefix}/{grp}/"));

                // Gets a list of shared prefixes for this group based on the value in the appsettings 
                // i.e. secrets that are universal to all envs of that app/group
                // e.g. "iag/shared" 
                if (!string.IsNullOrEmpty(secretConfig.SharedSecretPrefix))
                    secretConfig.SecretGroups.ToList().ForEach(grp => allowedPrefixes.Add($"{grp}/{secretConfig.SharedSecretPrefix}/"));

                // Gets a list of shared prefixes for this group based on the value in the appsettings  
                // i.e. secrets that are universal to all apps in that environment
                // e.g. "int/shared"
                if (!string.IsNullOrEmpty(secretConfig.SharedSecretPrefix))
                    allowedPrefixes.Add($"{env}/{secretConfig.SharedSecretPrefix}/");

                // Adds global secrets to the list of allowed prefixes
                // E.G. global/
            if (!string.IsNullOrEmpty(secretConfig.GlobalSecretPrefix))
                allowedPrefixes.Add($"{secretConfig.GlobalSecretPrefix}/");


            allowedPrefixes.ForEach(prefix => Log.Logger.Information($"SecretsConfigurationBuilderExtensions : GetSecretPrefixes : ALLOWED PREFIX : {prefix}"));
            


            return allowedPrefixes;
        }

        // Only load entries that start with any of the allowed prefixes
        private static bool HasPrefix(List<string> allowedPrefixes, SecretListEntry entry)
        {
            return allowedPrefixes.Any(prefix => entry.Name.StartsWith(prefix));
        }

        // Strip the prefix and replace '__' with ':'
        private static string GenerateKey(IEnumerable<string> prefixes, string secretValue)
        {
            // We know one of the prefixes matches, this assumes there's only one match,
            // So don't use '/' in your environment or secretgroup names!
            var prefix = prefixes.First(secretValue.StartsWith);
            
            // Strip the prefix, and replace "__" with ":"
            var key = secretValue
                        .Substring(prefix.Length)
                        .Replace("__", ":");

            Log.Logger.Information($"SecretsConfigurationBuilderExtensions : GenerateKey : Key : {key}");

            return key
        }
    }
}
