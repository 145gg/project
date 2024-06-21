using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using project_ver1.Models;

namespace project_ver1.Models
{

    public class HomeDbContext : DbContext
    {
        public HomeDbContext(DbContextOptions<HomeDbContext> options)
            : base(options)
        {

        }
        public DbSet<Contact> Contact { get; set; }
        public virtual DbSet<Agricultural_Order> AgriculturalOrder { get; set; }

        public virtual DbSet<Agricultural_Order_Details> AgriculturalOrderDetail { get; set; }

        public virtual DbSet<Agricultural_Products> AgriculturalProduct { get; set; }

        public virtual DbSet<Agricultural_Products_Category> AgriculturalProductsCategory { get; set; }



        public virtual DbSet<Customer> Customer { get; set; }

        public virtual DbSet<Employee> Employee { get; set; }

        public virtual DbSet<Meal> Meal { get; set; }

        public virtual DbSet<MealOrder> MealOrder { get; set; }

        public virtual DbSet<MealOrderDetail> MealOrderDetail { get; set; }

        public virtual DbSet<News> News { get; set; }

        public virtual DbSet<Rooms> Rooms { get; set; }

        public virtual DbSet<Room_Category> RoomCategory { get; set; }

        public virtual DbSet<Room_Order> RoomOrder { get; set; }

        public virtual DbSet<Room_Order_Details> RoomOrderDetails { get; set; }

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agricultural_Order>(entity =>
            {
                entity.ToTable("Agricultural_Order");

                entity.Property(e => e.ID).HasColumnName("ID");
                entity.Property(e => e.CustomerID).HasColumnName("CustomerID");
                entity.Property(e => e.EmployeeID).HasColumnName("EmployeeID");
                entity.Property(e => e.OderTime).HasColumnType("datetime");

                entity.HasOne(d => d.Customer).WithMany(p => p.AgriculturalOrders)
                    .HasForeignKey(d => d.CustomerID)
                    .HasConstraintName("FK_Agricultural_Order_Customer");

                entity.HasOne(d => d.Employee).WithMany(p => p.AgriculturalOrders)
                    .HasForeignKey(d => d.EmployeeID)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Agricultural_Order_Employee");
            });

            modelBuilder.Entity<Agricultural_Order_Details>(entity =>
            {
                entity.HasKey(e => new { e.OrderID, e.ProductID });

                entity.ToTable("Agricultural_Order_Details");

                entity.Property(e => e.OrderID).HasColumnName("OrderID");
                entity.Property(e => e.ProductID).HasColumnName("ProductID");

                entity.HasOne(d => d.Order).WithMany(p => p.Agricultural_Order_Details)
                    .HasForeignKey(d => d.OrderID)
                    .HasConstraintName("FK_Agricultural_Order_Details_Agricultural_Order");

                entity.HasOne(d => d.Product).WithMany(p => p.AgriculturalOrderDetails)
                    .HasForeignKey(d => d.ProductID)
                    .HasConstraintName("FK_Agricultural_Order_Details_Agricultural_Products");
            });

            modelBuilder.Entity<Agricultural_Products>(entity =>
            {
                entity.ToTable("Agricultural_Products");

                entity.Property(e => e.ID).HasColumnName("ID");
                entity.Property(e => e.CategoryID).HasColumnName("CategoryID");
                entity.Property(e => e.Image)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category).WithMany(p => p.AgriculturalProducts)
                    .HasForeignKey(d => d.CategoryID)
                    .HasConstraintName("FK_Agricultural_Products_Agricultural_Products_Category");
            });

            modelBuilder.Entity<Agricultural_Products_Category>(entity =>
            {
                entity.ToTable("Agricultural_Products_Category");

                entity.Property(e => e.ID).HasColumnName("ID");
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contact");

                entity.Property(e => e.ID).HasColumnName("ID");
                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.QuestionCategory)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.QuestionContent)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.SetTime).HasColumnType("datetime");

                entity.HasOne(d => d.Employee).WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Contact_Employee");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.ID).HasColumnName("ID");
                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.Pwd)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.Property(e => e.ID).HasColumnName("ID");
                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.BirthDate).HasColumnType("smalldatetime");
                entity.Property(e => e.Department)
                    .HasMaxLength(30)
                    .IsUnicode(false);
                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.HireDate).HasColumnType("smalldatetime");
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Meal>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MealOrder>(entity =>
            {
                entity.ToTable("Meal_Order");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
                entity.Property(e => e.OderTime).HasColumnType("datetime");

                entity.HasOne(d => d.Customer).WithMany(p => p.MealOrders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Meal_Order_Customer");

                entity.HasOne(d => d.Employee).WithMany(p => p.MealOrders)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Meal_Order_Employee");
            });

            modelBuilder.Entity<MealOrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.MealId });

                entity.ToTable("Meal_Order_Details");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");
                entity.Property(e => e.MealId).HasColumnName("MealID");

                entity.HasOne(d => d.Meal).WithMany(p => p.MealOrderDetails)
                    .HasForeignKey(d => d.MealId)
                    .HasConstraintName("FK_Meal_Order_Details_Meals");

                entity.HasOne(d => d.Order).WithMany(p => p.MealOrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_Meal_Order_Details_Meal_Order");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CategoryID).HasColumnName("CategoryID");
                entity.Property(e => e.ID).HasColumnName("ID");
                entity.Property(e => e.Image)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.NewsContent).IsUnicode(false);
                entity.Property(e => e.NewsTitle)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.SetTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Rooms>(entity =>
            {
                entity.Property(e => e.ID).HasColumnName("ID");
                entity.Property(e => e.CategoryID).HasColumnName("CategoryID");

                entity.HasOne(d => d.Category).WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.CategoryID)
                    .HasConstraintName("FK_Rooms_Room_Category");
            });

            modelBuilder.Entity<Room_Category>(entity =>
            {
                entity.HasKey(e => e.CategoryID);

                entity.ToTable("Room_Category");

                entity.Property(e => e.CategoryID).HasColumnName("CategoryID");
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Room_Order>(entity =>
            {
                entity.ToTable("Room_Order");

                entity.Property(e => e.ID).HasColumnName("ID");
                entity.Property(e => e.CheckIn).HasColumnType("smalldatetime");
                entity.Property(e => e.CheckOut).HasColumnType("smalldatetime");
                entity.Property(e => e.CustomerID).HasColumnName("CustomerID");
                entity.Property(e => e.EmployeeID).HasColumnName("EmployeeID");
                entity.Property(e => e.OrderTime).HasColumnType("datetime");

                entity.HasOne(d => d.Customer).WithMany(p => p.RoomOrders)
                    .HasForeignKey(d => d.CustomerID)
                    .HasConstraintName("FK_Room_Order_Customer");

                entity.HasOne(d => d.Employee).WithMany(p => p.RoomOrders)
                    .HasForeignKey(d => d.EmployeeID)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Room_Order_Employee");
            });

            modelBuilder.Entity<Room_Order_Details>(entity =>
            {
                entity.HasKey(e => new { e.OrderID, e.RoomID });

                entity.ToTable("Room_Order_Details");

                entity.Property(e => e.OrderID).HasColumnName("OrderID");
                entity.Property(e => e.RoomID).HasColumnName("RoomID");

                entity.HasOne(d => d.Order).WithMany(p => p.RoomOrderDetails)
                    .HasForeignKey(d => d.OrderID)
                    .HasConstraintName("FK_Room_Order_Details_Room_Order");

                entity.HasOne(d => d.Room).WithMany(p => p.RoomOrderDetails)
                    .HasForeignKey(d => d.RoomID)
                    .HasConstraintName("FK_Room_Order_Details_Rooms");
            });

        }


    }
}
