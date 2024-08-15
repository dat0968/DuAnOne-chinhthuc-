using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Du_An_One.Models
{
    public class SANPHAM
    {
        [Key]
        [StringLength(5, ErrorMessage ="Mã sản phẩm phải có đúng 5 kí tự")]
        [Required(ErrorMessage ="Mã sản phẩm không được để trống")]
        public string? MaSP { get; set; }

        [Required(ErrorMessage ="Tên sản phẩm không được để trống")]
        [StringLength(40)]
        public string? TenSP { get; set; }

        [Required(ErrorMessage ="Số lượng bán không được để trống")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Số lượng bán phải lớn hơn 0 và không được chứa ký tự đặc biệt ")]
        public int SoLuongBan { get; set; }

        [Required(ErrorMessage ="Đơn giá bán không được để trống")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Đơn giá bán phải lớn hơn 0 không được chứa ký tự đặc biệt")]
        public double DonGiaBan { get; set; }

        [Required(ErrorMessage ="Ngày nhập không được để trống")]
        public DateTime NgayNhap { get; set; }

        [Required(ErrorMessage ="Danh mục hàng không được để trống")]
        [StringLength(40)]
        public string? DanhMucHang { get; set; }
        [StringLength(50)]
        public string? HinhAnh { get; set; }

        [Required(ErrorMessage ="Kích cỡ không được để trống")]
        [StringLength(3)]
        public string? KichCo {  get; set; }
        [Required(ErrorMessage ="Mô tả không được để trống")]
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
