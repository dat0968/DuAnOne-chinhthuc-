using System.ComponentModel.DataAnnotations;

namespace Du_An_One.Models
{
    public class KHUYENMAI
    {
        [Key]
        [StringLength(5)]
        [Required(ErrorMessage ="Mã khuyến mãi không được để trống")]
        [MinLength(5, ErrorMessage ="Mã khuyến mãi phải có chính xác 5 kí tự")]
        public string? MaKhuyenMai { get; set; }

        [Required(ErrorMessage ="Phần trăm khuyến mãi không được để trống")]
        [Range(0, 100, ErrorMessage = "Phần trăm khuyến mãi phải nằm trong khoảng từ 0 đến 100")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Giá trị được điền không hợp lệ")]
        public int PhanTramKhuyenMai { get; set; }

        [Required(ErrorMessage ="Thời gian bắt đầu không được để trống")]
        public DateTime ThoiGianStart { get; set; }

        [Required(ErrorMessage ="Thời gian kết thúc không được để trống")]
        public DateTime ThoiGianEnd { get; set; }
        public ICollection<SANPHAM>? SANPHAMs { get; set; }
    }
}
