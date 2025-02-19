using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Querys.Dtos
{
    public class PermissionDto
    {
        public long Id { get; set; }
        public string NameEmployee { get; set; }
        public string LastNameEmployee { get; set; }
        public long PermissionTypeId { get; set; }
        public string PermissionType { get; set; }
        public DateTime Date { get; set; }
        public string FullName => $"{NameEmployee} {LastNameEmployee}";
        public PermissionDto()
        {

        }
        public PermissionDto(long id, string nameEmployee, string lastNameEmployee, long permissionTypeId, string permissionType, DateTime date)
        {
            Id = id;
            NameEmployee = nameEmployee;
            LastNameEmployee = lastNameEmployee;
            PermissionTypeId = permissionTypeId;
            PermissionType = permissionType;
            Date = date;
        }
    }
}
