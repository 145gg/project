using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using project_ver1.Models;

namespace project_ver1.Models
{

    public class RoomDbContext : DbContext
    {
        public RoomDbContext(DbContextOptions<RoomDbContext> options)
            : base(options)
        {

        }
        public DbSet<Room_Category> Room_Category { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room_Category>().HasKey(e => e.CategoryID);
        }
        public DbSet<Room_Order> Room_Order { get; set; }
        public DbSet<Room_Order_Details> Room_Order_Details { get; set; }

        public DbSet<Rooms> Rooms { get; set; }

    }
}
