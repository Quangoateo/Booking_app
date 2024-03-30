using System;

namespace BookingApp.DTO
{
    public class FacilityDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public int Number { get; set; } // this is for get room only
        public DateTime? CreateDate { get; set; }
        public int CreateBy { get; set; }
        public bool Status { get; set; }
        public string Guid { get; set; }
    }
}
