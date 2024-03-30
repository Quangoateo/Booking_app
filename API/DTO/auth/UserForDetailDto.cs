using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.DTO.auth
{
    public class UserForDetailDto
    {
        public object ID { get; set; }
        public string Guid { get; set; }
        public string UID { get; set; }
        public string Username { get; set; }
        public string AccountName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string GroupCode { get; set; }
        public string IsLineAccount { get; set; }
        public bool SubscribeLine { get; set; }
        public decimal? MobileMode { get; set; }
        public object GroupID { get; set; }
        public object PageSizeSetting { get; set; }
        public object PageSizeSettingValue { get; set; }
        public object PageSizeSettingList { get; set; }
    }
}
