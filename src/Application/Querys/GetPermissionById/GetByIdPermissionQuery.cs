using Application.Querys.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Querys.GetPermissionById
{
    public class GetByIdPermissionQuery : IRequest<ErrorOr<PermissionDto>>
    {
        public long Id { get; set; }
        public GetByIdPermissionQuery(long id)
        {
            Id = id;
        }
    }
}
