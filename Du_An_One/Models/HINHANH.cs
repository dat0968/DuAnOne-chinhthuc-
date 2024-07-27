using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Du_An_One.Models
{
    public class HINHANH
    {
        [Key]
        public int MaHinhAnh { get; set; }
        [Required]
        [StringLength(50)]
        public string? HinhAnh { get; set; }
        [StringLength(5)]
        [ForeignKey("MaSP")]
        public string? MaSP { get; set; }
        public SANPHAM? SANPHAM { get; set; }
    }
}
