using System.ComponentModel.DataAnnotations;

namespace Du_An_One.Models
{
    public class NHACUNGCAP
    {
        [Key]
        [StringLength(5)]
        public string? MaNhaCC { get; set; }

        [Required]
        [StringLength(60)]
        public string? TenNhaCC { get; set; }

        [Required]
        [StringLength(200)]
        public string? DiaChi { get; set; }

        [Required]
        [StringLength(40)]
        public string? Email { get; set; }

        [Required]
        [StringLength(11)]
        public string? SDT { get; set; }

        [Required]
        public DateTime NgayThanhLap { get; set; }

        [Required]
        [StringLength(40)]
        public string? NguoiDaiDien { get; set; }

        [Required]
        public DateTime ThoiGianCungCap { get; set; }

        [StringLength(25)]
        public string? TinhTrang { get; set; }
        public ICollection<CHITIETNHAP>? CHITIETNHAPs { get; set; }
    }
}
