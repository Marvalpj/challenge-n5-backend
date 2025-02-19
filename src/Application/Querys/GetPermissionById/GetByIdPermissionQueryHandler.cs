using Application.Querys.Dtos;
using Domain.Entities;
using Domain.Errors;
using Domain.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Querys.GetPermissionById
{
    public sealed class GetByIdPermissionQueryHandler : IRequestHandler<GetByIdPermissionQuery, ErrorOr<PermissionDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetByIdPermissionQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public async Task<ErrorOr<PermissionDto>> Handle(GetByIdPermissionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //await kafkaProducer.ProduceMessage("permission-topic", "get - permission");

                if (await unitOfWork.Repository<Permission>().GetByIdAsync(request.Id, p => p.PermissionType) is not Permission p)
                    return DomainError.Permission.PermissionIdDoesNotExist;

                return new PermissionDto(
                    p.Id,
                    p.NameEmployee,
                    p.LastNameEmployee,
                    p.PermissionTypeId,
                    p.PermissionType.Description,
                    p.Date
                );
            }
            catch (Exception ex)
            {
                return Error.Failure("CreatePermissionType.Failure", ex.Message);
            }
        }
    }
}
