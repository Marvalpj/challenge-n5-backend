using Domain.Interfaces;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // inyecta repositorios 
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // inyecta servicios
            //services.AddScoped<IKafkaProducer>(p => new KafkaProducer(configuration["kafka:bootstrapServer"]));

            //var settingsElasticS = new ElasticsearchClientSettings(new Uri(configuration["elasticsearch:uri"]))
            //    .Authentication(new BasicAuthentication(configuration["elasticsearch:username"], configuration["elasticsearch:password"]))
            //    .ServerCertificateValidationCallback(CertificateValidations.AllowAll)
            //    .DefaultIndex("permission");

            //services.AddSingleton(new ElasticsearchClient(settingsElasticS));

            //services.AddScoped(typeof(IElasticRepository<>), typeof(ElasticRepository<>));

            return services;
        }
    }
}
