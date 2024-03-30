using System;

namespace BookingApp.Models
{
    public class Role
    {
        public int ID { get; set; }
        public string RoleName { get; set; }
        public DateTime? CreateDate { get; set; }
        public int CreateBy { get; set; }
        public bool Status { get; set; }
        public string Guid { get; set; }
    }
}