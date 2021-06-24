using System;
using NETAPI.Configuration;

namespace XamCore.Services
{
    public class ApiConfiguration : ApiConfigurationBase<ApiEnvironment, ApiEndpoint>
    {
        public ApiConfiguration()
        {
        }

        public override void Configure()
        {
            // Add your envrionments
            AddEnvironment(ApiEnvironment.Dev, "https://localhost:52669");
            AddEnvironment(ApiEnvironment.Staging, "https://uat.api.com");
            AddEnvironment(ApiEnvironment.Production, "https://prod.api.com");

            // Set the current environment
            SetCurrentEnvironment(ApiEnvironment.Dev);

            // Add headers for each endpoint
            AddHeader("Content-Type", "application/json");

            // Set max concurrent requests for each endpoint
            SetMaxConcurrentRequests(2);

            // Set max concurrent requests per endpoint
            SetMaxConcurrentRequests(ApiEndpoint.AccountLogin, 1);
            SetMaxConcurrentRequests(ApiEndpoint.AccountRegister, 1);
        }
    }
}
