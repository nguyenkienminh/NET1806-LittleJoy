using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NET1806_LittleJoy.Repository.Entities;

public partial class LittleJoyContext : DbContext
{
    public LittleJoyContext()
    {
    }

    public LittleJoyContext(DbContextOptions<LittleJoyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<AgeGroupProduct> AgeGroupProducts { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Origin> Origins { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PointMoney> PointMoneys { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Refund> Refunds { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Otp> Otps { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Address__3214EC074F7046C4");

            entity.ToTable("Address");

            entity.Property(e => e.Address1).HasColumnName("Address");

            entity.HasOne(d => d.User).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Address__UserId__66603565");
        });

        modelBuilder.Entity<AgeGroupProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AgeGroup__3214EC07A8D27E3F");

            entity.ToTable("AgeGroupProduct");

            entity.Property(e => e.AgeRange).HasMaxLength(250);
            
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Brand__3214EC079B39557F");

            entity.ToTable("Brand");

            entity.Property(e => e.BrandName).HasMaxLength(250);
        });

        modelBuilder.Entity<Otp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OTP__3214EC079B39557F");

            entity.ToTable("Otp");

            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.OTPCode).HasMaxLength(6);
            entity.Property(e => e.IsUsed);
            entity.Property(e => e.OTPTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07644223FC");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryName).HasMaxLength(500);
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__3214EC07E0A2CFC9");

            entity.ToTable("Feedback");

            entity.Property(e => e.Comment).HasMaxLength(500);

            entity.HasOne(d => d.Product).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__Produc__6754599E");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__UserId__68487DD7");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC077E9989C3");

            entity.ToTable("Order");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.DeliveryStatus).HasMaxLength(250);
            entity.HasOne(d => d.Payment).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentId)
                .HasConstraintName("FK__Order__PaymentId__6A30C649");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__UserId__6B24EA82");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3214EC07BF4BE7FD");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__Order__6C190EBB");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__OrderDeta__Produ__6D0D32F4");
        });

        modelBuilder.Entity<Origin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Origin__3214EC072BFD6020");

            entity.ToTable("Origin");

            entity.Property(e => e.OriginName).HasMaxLength(250);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payment__3214EC072E9667DA");

            entity.ToTable("Payment");

            entity.Property(e => e.Method).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(250);
        });

        modelBuilder.Entity<PointMoney>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PointMon__3214EC076B2C3B19");

            entity.ToTable("PointMoney");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Post__3214EC0772DC9C94");

            entity.ToTable("Post");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(500);
            entity.Property(e => e.UnsignTitle).HasMaxLength(500);

            entity.HasOne(d => d.User).WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Post__UserId__6E01572D");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3214EC07F9237F13");

            entity.ToTable("Product");

            entity.Property(e => e.ProductName).HasMaxLength(500);
            entity.Property(e => e.UnsignProductName).HasMaxLength(500);

            entity.HasOne(d => d.Age).WithMany(p => p.Products)
                .HasForeignKey(d => d.AgeId)
                .HasConstraintName("FK__Product__AgeId__6EF57B66");

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK__Product__BrandId__6FE99F9F");

            entity.HasOne(d => d.Cate).WithMany(p => p.Products)
                .HasForeignKey(d => d.CateId)
                .HasConstraintName("FK__Product__CateId__70DDC3D8");

            entity.HasOne(d => d.Origin).WithMany(p => p.Products)
                .HasForeignKey(d => d.OriginId)
                .HasConstraintName("FK__Product__OriginI__71D1E811");
        });

        modelBuilder.Entity<Refund>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Refund__C3905BCF300B2A7A");

            entity.ToTable("Refund");

            entity.Property(e => e.OrderId).ValueGeneratedOnAdd();
            entity.Property(e => e.Reason).HasMaxLength(500);
            entity.Property(e => e.Status).HasMaxLength(500);

            entity.HasOne(d => d.Order).WithOne(p => p.Refund)
                .HasForeignKey<Refund>(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Refund__OrderId__72C60C4A");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC070DE1BF62");

            entity.ToTable("Role");

            entity.Property(e => e.RoleName).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC0785EC007A");

            entity.ToTable("User");

            entity.Property(e => e.Avatar).HasMaxLength(250);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Fullname).HasMaxLength(250);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.UnsignName).HasMaxLength(250);
            entity.Property(e => e.ConfirmEmail);
            entity.Property(e => e.TokenConfirmEmail).HasMaxLength(250);
            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__User__RoleId__73BA3083");
            
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
