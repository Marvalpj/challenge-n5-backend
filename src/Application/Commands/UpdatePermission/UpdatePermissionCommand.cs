using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UpdatePermission
{
    public class UpdatePermissionCommand : IRequest<ErrorOr<Unit>>
    {
        public long Id { get; set; }
        public string? NameEmployee { get; private set; }
        public string? LastNameEmployee { get; private set; }
        public long? PermissionTypeId { get; private set; }
        public DateTime? Date { get; private set; }

        public UpdatePermissionCommand(long id, string? nameEmployee, string? lastNameEmployee, long? permissionTypeId, DateTime? date)
        {
            Id = id;
            NameEmployee = nameEmployee;
            LastNameEmployee = lastNameEmployee;
            PermissionTypeId = permissionTypeId;
            Date = date;
        }
    }
}
