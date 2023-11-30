using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Core.Entities;
using Entities.Hotels;
using Entities.Reservations;
using Entities.Rooms;
using Entities.Payments;

namespace DataAccess
{
    public class HotelReservationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public HotelReservationDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PostgreSQL"));

        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var datas = ChangeTracker.Entries<BaseEntity>();
            foreach (var data in datas)
            {
                _ = data.State switch
                {
                    EntityState.Added => data.Entity.CreatedDate = DateTime.UtcNow,
                    EntityState.Modified => data.Entity.UpdatedDate = DateTime.UtcNow,
                    _ => DateTime.UtcNow,
                };
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Reservation>()
           .HasOne(e => e.Customer)
           .WithMany()
           .HasForeignKey(e => e.CustomerId);
        }
        DbSet<Hotel> Hotels { get; set; }
        DbSet<Payment> Payments { get; set; }
        DbSet<Reservation> Reservations { get; set; }
        DbSet<Room> Rooms { get; set; }
        DbSet<AppUser> Users { get; set; }
    }
}
