using AutoMapper;
using BookingApp.Data;
using BookingApp.Models;
using BookingApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BookingApp.Helpers.ScheduleQuartz
{
    [DisallowConcurrentExecution] //Add this so jobs are not scheduled more than once at a time
    public class SendMailJob : IJob
    {
        ////repository
        //private readonly IRepositoryBase<BookingDetail> _repoBooking;
        //private readonly IRepositoryBase<EmailPool> _repoEmailPool;

        ////servie

        //private readonly IBookingService _serviceBooking;

        ////
        //private readonly IUnitOfWork _unitOfWork;
        //private readonly IMapper _mapper;
        //private readonly IMailExtension emailService;

        //IConfiguration _configuration;
        //BookingAppContext _context;
        //public SendMailJob(IMailExtension emailService)
        //{
        //    this.emailService = emailService;
        //}

        private readonly ILogger<SendMailJob> _logger;

        public SendMailJob(ILogger<SendMailJob> logger)
        {
            _logger = logger;
        }
        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Hello from job logger!");
            return Task.CompletedTask;
        }

        public  async Task Execute2(IJobExecutionContext context)
        {
            try
            {
                JobDataMap dataMap = context.JobDetail.JobDataMap;
                string API_URL = dataMap.GetString("API_URL");
                string UseDefaultCredentials = dataMap.GetString("UseDefaultCredentials");
                string Port = dataMap.GetString("Port");
                string Server = dataMap.GetString("Server");
                string EnableSsl = dataMap.GetString("EnableSsl");
                string UserName = dataMap.GetString("UserName");
                string Password = dataMap.GetString("Password");
                string FromEmail = dataMap.GetString("FromEmail");
                string FromName = dataMap.GetString("FromName");
                var subject = "Ly Booking";
                var message = "Please refer to the Ly Booking";
                var persionalEmail = "huuquynhit97@gmail.com";



                SmtpClient client = new SmtpClient(Server)
                {

                    UseDefaultCredentials = bool.Parse(UseDefaultCredentials),
                    Port = int.Parse(Port),
                    EnableSsl = bool.Parse(EnableSsl),
                    Credentials = new NetworkCredential(UserName, Password)
                };

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(FromEmail, FromName),
                };
                mailMessage.Body = message;
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = MailPriority.High;
                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                try
                {
                    mailMessage.To.Add(persionalEmail);
                    client.Send(mailMessage);
                    Console.WriteLine($"Send - success");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine($"Send - fail");
                }
                //await _emailService.SendEmailAsync(persionalEmail, subject, message);




            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("The system can not send mail");
            }
        }

        
    }
}
