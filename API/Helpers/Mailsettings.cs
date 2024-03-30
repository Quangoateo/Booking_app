using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.Helpers
{
    public class Mailsettings
    {
        public string API_URL { get; set; }
        public string Server { get; set; }
        public string UseDefaultCredentials { get; set; }
        public string Port { get; set; }
        public string EnableSsl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string TimeAsync { get; set; }
    }
}
