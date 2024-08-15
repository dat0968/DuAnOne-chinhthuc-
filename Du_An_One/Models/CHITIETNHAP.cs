using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Du_An_One.Models
{
    public class CHITIETNHAP
    {
        [Key]
        [StringLength(5, ErrorMessage = "Mã chi tiết nhập phải có đúng 5 kí tự")]
        [Required(ErrorMessage = "Mã chi tiết nhập không được để trống")]
        [MinLength(5, ErrorMessage = "Mã chi tiết nhập phải có đsung 5 kí tự")]
        public string? MaChiTietNhap { get; set; }

        [StringLength(5)]
        [ForeignKey("MaNhaCC")]
        public string? MaNhaCC { get; set; }
        public NHACUNGCAP? NHACUNGCAP { get; set; }

        [StringLength(5)]
        [ForeignKey("MaSP")]
        public string? MaSP { get; set; }
        public SANPHAM? SANPHAM { get; set; }
        [Required(ErrorMessage = "Số lượng nhập không được để trống")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Số lượng nhập phải lớn hơn 0 và không được chứa ký tự đặc biệt ")]
        public int SoLuongNhap { get; set; }

        [Required(ErrorMessage = "Đơn giá nhập không được để trống")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Đơn giá nhập phải lớn hơn 0 không được chứa ký tự đặc biệt")]
        public double DonGiaNhap { get; set; }


    }
}
