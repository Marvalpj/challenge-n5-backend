using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public sealed class PermissionType
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public ICollection<Permission>? Permissions { get; set; } 
        public PermissionType()
        {
            Permissions = new List<Permission>();
        }
        public PermissionType(string description)
        {
            Description = description;
        }
        public PermissionType(long id, string description)
        {
            Id = id;
            Description = description;
        }
    }
}
