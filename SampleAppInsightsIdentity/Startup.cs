using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SampleAppInsightsIdentity
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
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
            app.UseDeveloperExceptionPage();
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
