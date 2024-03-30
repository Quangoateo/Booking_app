namespace BookingApp.DTO.Filter
{
#nullable enable
    public class RoomFilter
    {
        public string? BuildingGuid { get; set; }
        public string? FloorGuid { get; set; }
        public string? CampusGuid { get; set; }
        public int? RoomTypeID { get; set; }
        public string? RoomGuid { get; set; }
    }
}
