using System.ComponentModel.DataAnnotations;

namespace Du_An_One.Models
{
    public class KHUYENMAI
    {
        [Key]
        [StringLength(5)]
        public string? MaKhuyenMai { get; set; }

        [Required]
        public int PhanTramKhuyenMai { get; set; }

        [Required]
        public DateTime ThoiGianStart { get; set; }

        [Required]
        public DateTime ThoiGianEnd { get; set; }
        public ICollection<SANPHAM>? SANPHAMs { get; set; }
    }
}
