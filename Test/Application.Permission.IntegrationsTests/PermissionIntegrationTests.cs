using Application.Commands.CreatePermission;
using Application.Interface;
using Application.Querys.GetAllPermission;
using Application.Querys.GetPermissionById;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;

namespace Application.Permission.IntegrationsTests
{
    public class PermissionIntegrationTests : IClassFixture<IntegrationTestFixture>
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IElasticsearchRepository elasticClientService;
        private readonly IKafkaProducer kafkaProducer;
        private readonly GetByIdPermissionQueryHandler getByIdHandler;
        private readonly GetAllPermissionQueryHandler getAllHandler;
        private readonly CreatePermissionCommandHandler createPermissionCommandHandler;
        public PermissionIntegrationTests(IntegrationTestFixture fixture)
        {
            dbContext = fixture.DbContext;
            elasticClientService = fixture.elasticClientService.Object;
            kafkaProducer = fixture.KafkaProducer;

            var unitOfWork = new UnitOfWork(dbContext); 
            var elasticsearchRepository = elasticClientService; 

            getByIdHandler = new GetByIdPermissionQueryHandler(unitOfWork, elasticsearchRepository, kafkaProducer);
            getAllHandler = new GetAllPermissionQueryHandler(unitOfWork, elasticsearchRepository, kafkaProducer);
            createPermissionCommandHandler = new CreatePermissionCommandHandler(unitOfWork, elasticsearchRepository, kafkaProducer);
        }

        [Fact]
        public async Task GetByIdPermission_ShouldReturnPermissionDto_WhenPermissionExists()
        {
            // Arrange
            var query = new GetByIdPermissionQuery(1);

            // Act
            var result = await getByIdHandler.Handle(query, CancellationToken.None);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetAllPermissions_ShouldReturnAllPermissions()
        {
            // Act
            var result = await getAllHandler.Handle(new GetAllPermissionQuery(), CancellationToken.None);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().HaveCount(2);
        }

        [Fact]
        public async Task Handle_ShouldAddPermission_WhenValidRequest()
        {
            // Arrange
            var command = new CreatePermissionCommand("prueba", "create", 1, DateTime.Now);

            // Act
            var result = await createPermissionCommandHandler.Handle(command, CancellationToken.None);

            // Assert
            result.IsError.Should().BeFalse();
            var permission = dbContext.Permissions.FirstOrDefault(p => p.NameEmployee == "prueba" && p.LastNameEmployee == "create" && p.PermissionTypeId == 1);
            permission.Should().NotBeNull();
        }
    }
}
