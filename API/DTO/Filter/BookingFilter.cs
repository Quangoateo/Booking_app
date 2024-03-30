using System;

namespace BookingApp.DTO.Filter
{
    public class BookingFilter
    {
        public string CampusGuid { get; set; }
        public string BuildingGuid { get; set; }
        public string FloorGuid { get; set; }
        public string RoomGuid { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
