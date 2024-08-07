using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Du_An_One.Models
{
    public class CHITIETHOADON
    {
        [Key]
        public int ID { get; set; }
        [StringLength(5)]
        [ForeignKey("MaHoaDon")]
        public string? MaHoaDon { get; set; }

        [StringLength(5)]
        [ForeignKey("MaSP")]
        public string? MaSP { get; set; }

        [Required(ErrorMessage = "Số lượng mua không được để trống")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Số lượng mua phải lớn hơn 0 và không được chứa ký tự đặc biệt ")]
        public int SoLuongMua { get; set; }

      
        public double DonGia { get; set; }

        public HOADON? HOADON { get; set; }
        public SANPHAM? SANPHAM { get; set; }
    }
}
