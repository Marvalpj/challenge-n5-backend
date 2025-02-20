using Application.Interface;
using Elastic.Clients.Elasticsearch;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Permission.IntegrationsTests
{
    public class IntegrationTestFixture : IDisposable
    {
        public ApplicationDbContext DbContext { get; private set; }
        public IKafkaProducer KafkaProducer { get; private set; }
        public Mock<IElasticsearchRepository> elasticClientService { get; private set; }
        public IntegrationTestFixture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

            DbContext = new ApplicationDbContext(options);

            KafkaProducer = new Mock<IKafkaProducer>().Object;
            
            elasticClientService = new Mock<IElasticsearchRepository>();
            elasticClientService.Setup(e => e.GetallAsync())
                .ReturnsAsync((List<Domain.Entities.Permission>)null);
            elasticClientService.Setup(e => e.Index(It.IsAny<Domain.Entities.Permission>()))
                .ReturnsAsync(true);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            this.DbContext.PermissionTypes.AddRange(new List<Domain.Entities.PermissionType>
            {
                new Domain.Entities.PermissionType(1, "Read"),
                new Domain.Entities.PermissionType(2, "Write")
            });

            this.DbContext.Permissions.AddRange(new List<Domain.Entities.Permission>
            {
                new Domain.Entities.Permission(1, "John", "Doe", 1, DateTime.Now),
                new Domain.Entities.Permission(2, "Jane", "Doe", 2, DateTime.Now)
            });

            DbContext.SaveChanges();
        }

        public void Dispose()
        {
            this.DbContext.Database.EnsureDeleted();
            this.DbContext.Dispose();
        }
    }
}
