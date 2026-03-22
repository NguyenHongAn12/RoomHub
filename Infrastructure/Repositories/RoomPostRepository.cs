using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RoomPostRepository : IRoomPostRepository
    {
        private readonly ApplicationDbContext _context;

        public RoomPostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Room>> GetAllActiveAsync()
        {
            return await _context.Rooms.AsNoTracking()  // Tăng tốc độ đọc dữ liệu
        .AsSplitQuery()  // Tách truy vấn, chống giật lag và TimeOut
                .Include(r => r.Floor)
                    .ThenInclude(f => f.Building)
                .Include(r => r.RoomAmenities)
                    .ThenInclude(ra => ra.Amenity)
                .Include(r => r.RoomPhotos)
                .Where(r => !r.IsDeleted && r.Status == Domain.Enums.RoomStatus.Active)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetByLandlordIdAsync(string landlordId)
        {
            return await _context.Rooms
                .Include(r => r.Floor)
                    .ThenInclude(f => f.Building)
                .Include(r => r.RoomAmenities)
                    .ThenInclude(ra => ra.Amenity)
                .Include(r => r.RoomPhotos)
                .Where(r => r.LandlordId == landlordId && !r.IsDeleted)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Room?> GetByIdAsync(int id)
        {
            return await _context.Rooms
                .Include(r => r.Floor)
                    .ThenInclude(f => f.Building)
                .Include(r => r.Landlord)
                .Include(r => r.RoomAmenities)
                    .ThenInclude(ra => ra.Amenity)
                .Include(r => r.RoomPhotos)
                .Include(r => r.Deposits)
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        }

        public async Task AddAsync(Room room)
        {
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Room room)
        {
            room.IsDeleted = true;
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Floor>> GetFloorsByLandlordIdAsync(string landlordId)
        {
            return await _context.Floors
                .Include(f => f.Building)
                .Where(f => f.Building.OwnerId == landlordId && !f.IsDeleted && !f.Building.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Amenity>> GetAllAmenitiesAsync()
        {
            return await _context.Amenities.ToListAsync();
        }

        /// <summary>
        /// Tìm kiếm phòng theo từ khóa và/hoặc tỉnh thành từ DB thực sự.
        /// Keyword: so khớp Title, Description, Building.Name, Building.Address.
        /// Province: so khớp Building.Province hoặc Building.City.
        /// </summary>
        public async Task<IEnumerable<Room>> SearchAsync(string? keyword, string? province, Domain.Enums.RoomType? roomType = null)
        {
            var query = _context.Rooms
                .Include(r => r.Floor)
                    .ThenInclude(f => f.Building)
                .Include(r => r.RoomAmenities)
                    .ThenInclude(ra => ra.Amenity)
                .Include(r => r.RoomPhotos)
                .Where(r => !r.IsDeleted && r.Status == Domain.Enums.RoomStatus.Active)
                .AsQueryable();

            // Filter theo keyword với Accent-Insensitive (Tiếng Việt không dấu)
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(r =>
                    EF.Functions.Collate(r.Title, "SQL_Latin1_General_CP1_CI_AI").Contains(keyword) ||
                    (r.Description != null && EF.Functions.Collate(r.Description, "SQL_Latin1_General_CP1_CI_AI").Contains(keyword)) ||
                    EF.Functions.Collate(r.Floor.Building.Name, "SQL_Latin1_General_CP1_CI_AI").Contains(keyword) ||
                    EF.Functions.Collate(r.Floor.Building.Address, "SQL_Latin1_General_CP1_CI_AI").Contains(keyword) ||
                    EF.Functions.Collate(r.Floor.Building.District, "SQL_Latin1_General_CP1_CI_AI").Contains(keyword) ||
                    EF.Functions.Collate(r.Floor.Building.Ward, "SQL_Latin1_General_CP1_CI_AI").Contains(keyword)
                );
            }

            // Filter theo tỉnh/thành phố (Accent-Insensitive)
            if (!string.IsNullOrWhiteSpace(province))
            {
                query = query.Where(r =>
                    (r.Floor.Building.Province != null && EF.Functions.Collate(r.Floor.Building.Province, "SQL_Latin1_General_CP1_CI_AI").Contains(province)) ||
                    EF.Functions.Collate(r.Floor.Building.City, "SQL_Latin1_General_CP1_CI_AI").Contains(province)
                );
            }

            // Filter theo loại phòng
            if (roomType.HasValue)
            {
                query = query.Where(r => r.RoomType == roomType.Value);
            }

            return await query
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
    }
}
