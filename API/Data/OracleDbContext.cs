using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using NetUtility;
using BookingApp.Helpers;
using BookingApp.Models;

#nullable disable

namespace BookingApp.Data
{
    public partial class OracleDbContext : DbContext
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public OracleDbContext(DbContextOptions<OracleDbContext> options, IHttpContextAccessor contextAccessor = null) : base(options)
        {
            _contextAccessor = contextAccessor;
        }
        
    }
}
