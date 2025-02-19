using Application.Querys.Dtos;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Querys.GetAllPermission
{
    public class GetAllPermissionQuery : IRequest<ErrorOr<IEnumerable<PermissionDto>>>
    {
        public GetAllPermissionQuery()
        {

        }
    }
}
