using System;

namespace BookingApp.Models
{
    public class Floor
    {
        public int ID { get; set; }
        public int FloorNum { get; set; }
        public string BuildingGuid { get; set; }
        public DateTime? CreateDate { get; set; }
        public int CreateBy { get; set; }
        public bool Status { get; set; }
        public string Guid { get; set; }
    }
}
