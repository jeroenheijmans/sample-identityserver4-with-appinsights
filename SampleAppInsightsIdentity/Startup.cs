using IdentityServer4.Models;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SampleAppInsightsIdentity
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Needed to get options other than InstrumentationKey from appsettings, see also: https://github.com/microsoft/ApplicationInsights-dotnet/issues/1448#issuecomment-276574251
            services.Configure<ApplicationInsightsServiceOptions>(configuration.GetSection("ApplicationInsights"));
            
            services.AddApplicationInsightsTelemetry();

            services.AddIdentityServer()
                .AddInMemoryApiResources(new[] { new ApiResource("foo-api") })
                .AddInMemoryClients(new[]
                {
                    new Client
                    {
                        ClientId = "b2bclient",
                        ClientSecrets = { new Secret("b2bsecret".Sha256()) },
                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        AllowedScopes = { "foo-api" },
                    },
                })
                .AddDeveloperSigningCredential();

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.Audience = "foo-api";
                });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseIdentityServer();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
