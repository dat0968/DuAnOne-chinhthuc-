using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Du_An_One.Models
{
    public class CheckoutViewModel
    {
        public string MaHoaDon { get; set; }
        public string MaKH { get; set; }

        [Required(ErrorMessage = "Địa chỉ nhận hàng là bắt buộc.")]
        public string DiaChiNhanHang { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn hình thức thanh toán.")]
        public string HTTT { get; set; }
    }
}
