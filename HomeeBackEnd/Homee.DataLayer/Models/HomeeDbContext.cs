using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Homee.DataLayer.Models;

public partial class HomeedbContext : DbContext
{
    public HomeedbContext()
    {
    }

    public HomeedbContext(DbContextOptions<HomeedbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CategoryPlace> CategoryPlaces { get; set; }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<FavoritePost> FavoritePosts { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Place> Places { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Server=(local);Uid=sa;Pwd=1234567890;Database=HomeeDB; TrustServerCertificate=True");
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(GetConnectionString());
    }

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();
        var strConn = config.GetConnectionString("DefaultConnectionStringDB");
        return strConn;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__349DA5A6CBA7BC63");

            entity.ToTable("Account");

            entity.Property(e => e.BirthDay).HasColumnType("datetime");
            entity.Property(e => e.CitizenId).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.GoogleUserId).HasMaxLength(255);
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0B8D99E1F3");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryName).HasMaxLength(250);
        });

        modelBuilder.Entity<CategoryPlace>(entity =>
        {
            entity.HasKey(e => e.CategoryPlaceId).HasName("PK__Category__C882A4EB99D5600E");

            entity.ToTable("CategoryPlace");

            entity.HasOne(d => d.Category).WithMany(p => p.CategoryPlaces)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CategoryPlace_Category");

            entity.HasOne(d => d.Place).WithMany(p => p.CategoryPlaces)
                .HasForeignKey(d => d.PlaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CategoryPlace_Place");
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.ContractId).HasName("PK__Contract__C90D3469C5738CE8");

            entity.ToTable("Contract");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Place).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.PlaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Contract_Place");

            entity.HasOne(d => d.Render).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.RenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Contract_Render");
        });

        modelBuilder.Entity<FavoritePost>(entity =>
        {
            entity.HasKey(e => new { e.AccountId, e.PostId }).HasName("PK__Favorite__AE3C83A72EBB023E");

            entity.ToTable("FavoritePost");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.FavoritePosts)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FavoritePost_Account");

            entity.HasOne(d => d.Post).WithMany(p => p.FavoritePosts)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FavoritePost_Post");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__Image__7516F70C67857F52");

            entity.ToTable("Image");

            entity.Property(e => e.ImageUrl).HasMaxLength(255);

            entity.HasOne(d => d.Post).WithMany(p => p.Images)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Image_Post");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E123DFEE9ED");

            entity.ToTable("Notification");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UrlPath).HasMaxLength(255);

            entity.HasOne(d => d.Account).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_Account");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BCF5B4B450D");

            entity.ToTable("Order");

            entity.Property(e => e.ExpiredAt).HasColumnType("datetime");
            entity.Property(e => e.PaymentId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("paymentId");
            entity.Property(e => e.SubscribedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Owner).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Owner");

            entity.HasOne(d => d.Subscription).WithMany(p => p.Orders)
                .HasForeignKey(d => d.SubscriptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Subscription");
        });

        modelBuilder.Entity<Place>(entity =>
        {
            entity.HasKey(e => e.PlaceId).HasName("PK__Place__D5222B6EE2245CCC");

            entity.ToTable("Place");

            entity.Property(e => e.Distinct).HasMaxLength(255);
            entity.Property(e => e.Number).HasMaxLength(50);
            entity.Property(e => e.Province).HasMaxLength(255);
            entity.Property(e => e.Street).HasMaxLength(255);
            entity.Property(e => e.Ward).HasMaxLength(255);

            entity.HasOne(d => d.Owner).WithMany(p => p.Places)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Place_Owner");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__Post__AA12601842D5C110");

            entity.ToTable("Post");

            entity.Property(e => e.PostedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Place).WithMany(p => p.Posts)
                .HasForeignKey(d => d.PlaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Post_Place");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.SubscriptionId).HasName("PK__Subscrip__9A2B249DCF1AAF82");

            entity.ToTable("Subscription");

            entity.Property(e => e.SubscriptionName).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
