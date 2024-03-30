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
    public partial class BookingAppContext : DbContext
    {

            private readonly IHttpContextAccessor _contextAccessor;

            /// <summary>
            /// Initializes a new instance of the <see cref="BookingAppContext"/> class.
            /// </summary>
            /// <param name="options">The options for configuring the context.</param>
            /// <param name="contextAccessor">The HTTP context accessor.</param>
            public BookingAppContext(DbContextOptions<BookingAppContext> options,
                IHttpContextAccessor contextAccessor = null) : base(options)
            {
                _contextAccessor = contextAccessor;
            }

            // DbSet properties representing database tables
            public virtual DbSet<Building> Building { get; set; }
            public virtual DbSet<User> User { get; set; }
            public virtual DbSet<Room> Room { get; set; }
            public virtual DbSet<Floor> Floor { get; set; }
            public virtual DbSet<Log> Log { get; set; }
            public virtual DbSet<Booking> Booking { get; set; }
            public virtual DbSet<Campus> Campus { get; set; }
            public virtual DbSet<Room2Facility> Room2Facility { get; set; }
            public virtual DbSet<Facility> Facility { get; set; }
            public virtual DbSet<Role> Role { get; set; }
            public virtual DbSet<RolePermission> RolePermission { get; set; }

            //protected virtual DbSet<RefreshToken> RefreshTokens { get; set; }

            /// <summary>
            /// Configures the model for the context.
            /// </summary>
            /// <param name="modelBuilder">The model builder to use.</param>
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.HasAnnotation("Relational:Collation", "English_United States.1252");

                // Configure entity mappings
                modelBuilder.Entity<Building>(entity =>
                {
                    entity.Property(e => e.BuildingGuid)
                        .HasColumnName("BuildingGuid")
                        .HasMaxLength(50);
                    entity.ToTable("Building");
                });
                modelBuilder.Entity<Campus>(entity =>
                {
                    entity.Property(e => e.Guid)
                        .HasColumnName("Guid")
                        .HasMaxLength(50);
                    entity.ToTable("Campus");
                });
                modelBuilder.Entity<User>(entity =>
                {
                    entity.Property(e => e.LdapName)
                        .HasColumnName("LdapName")
                        .HasMaxLength(50);
                    entity.ToTable("User");
                });
                modelBuilder.Entity<Room>(entity =>
                {
                    entity.Property(e => e.Guid)
                        .HasColumnName("Guid")
                        .HasMaxLength(50);
                    entity.ToTable("Room");
                });
                modelBuilder.Entity<Floor>(entity =>
                {
                    entity.Property(e => e.Guid)
                        .HasColumnName("Guid")
                        .HasMaxLength(50);
                    entity.ToTable("Floor");
                });
                modelBuilder.Entity<Booking>(entity =>
                {
                    entity.Property(e => e.BookingGuid)
                        .HasColumnName("BookingGuid")
                        .HasMaxLength(50);
                });
                modelBuilder.Entity<Facility>(entity =>
                {
                    entity.Property(e => e.Guid)
                        .HasColumnName("Guid")
                        .HasMaxLength(50);
                    entity.ToTable("Facility");
                });
                modelBuilder.Entity<Room2Facility>(entity =>
                {
                    entity.Property(e => e.Guid)
                        .HasColumnName("Guid")
                        .HasMaxLength(50);
                    entity.ToTable("Room2Facility");
                });
                modelBuilder.Entity<Role>(entity =>
                {
                    entity.Property(e => e.Guid)
                        .HasColumnName("Guid")
                        .HasMaxLength(50);
                    entity.ToTable("Role");
                });
                modelBuilder.Entity<RolePermission>(entity =>
                {
                    entity.Property(e => e.Guid)
                        .HasColumnName("Guid")
                        .HasMaxLength(50);
                    entity.ToTable("Role_Permission");
                });
                OnModelCreatingPartial(modelBuilder);
            }

            partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

            /// <summary>
            /// Saves all changes made in this context to the underlying database asynchronously.
            /// </summary>
            /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
            /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
            public override async Task<int> SaveChangesAsync(
                CancellationToken cancellationToken = default(CancellationToken))
            {
                // Automatically update create and update dates
                AutoAddDateTracking();
                return (await base.SaveChangesAsync(true, cancellationToken));
            }

    #if DEBUG
            /// <summary>
            /// Configures the options for the context in debug mode.
            /// </summary>
            /// <param name="optionsBuilder">The options builder to use.</param>
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                => optionsBuilder.LogTo(Console.WriteLine).EnableSensitiveDataLogging().EnableDetailedErrors();
    #endif

            /// <summary>
            /// Automatically adds date tracking properties to the entity being added or modified.
            /// </summary>
            public void AutoAddDateTracking()
            {
                var modified = ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);
                foreach (EntityEntry item in modified)
                {
                    var changedOrAddedItem = item.Entity;
                    if (changedOrAddedItem != null)
                    {
                        if (item.State == EntityState.Added)
                        {
                            SetValueProperty(ref changedOrAddedItem, "CreateDate", "CreateBy");
                        }

                        if (item.State == EntityState.Modified)
                        {
                            SetValueProperty(ref changedOrAddedItem, "UpdateDate", "UpdateBy");
                            SetDeleteValueProperty(ref changedOrAddedItem);
                        }
                    }
                }
            }

            /// <summary>
            /// Gets the value of a property from an object.
            /// </summary>
            /// <param name="src">The object to get the property value from.</param>
            /// <param name="propName">The name of the property.</param>
            /// <returns>The value of the property.</returns>
            public decimal? GetPropValue(object src, string propName)
            {
                return (decimal?)src.GetType().GetProperty(propName).GetValue(src, null);
            }

            /// <summary>
            /// Sets the delete value properties of the entity being modified.
            /// </summary>
            /// <param name="changedOrAddedItem">The entity being modified.</param>
            public void SetDeleteValueProperty(ref object changedOrAddedItem)
            {
                string deleteDate = "DeleteDate";
                string status = "Status";
                string deleteBy = "DeleteBy";
                Type type = changedOrAddedItem.GetType();
                PropertyInfo propAdd = type.GetProperty(deleteDate);
                PropertyInfo propStatus = type.GetProperty(status);

                if (propStatus != null && propStatus.PropertyType.Name == "Decimal")
                {
                    var statusValue = (decimal?)propStatus.GetValue(changedOrAddedItem, null);
                    if (statusValue == 0)
                    {
                        if (propAdd != null)
                        {
                            propAdd.SetValue(changedOrAddedItem, DateTime.Now, null);
                        }

                        var httpContext = _contextAccessor.HttpContext;
                        if (httpContext != null)
                        {
                            var accessToken = httpContext.Request.Headers["Authorization"];
                            var accountID = JWTExtensions.GetDecodeTokenByID(accessToken).ToDecimal();
                            PropertyInfo propCreateBy = type.GetProperty(deleteBy);
                            if (propCreateBy != null)
                            {
                                if (accountID > 0)
                                {
                                    propCreateBy.SetValue(changedOrAddedItem, accountID, null);
                                }
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Sets the value properties for "create date" and "create by" of the entity being added.
            /// </summary>
            /// <param name="changedOrAddedItem">The entity being added.</param>
            /// <param name="propDate">The name of the create date property.</param>
            /// <param name="propUser">The name of the create by property.</param>
            public void SetValueProperty(ref object changedOrAddedItem, string propDate, string propUser)
            {
                Type type = changedOrAddedItem.GetType();
                PropertyInfo propAdd = type.GetProperty(propDate);
                if (propAdd != null)
                {
                    propAdd.SetValue(changedOrAddedItem, DateTime.Now, null);
                }

                var httpContext = _contextAccessor.HttpContext;
                if (httpContext != null)
                {
                    var accessToken = httpContext.Request.Headers["Authorization"];
                    var accountID = JWTExtensions.GetDecodeTokenByID(accessToken).ToDecimal();
                    PropertyInfo propCreateBy = type.GetProperty(propUser);
                    if (propCreateBy != null)
                    {
                        if (accountID > 0)
                        {
                            propCreateBy.SetValue(changedOrAddedItem, accountID, null);
                        }
                    }
                }
            }
        }
    }
