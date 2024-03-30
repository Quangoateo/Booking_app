using System;

namespace BookingApp.Models
{
    public class Booking
    {
        public int ID { get; set; }
        public string UserGuid { get; set; }
        public string CampusGuid { get; set; }
        public string BuildingGuid { get; set; }
        public string FloorGuid { get; set; }
        public string RoomGuid { get; set; }
        public DateTime? BookingDate { get; set; }
        public string BookingTimeS { get; set; }
        public string BookingTimeE { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public int BookingStatus { get; set; }
        public DateTime? CreateDate { get; set; }
        public int CreateBy { get; set; }
        public bool Status { get; set; }
        public string BookingGuid { get; set; }
    }
}
