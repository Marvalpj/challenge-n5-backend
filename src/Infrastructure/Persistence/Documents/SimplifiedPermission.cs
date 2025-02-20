using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Documents
{
    public class SimplifiedPermission
    {
        public long Id { get; set; }
        public string NameEmployee { get; set; }
        public string LastNameEmployee { get; set; }
        public long PermissionTypeId { get; set; }
        public DateTime Date { get; set; }
        
        public SimplifiedPermission()
        {
            
        }

        public SimplifiedPermission(long id, string nameEmployee, string lastNameEmployee, long permissionTypeId, DateTime date)
        {
            Id = id;
            NameEmployee = nameEmployee;
            LastNameEmployee = lastNameEmployee;
            PermissionTypeId = permissionTypeId;
            Date = date;
        }
    }
}
