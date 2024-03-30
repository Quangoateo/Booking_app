using System;

namespace BookingApp.Models
{
    public class RolePermission
    {
        public int ID { get; set; }
        public string RoleGuid { get; set; }
        public int ResourceID{ get; set; }
        public bool CanCreate { get; set; }
        public bool CanRead { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public int CreateBy { get; set; }
        public bool Status { get; set; }
        public string Guid { get; set; }
    }
}
