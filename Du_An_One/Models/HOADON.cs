using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Du_An_One.Models
{
    public class HOADON
    {
        [Key]
        [StringLength(5)]
        public string? MaHoaDon { get; set; }

        [Required]
        [StringLength(200)]
        public string? DiaChiNhanHang { get; set; }

        [Required]
        public DateTime NgayTao { get; set; }

        [Required]
        [StringLength(25)]
        public string? HTTT { get; set; }

        [StringLength(30)]
        public string? TinhTrang { get; set; } = "Chuẩn bị hàng";
        [StringLength(5)]
        [ForeignKey("MaNV")]
        public string? MaNV { get; set; }
        public NHANVIEN? NHANVIEN { get; set; }
        [StringLength(5)]
        [ForeignKey("MaKH")]
        public string? MaKH {  get; set; } 
        public KHACHHANG? KHACHHANG { get; set; }
        public ICollection<CHITIETHOADON>? CHITIETHOADONs { get; set; }
    }
}
