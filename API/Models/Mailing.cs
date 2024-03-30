using BookingApp.Models.Abstracts;
using BookingApp.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.Models
{
    [Table("Mailings")]
    public class Mailing: AuditEntity
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(1000)]
        public string Url { get; set; }
        [MaxLength(255)]
        public string Frequency { get; set; }
        [MaxLength(255)]
        public string Report { get; set; }
        public int AccountID { get; set; }
        public DateTime TimeSend { get; set; }
   
        
    }
}
