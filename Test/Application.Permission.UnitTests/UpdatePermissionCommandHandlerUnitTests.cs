using Application.Commands.UpdatePermission;
using Application.Interface;
using Domain.Entities;
using Domain.Errors;
using Domain.Interfaces;
using MediatR;

namespace Application.Permission.UnitTests
{
    public class UpdatePermissionCommandHandlerUnitTests
    {
        private readonly Mock<IUnitOfWork> mockUnitOfWork;
        private readonly Mock<IElasticsearchRepository> mockElasticsearchRepository;
        private readonly Mock<IKafkaProducer> mockKafkaProducer;
        private readonly UpdatePermissionCommandHandler handler;

        public UpdatePermissionCommandHandlerUnitTests()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockElasticsearchRepository = new Mock<IElasticsearchRepository>();
            mockKafkaProducer = new Mock<IKafkaProducer>();
            handler = new UpdatePermissionCommandHandler(
                mockUnitOfWork.Object,
                mockElasticsearchRepository.Object,
                mockKafkaProducer.Object
            );

            mockKafkaProducer.Setup(k => k.ProduceMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            mockElasticsearchRepository.Setup(e => e.Index(It.IsAny<Domain.Entities.Permission>()))
                .ReturnsAsync(true);

            mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task Handle_ShouldReturnUnitValue_WhenValidRequest()
        {
            // Arrange
            var command = new UpdatePermissionCommand(1, "Doe", "John", 2, DateTime.Now);
            var permission = new Domain.Entities.Permission(1, "John", "Doe", 1, DateTime.Now);
            var permissionType = new PermissionType(2, "Descripcion");

            mockUnitOfWork.Setup(u => u.Repository<Domain.Entities.Permission>().GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(permission);

            mockUnitOfWork.Setup(u => u.Repository<PermissionType>().GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(permissionType);

            mockUnitOfWork.Setup(u => u.Repository<Domain.Entities.Permission>().Update(It.IsAny<Domain.Entities.Permission>()));

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().Be(Unit.Value);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenPermissionIdDoesNotExist()
        {
            // Arrange
            var command = new UpdatePermissionCommand(1, "John", "Doe", 1, DateTime.Now);

            mockUnitOfWork.Setup(u => u.Repository<Domain.Entities.Permission>().GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync((Domain.Entities.Permission)null);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(DomainError.Permission.PermissionIdDoesNotExist);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenPermissionTypeIdDoesNotExist()
        {
            // Arrange
            var command = new UpdatePermissionCommand(1, "John", "Doe", 0, DateTime.Now);

            var permission = new Domain.Entities.Permission(1, "John", "Doe", 1, DateTime.Now);
            
            mockUnitOfWork.Setup(u => u.Repository<Domain.Entities.Permission>().GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(permission);

            mockKafkaProducer.Setup(k => k.ProduceMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            mockElasticsearchRepository.Setup(e => e.Index(It.IsAny<Domain.Entities.Permission>()))
                .ReturnsAsync(true);

            mockUnitOfWork.Setup(u => u.Repository<PermissionType>().GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync((PermissionType)null);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(DomainError.PermissionType.PermissionTypeIdDoesNotExist);
        }

    }
}
