using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Du_An_One.Models
{
    public class SANPHAM
    {
        [Key]
        [StringLength(5)]
        public string? MaSP { get; set; }

        [Required]
        [StringLength(40)]
        public string? TenSP { get; set; }

        [Required]
        public int SoLuongBan { get; set; }

        [Required]
        public double DonGiaBan { get; set; }

        [Required]
        public DateTime NgayNhap { get; set; }

        [Required]
        [StringLength(40)]
        public string? DanhMucHang { get; set; }
        [StringLength(50)]
        public string? HinhAnh { get; set; }

        [Required]
        [StringLength(3)]
        public string? KichCo {  get; set; }
        [Required]
        public string? MoTa { get; set; }
        [StringLength(5)]
        [ForeignKey("MaKhuyenMai")]
        public string? MaKhuyenMai { get; set; }
        public KHUYENMAI? KHUYENMAI { get; set; }
        [StringLength(5)]
        [ForeignKey("MaNV")]
        public string? MaNV { get; set; }
        public NHANVIEN? NHANVIEN { get; set; }

        [NotMapped] public IFormFile? FileImage { get; set; }
        [NotMapped] public List<IFormFile>? FileImages { get; set; } // New property for multiple images

        public ICollection<CHITIETHOADON>? CHITIETHOADONs { get; set; }
        public ICollection<CHITIETNHAP>? CHITIETNHAPs { get; set; }
        public ICollection<HINHANH>? HINHANHs { get; set; }
    }
}
