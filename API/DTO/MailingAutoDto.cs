
using BookingApp.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class MailingAutoDto
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
