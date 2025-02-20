using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public sealed class Permission
    {
        public long Id { get; private set; }
        public string NameEmployee { get; private set; }
        public string LastNameEmployee { get; private set; }
        public long PermissionTypeId { get; private set; }
        public DateTime Date { get; private set; }
        public PermissionType PermissionType { get; private set; }

        public Permission()
        {
            
        }
        public Permission(string nameEmployee, string lastNameEmployee, long permissionTypeId, DateTime date)
        {
            NameEmployee = nameEmployee;
            LastNameEmployee = lastNameEmployee;
            PermissionTypeId = permissionTypeId;
            Date = date;
        }

        public Permission(long id, string nameEmployee, string lastNameEmployee, long permissionTypeId, DateTime date, PermissionType permissionType)
        {
            Id = id;
            NameEmployee = nameEmployee;
            LastNameEmployee = lastNameEmployee;
            PermissionTypeId = permissionTypeId;
            Date = date;
            PermissionType = permissionType;
        }
        public Permission(long id, string nameEmployee, string lastNameEmployee, long permissionTypeId, DateTime date)
        {
            Id = id;
            NameEmployee = nameEmployee;
            LastNameEmployee = lastNameEmployee;
            PermissionTypeId = permissionTypeId;
            Date = date;
        }

        public void UpdatePermission(string? nameEmployee, string? lastNameEmployee, long? permissionTypeId, DateTime? date)
        {
            if (nameEmployee != null)
                this.NameEmployee = nameEmployee;

            if (lastNameEmployee != null)
                this.LastNameEmployee = lastNameEmployee;

            if (permissionTypeId != null)
                this.PermissionTypeId = permissionTypeId.Value;

            if (date != null)
                this.Date = date.Value;
        }
    }
}
