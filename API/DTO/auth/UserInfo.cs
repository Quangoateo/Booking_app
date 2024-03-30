using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.DTO.auth
{
    public class UserInfo
    {
        public Dictionary<string, string> Attributes { get; set; }

        public UserInfo()
        {
            Attributes = new Dictionary<string, string>();
        }
    }
}
