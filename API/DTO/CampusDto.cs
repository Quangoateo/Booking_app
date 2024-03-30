using System;

namespace BookingApp.DTO
{
    public class CampusDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime? CreateDate { get; set; }
        public int CreateBy { get; set; }
        public bool Status { get; set; }
        public string RoomGuid { get; set; }
    }
}
