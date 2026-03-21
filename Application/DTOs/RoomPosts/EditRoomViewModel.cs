using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.RoomPosts
{
    public class EditRoomViewModel
    {
        public int Id { get; set; }

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

        [Required(ErrorMessage = "Trường này là bắt buộc")]
        [Display(Name = "Tầng")]
        public int FloorId { get; set; }

        public IEnumerable<Floor> AvailableFloors { get; set; } = new List<Floor>();

        [Display(Name = "Trạng thái")]
        public RoomStatus Status { get; set; } = RoomStatus.Active;

        public List<int> SelectedAmenityIds { get; set; } = new List<int>();

        public List<Amenity> AvailableAmenities { get; set; } = new List<Amenity>();

        public List<RoomPhotoViewModel> ExistingPhotos { get; set; } = new List<RoomPhotoViewModel>();

        public List<int> DeletePhotoIds { get; set; } = new List<int>();

        [Display(Name = "Tải lên hình ảnh mới")]
        public List<IFormFile> NewPhotos { get; set; } = new List<IFormFile>();
    }
}