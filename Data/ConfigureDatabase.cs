using Microsoft.EntityFrameworkCore;

namespace WebApi
{
    public static class ConfigureDatabase
    {
        public static void ConfigureDatabaseServices(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var serverVersion = new MySqlServerVersion(new Version(10, 4, 28));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connectionString, serverVersion));
        }
    }
}