using System;

namespace BookingApp.Models
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime? TimeStamp { get; set; }
        public int UserID { get; set; }
        public string EventType { get; set; }
        public string EventName { get; set; }
        public string Status { get; set; }

    }
}
