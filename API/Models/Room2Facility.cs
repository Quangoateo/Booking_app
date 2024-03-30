using System;

namespace BookingApp.Models
{
    public class Room2Facility
    {
        public int ID { get; set; }
        public string FacilityGuid { get; set; }
        public string RoomGuid { get; set; }
        public int Number { get; set; }
        public DateTime? CreateDate { get; set; }
        public int CreateBy { get; set; }
        public bool Status { get; set; }
        public string Guid { get; set; }
    }
}
