using Application.Commands.CreatePermission;
using Application.Interface;
using Domain.Entities;
using Domain.Errors;
using Domain.Interfaces;
using MediatR;

namespace Application.Permission.UnitTests
{
    public class CreatePermissionCommandHandlerUnitTest
    {
        private readonly Mock<IUnitOfWork> mockUnitOfWork;
        private readonly Mock<IElasticsearchRepository> mockElasticsearchRepository;
        private readonly Mock<IKafkaProducer> mockKafkaProducer;
        private readonly CreatePermissionCommandHandler handler;
        public CreatePermissionCommandHandlerUnitTest()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockElasticsearchRepository = new Mock<IElasticsearchRepository>();
            mockKafkaProducer = new Mock<IKafkaProducer>();
            handler = new CreatePermissionCommandHandler(
                mockUnitOfWork.Object,
                mockElasticsearchRepository.Object,
                mockKafkaProducer.Object
            );

            mockKafkaProducer.Setup(k => k.ProduceMessage(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            mockElasticsearchRepository.Setup(e => e.Index(It.IsAny<Domain.Entities.Permission>()))
                .ReturnsAsync(true);

        }

        [Fact]
        public async Task Handle_ShouldReturnUnitValue_WhenValidRequest()
        {
            // Arrange
            var command = new CreatePermissionCommand("John", "Doe", 1, DateTime.Now);
            var permissionType = new PermissionType(1, "Descripcion");

            mockUnitOfWork.Setup(u => u.Repository<PermissionType>().GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(permissionType);
            
            mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            
            mockUnitOfWork.Setup(u => u.Repository<Domain.Entities.Permission>().AddAsync(It.IsAny<Domain.Entities.Permission>()))
            .Returns(Task.CompletedTask);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsError.Should().BeFalse();
            result.Value.Should().Be(Unit.Value);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenNameEmployeeIsEmpty()
        {
            // Arrange
            var command = new CreatePermissionCommand(string.Empty, "Doe", 1, DateTime.Now);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(DomainError.Permission.PermissionNameIsEmpty);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenLastNameEmployeeIsEmpty()
        {
            // Arrange
            var command = new CreatePermissionCommand("John", string.Empty, 1, DateTime.Now);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().Be(DomainError.Permission.PermissionLastNameIsEmpty);
        }

        [Fact]
        public async Task Handle_ShouldReturnError_WhenPermissionTypeIdDoesNotExist()
        {
            // Arrange
            var command = new CreatePermissionCommand("John", "Doe", 1, DateTime.Now);

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
