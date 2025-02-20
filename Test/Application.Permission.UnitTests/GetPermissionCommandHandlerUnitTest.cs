using Application.Interface;
using Application.Querys.Dtos;
using Application.Querys.GetAllPermission;
using Application.Querys.GetPermissionById;
using Domain.Errors;
using Domain.Interfaces;
using Moq;

namespace Application.Permission.UnitTests
{
    public class GetPermissionCommandHandlerUnitTest
    {
        private readonly Mock<IUnitOfWork> mockUnitOfWork;
        private readonly Mock<IElasticsearchRepository> mockElasticsearchRepository;
        private readonly Mock<IKafkaProducer> mockKafkaProducer;
        private readonly GetByIdPermissionQueryHandler handler;
        private readonly GetAllPermissionQueryHandler handlerGetAll;
        public GetPermissionCommandHandlerUnitTest()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockElasticsearchRepository = new Mock<IElasticsearchRepository>();
            mockKafkaProducer = new Mock<IKafkaProducer>();
            handler = new GetByIdPermissionQueryHandler(mockUnitOfWork.Object, mockElasticsearchRepository.Object, mockKafkaProducer.Object);
            handlerGetAll = new GetAllPermissionQueryHandler(mockUnitOfWork.Object, mockElasticsearchRepository.Object, mockKafkaProducer.Object);

            mockKafkaProducer.Setup(k => k.ProduceMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            mockElasticsearchRepository.Setup(e => e.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((Domain.Entities.Permission)null);

            mockElasticsearchRepository.Setup(e => e.GetallAsync())
                .ReturnsAsync((List<Domain.Entities.Permission>) null);
        }

        [Fact]
        public async Task Handle_ShouldReturnPermissionDto_WhenPermissionExists()
        {
            // Arrange
            var query = new GetByIdPermissionQuery(1);
            var permission = new Domain.Entities.Permission(1, "John", "Doe", 1, DateTime.Now);
            var permissionDto = new PermissionDto(permission.Id, permission.NameEmployee, permission.LastNameEmployee, permission.PermissionTypeId, permission.Date);

            mockUnitOfWork.Setup(u => u.Repository<Domain.Entities.Permission>().GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(permission);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().BeEquivalentTo(permissionDto);
        }
        [Fact]
        public async Task Handle_ShouldReturnAllPermissionsFromElasticsearch_WhenPermissionsExist()
        {
            DateTime now = DateTime.Now;
            // Arrange
            var permissions = new List<Domain.Entities.Permission>
            {
                new Domain.Entities.Permission(1, "John", "Doe", 1, now),
                new Domain.Entities.Permission(2, "Jane", "Doe", 2, now)
            };

            var permissionsDto = permissions.Select(p => new PermissionDto(p.Id, p.NameEmployee, p.LastNameEmployee, p.PermissionTypeId, p.Date)).ToList();

            mockUnitOfWork.Setup(u => u.Repository<Domain.Entities.Permission>().GetAllAsync())
                .ReturnsAsync(permissions);

            // Act
            var result = await handlerGetAll.Handle(new GetAllPermissionQuery(), CancellationToken.None);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().BeEquivalentTo(permissionsDto);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenPermissionDoesNotExist()
        {
            // Arrange
            var query = new GetByIdPermissionQuery(1);

            mockUnitOfWork.Setup(u => u.Repository<Domain.Entities.Permission>().GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync((Domain.Entities.Permission) null);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(DomainError.Permission.PermissionIdDoesNotExist);
        }


    }
}
