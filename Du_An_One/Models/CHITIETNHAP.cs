using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Du_An_One.Models
{
    public class CHITIETNHAP
    {
        [Key]
        [StringLength(5)]
        public string? MaChiTietNhap { get; set; }

        [StringLength(5)]
        [ForeignKey("MaNhaCC")]
        public string? MaNhaCC { get; set; }
        public NHACUNGCAP? NHACUNGCAP { get; set; }

        [StringLength(5)]
        [ForeignKey("MaSP")]
        public string? MaSP { get; set; }
        public SANPHAM? SANPHAM { get; set; }
        [Required]
        public int SoLuongNhap { get; set; }

        [Required]
        public double DonGiaNhap { get; set; }


    }
}
