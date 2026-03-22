using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace Domain.Enums
{
    public enum RoomType
    {
        [Display(Name = "Phòng đơn")]
        Single,
        [Display(Name = "Phòng đôi")]
        Double,
        [Display(Name = "Căn hộ Studio")]
        Studio,
        [Display(Name = "Phòng ở ghép")]
        Shared,
        [Display(Name = "Căn hộ Duplex")]
        Duplex,
        [Display(Name = "Nhà nguyên căn")]
        WholeHouse,
        [Display(Name = "Khác")]
        Other
    }
}
