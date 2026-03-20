using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Identity extension
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        // Property hierarchy
        public DbSet<Building> Buildings => Set<Building>();
        public DbSet<Floor> Floors => Set<Floor>();
        public DbSet<Amenity> Amenities => Set<Amenity>();

        // Rooms
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<RoomAmenity> RoomAmenities => Set<RoomAmenity>();
        public DbSet<RoomPhoto> RoomPhotos => Set<RoomPhoto>();

        // Tenant profile
        public DbSet<TenantProfile> TenantProfiles => Set<TenantProfile>();

        // Deposit & Contract
        public DbSet<Deposit> Deposits => Set<Deposit>();
        public DbSet<Contract> Contracts => Set<Contract>();

        // Billing
        public DbSet<UtilityReading> UtilityReadings => Set<UtilityReading>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();
        public DbSet<Payment> Payments => Set<Payment>();

        // Maintenance & Services
        public DbSet<MaintenanceTicket> MaintenanceTickets => Set<MaintenanceTicket>();
        public DbSet<Service> Services => Set<Service>();
        public DbSet<ServiceRequest> ServiceRequests => Set<ServiceRequest>();

        // Communication & Reviews
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<Review> Reviews => Set<Review>();

        // Notification & Audit
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        // AI support
        public DbSet<SearchHistory> SearchHistories => Set<SearchHistory>();
        public DbSet<BookingHistory> BookingHistories => Set<BookingHistory>();

        // System
        public DbSet<SystemSetting> SystemSettings => Set<SystemSetting>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Apply all IEntityTypeConfiguration from this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // ==========================================
            // SEED DATA DÙNG ĐỂ TEST
            // ==========================================

            string ownerId = "test-user-id-123";

            // 1. Tạo User Chủ trọ (Property Owner)
            builder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = ownerId,
                UserName = "owner_test",
                NormalizedUserName = "OWNER_TEST",
                Email = "owner@roomhub.com",
                NormalizedEmail = "OWNER@ROOMHUB.COM",
                FullName = "Chủ Trọ Test",
                EmailConfirmed = true,
                PhoneNumber = "0123456789",
                PasswordHash = "AQAAAAEAACcQAAAAE...", // Password giả định
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            });

            // 2. Gán Role "PropertyOwner" cho User này
            // Lưu ý: "owner-role-id" là Id của Role PropertyOwner đã có sẵn trong ModelSnapshot của bạn
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "owner-role-id",
                UserId = ownerId
            });

            // 3. Tạo Tòa nhà (Building)
            builder.Entity<Building>().HasData(new Building
            {
                Id = 1,
                OwnerId = ownerId,
                Name = "Chung cư mini RoomHub",
                Address = "123 Đường Test",
                City = "Hồ Chí Minh",
                District = "Quận 1",
                Ward = "Phường Bến Nghé",
                CreatedAt = new DateTime(2026, 3, 9)
            });

            // 4. Tạo Tầng (Floor)
            builder.Entity<Floor>().HasData(new Floor
            {
                Id = 1,
                BuildingId = 1,
                FloorNumber = 1,
                Description = "Tầng 1 (Trệt)",
                CreatedAt = new DateTime(2026, 3, 9)
            });

            // 5. Tạo Phòng (Room)
            builder.Entity<Room>().HasData(new Room
            {
                Id = 1,
                FloorId = 1,
                LandlordId = ownerId,
                Title = "Phòng trọ cao cấp cửa sổ thoáng mát",
                RoomNumber = "101",
                RoomType = Domain.Enums.RoomType.Other, // Hoặc Studio, Căn hộ... tùy enum/logic của bạn
                BasePrice = 3500000m,
                Status = Domain.Enums.RoomStatus.Available,
                SurfaceArea = 25.5m,
                MaxCapacity = 2,
                IsFurnished = true,
                Description = "Phòng mới xây, dọn vào ở ngay.",
                CreatedAt = new DateTime(2026, 3, 9)
            });

            // 6. Gán Tiện ích (RoomAmenities) cho Phòng 1
            // Id 1: Air Conditioning, Id 2: WiFi (Lấy từ DB đã seed của bạn)
            builder.Entity<RoomAmenity>().HasData(
                new RoomAmenity { RoomId = 1, AmenityId = 1 },
                new RoomAmenity { RoomId = 1, AmenityId = 2 }
            );
        }
    }
}
