using System;
using System.Collections.Generic;
using BookingApp.Models;

namespace BookingApp.DTO
{
    public partial class RoomDto
    {
        public int ID { get; set; }
        public string FloorGuid { get; set; }
        public int FloorNum { get; set; }
        public string BuildingGuid { get; set; }
        public string BuildingName { get; set; }
        public int RoomNum { get; set; }
        public int RoomTypeID { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public string FacilityListGuid { get; set; }
        public List<FacilityDto> Facilities { get; set; } // from FacilityListGuid -> FacilityGuid -> Facility
        //public int NumberFacilities { get; set; } // from FacilityListGuid - Room2Facility
        public DateTime? CreateDate { get; set; }
        public int CreateBy { get; set; }
        public bool Status { get; set; }
        public string Guid { get; set; }
    }
}
