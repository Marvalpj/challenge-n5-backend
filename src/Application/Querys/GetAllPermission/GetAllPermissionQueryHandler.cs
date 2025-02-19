using Application.Querys.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Querys.GetAllPermission
{
    public class GetAllPermissionQueryHandler : IRequestHandler<GetAllPermissionQuery, ErrorOr<IEnumerable<PermissionDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllPermissionQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public async Task<ErrorOr<IEnumerable<PermissionDto>>> Handle(GetAllPermissionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //await kafkaProducer.ProduceMessage("permission-topic", "get - permissions");

                IEnumerable<Permission> permission = await unitOfWork.Repository<Permission>().GetAllAsync(p => p.PermissionType);

                return permission.Select(p => new PermissionDto(
                    p.Id,
                    p.NameEmployee,
                    p.LastNameEmployee,
                    p.PermissionTypeId,
                    p.PermissionType.Description,
                    p.Date
                )).ToList();
            }
            catch (Exception ex)
            {
                return Error.Failure("CreatePermissionType.Failure", ex.Message);
            }
        }
    }
}
