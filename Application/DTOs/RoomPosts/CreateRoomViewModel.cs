using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.RoomPosts
{
    public class CreateRoomViewModel
    {
        [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
        [StringLength(200, ErrorMessage = "Tiêu đề không quá 200 ký tự")]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Số phòng là bắt buộc")]
        [StringLength(50)]
        [Display(Name = "Số phòng")]
        public string RoomNumber { get; set; } = null!;

        [Required(ErrorMessage = "Loại phòng là bắt buộc")]
        [Display(Name = "Loại phòng")]
        public RoomType RoomType { get; set; }


        [Required(ErrorMessage = "Giá thuê là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá thuê phải là số dương")]
        [Display(Name = "Giá thuê")]
        public decimal BasePrice { get; set; }

        [Range(0, 1000, ErrorMessage = "Diện tích phải từ 0 đến 1000")]
        [Display(Name = "Diện tích (m2)")]
        public decimal? SurfaceArea { get; set; }

        [Display(Name = "Số người tối đa")]
        [Range(1, 20)]
        public int MaxCapacity { get; set; } = 2;

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Đã có nội thất")]
        public bool IsFurnished { get; set; } = true;

        [Display(Name = "Sử dụng toà nhà mới")]
        public bool IsNewBuilding { get; set; }

        [Display(Name = "Tên toà nhà mới")]
        public string? NewBuildingName { get; set; }

        [Display(Name = "Địa chỉ")]
        public string? NewBuildingAddress { get; set; }

        [Display(Name = "Tỉnh/Thành phố")]
        public string? NewBuildingCity { get; set; }

        [Display(Name = "Quận/Huyện")]
        public string? NewBuildingDistrict { get; set; }

        [Display(Name = "Phường/Xã")]
        public string? NewBuildingWard { get; set; }

        [Display(Name = "Số tầng")]
        [Range(1, 100, ErrorMessage = "Số tầng phải từ 1 đến 100")]
        public int? NewFloorNumber { get; set; }

        [Required(ErrorMessage = "Trường này là bắt buộc")]
        [Display(Name = "Tầng")]
        public int FloorId { get; set; }

        public IEnumerable<Floor> AvailableFloors { get; set; } = new List<Floor>();

        [Display(Name = "Trạng thái")]
        public RoomStatus Status { get; set; } = RoomStatus.Active;

        public List<int> SelectedAmenityIds { get; set; } = new List<int>();

        public List<Amenity> AvailableAmenities { get; set; } = new List<Amenity>();

        [Display(Name = "Hình ảnh phòng")]
        public List<IFormFile> Photos { get; set; } = new List<IFormFile>();
    }
}