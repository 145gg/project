using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using project_ver1.Models;

namespace project_ver1.Models
{
    public class Room_Order_DetailsDbContext : DbContext
    {
        public Room_Order_DetailsDbContext(DbContextOptions<Room_Order_DetailsDbContext> options)
            : base(options)
        {
        }
        public DbSet<Room_Order_Details> Room_Order_Details { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 設定 Room_Order_Details 實體的主鍵
            modelBuilder.Entity<Room_Order_Details>()
                .HasKey(ro => new { ro.OrderID, ro.RoomID });

            Rooms rooms = new Rooms();
            Room_Order room_order = new Room_Order();

            // 設定 OrderID 外鍵
            modelBuilder.Entity<Room_Order_Details>()
                .HasOne<Room_Order>()
                .WithMany()
                .HasForeignKey(ro => ro.OrderID);

            // 設定 RoomID 外鍵
            modelBuilder.Entity<Room_Order_Details>()
                .HasOne<Rooms>()
                .WithMany()
                .HasForeignKey(ro => ro.RoomID);
        }

    }
}
