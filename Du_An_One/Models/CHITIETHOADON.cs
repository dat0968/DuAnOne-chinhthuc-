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

        [Required]
        public int SoLuongMua { get; set; }

        [Required]
        public double DonGia { get; set; }

        public HOADON? HOADON { get; set; }
        public SANPHAM? SANPHAM { get; set; }
    }
}
