using System;

namespace BookingApp.DTO
{
    public class LogDto
    {
        public int Id { get; set; }
        public DateTime? TimeStamp { get; set; }
        //public int UserID { get; set; }
        public string UserName { get; set; }
        public string EventType { get; set; }
        public string EventName { get; set; }
        public string Status { get; set; }
    }
}
