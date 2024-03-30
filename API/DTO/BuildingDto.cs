using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

#nullable disable

namespace BookingApp.DTO
{
    public partial class BuildingDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Block { get; set; }
        public string LocationX { get; set; }
        public string LocationY { get; set; }
        public DateTime? CreateDate { get; set; }
        public int CreateBy { get; set; }
        public bool Status { get; set; }
        public string BuildingGuid { get; set; }
    }
}
