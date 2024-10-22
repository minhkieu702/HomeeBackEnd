using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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

    public virtual DbSet<Box> Boxes { get; set; }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<FavoritePost> FavoritePosts { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Place> Places { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("workstation id=homeedb.mssql.somee.com;packet size=4096;user id=quangminh_SQLLogin_1;pwd=at22vmjqnq;data source=homeedb.mssql.somee.com;persist security info=False;initial catalog=homeedb;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__349DA5A6CBA7BC63");

            entity.ToTable("Account");

            entity.Property(e => e.BirthDay).HasColumnType("datetime");
            entity.Property(e => e.CitizenId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.IsVerified).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.VerificationToken)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Box>(entity =>
        {
            entity.HasKey(e => e.BoxId).HasName("PK__Box__136CF764A047EDC6");

            entity.ToTable("Box");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.ContractId).HasName("PK__Contract__C90D3469DD732F05");

            entity.ToTable("Contract");

            entity.Property(e => e.Confirmed).HasDefaultValue(false);
            entity.Property(e => e.CreateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Renter).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.RenterId)
                .HasConstraintName("FK_Contract_Renter");

            entity.HasOne(d => d.Room).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK_Contract_Room");
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
            entity.Property(e => e.No).HasColumnName("NO");

            entity.HasOne(d => d.Post).WithMany(p => p.Images)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Image_Post");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__Message__C87C0C9C49175822");

            entity.ToTable("Message");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Messages)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Message__Account__7E37BEF6");

            entity.HasOne(d => d.Box).WithMany(p => p.Messages)
                .HasForeignKey(d => d.BoxId)
                .HasConstraintName("FK__Message__BoxId__7D439ABD");
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
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCF0FAA15ED");

            entity.Property(e => e.OrderId).ValueGeneratedNever();
            entity.Property(e => e.ExpiredAt).HasColumnType("datetime");
            entity.Property(e => e.PaymentId).HasMaxLength(255);
            entity.Property(e => e.SubscribedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Owner).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK_Owner");

            entity.HasOne(d => d.Subscription).WithMany(p => p.Orders)
                .HasForeignKey(d => d.SubscriptionId)
                .HasConstraintName("FK_Subscription");
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
            entity.Property(e => e.Title).HasMaxLength(500);

            entity.HasOne(d => d.Room).WithMany(p => p.Posts)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK_Post_Room");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__Room__328639196F4E51A9");

            entity.ToTable("Room");

            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.Area).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ElectricAmount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.IsBlock).HasDefaultValue(false);
            entity.Property(e => e.RentAmount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.RoomName).HasMaxLength(255);
            entity.Property(e => e.ServiceAmount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.WaterAmount).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Place).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.PlaceId)
                .HasConstraintName("FK__Room__PlaceId__74AE54BC");
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
