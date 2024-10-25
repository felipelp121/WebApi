using WebApi.Repository;
using WebApi.Repository.Interface;

namespace WebApi.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IAlunoRepository, AlunoRepository>();
            services.AddScoped<ITurmaRepository, TurmaRepository>();
            services.AddScoped<IAlunoTurmaRepository, AlunoTurmaRepository>();
            
            return services;
        }
    }
}