using System.ComponentModel.DataAnnotations;

namespace Du_An_One.Models
{
    public class NHACUNGCAP
    {
        [Key]
        [StringLength(5, ErrorMessage ="Mã nhà cung cấp phải có đúng 5 kí tự")]
        [Required(ErrorMessage = "Mã nhà cung cấp không được để trống")]
        [MinLength(5, ErrorMessage = "Mã nhà cung cấp phải có đsung 5 kí tự")]
        public string? MaNhaCC { get; set; }

        [Required(ErrorMessage = "Tên nhà cung cấp không được để trống")]
        [StringLength(60)]
        public string? TenNhaCC { get; set; }

        [Required(ErrorMessage = "Địa chỉ nhà cung cấp không được để trống")]
        [StringLength(200)]
        public string? DiaChi { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Email phải có đuôi @gmail.com")]
        [StringLength(40)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "SDT không được để trống")]
        [RegularExpression("^[0-9]{10,15}$", ErrorMessage = "Số điện thoại không hợp lệ!")]
        [StringLength(11)]
        public string? SDT { get; set; }

        [Required(ErrorMessage ="Ngày thành lập không được để trống")]
        public DateTime NgayThanhLap { get; set; }

        [Required(ErrorMessage ="Đại diện không được để trống")]
        [StringLength(40)]
        public string? NguoiDaiDien { get; set; }

        [Required(ErrorMessage ="Thời gian cung cấp không được để trống")]
        public DateTime ThoiGianCungCap { get; set; }

        [StringLength(25)]
        public string? TinhTrang { get; set; }
        public ICollection<CHITIETNHAP>? CHITIETNHAPs { get; set; }
    }
}
