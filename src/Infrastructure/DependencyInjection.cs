using Application.Interface;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPersistence(configuration);

            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // inyecta base de datos
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("database")));
            
            // unit of work 
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IElasticsearchRepository, ElasticsearchRepository>();

            // inyecta repositorios 
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // servicio de kafka 
            services.AddSingleton<IKafkaProducer>(p => new KafkaProducer(configuration["kafka:bootstrapServer"]));
            
            // cliente de elastic
            services.AddSingleton<ElasticClientService>();
            services.AddSingleton(sp => sp.GetRequiredService<ElasticClientService>().Client);


            return services;
        }
    }
}
