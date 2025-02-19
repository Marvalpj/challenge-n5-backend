using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Errors
{
    public static class DomainError
    {
        public static class Permission
        {
            public static Error PermissionIdDoesNotExist =>
                Error.Validation("Permission.NotFound", "El permiso con el id proporcionado no existe");
            public static Error PermissionNameIsEmpty =>
                Error.Validation("Permission.NameEmployee", "Debe enviar el nombre de la descripcion");
            public static Error PermissionLastNameIsEmpty =>
                Error.Validation("Permission.LastNameEmployee", "Debe enviar el nombre de la descripcion");

        }

        public static class PermissionType
        {
            public static Error PermissionTypeIdDoesNotExist =>
                Error.Validation("PermissionType.NotFound", "El tipo de permiso con el id proporcionado no existe");

        }
    }
}
