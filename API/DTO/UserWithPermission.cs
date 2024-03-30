using System;
using System.Collections.Generic;

namespace BookingApp.DTO
{
    public class UserWithPermission
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string RoleName { get; set; }
        public int StaffCode { get; set; }
        public string LdapName { get; set; }
        public List<RolePermissionDto> Permissions { get; set; }
    }
}
