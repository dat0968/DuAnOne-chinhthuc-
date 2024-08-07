using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Du_An_One.Models
{
    public class HOADON
    {
        [Key]
        [StringLength(5, ErrorMessage = "Mã hóa đơn phải có đúng 5 kí tự")]
        [Required(ErrorMessage = "Mã hóa đơn không được để trống")]
        public string? MaHoaDon { get; set; }

        [Required(ErrorMessage ="Địa chỉ đơn hàng không được để trống")]
        [StringLength(200)]
        public string? DiaChiNhanHang { get; set; }

        [Required(ErrorMessage = "Ngày tạo không được để trống")]
        public DateTime NgayTao { get; set; }

        [Required(ErrorMessage ="HTTT không được để trống")]
        [StringLength(25)]
        public string? HTTT { get; set; }

        [StringLength(30)]
        public string? TinhTrang { get; set; } = null;
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
