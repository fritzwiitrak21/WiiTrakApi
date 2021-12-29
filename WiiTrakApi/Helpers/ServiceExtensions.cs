using Microsoft.EntityFrameworkCore;
using WiiTrakApi.Data;

namespace WiiTrakApi.Helpers
{
    public static class ServiceExtensions
    {
        public static void ConfigureCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(policy =>
            {
                policy.AddPolicy("CorsPolicy", options => options
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("totalAmountPages"));
            });
        }

        public static void ConfigureAddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
