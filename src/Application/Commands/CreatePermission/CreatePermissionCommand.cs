using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreatePermission
{
    public class CreatePermissionCommand : IRequest<ErrorOr<Unit>>
    {
        public string NameEmployee { get; private set; }
        public string LastNameEmployee { get; private set; }
        public long PermissionTypeId { get; private set; }
        public DateTime Date { get; private set; }

        public CreatePermissionCommand(string nameEmployee, string lastNameEmployee, long permissionTypeId, DateTime date)
        {
            NameEmployee = nameEmployee;
            LastNameEmployee = lastNameEmployee;
            PermissionTypeId = permissionTypeId;
            Date = date;
        }
    }
}
