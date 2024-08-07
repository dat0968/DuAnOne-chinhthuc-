using System.ComponentModel.DataAnnotations;

namespace Du_An_One.Models
{
    public class NHANVIEN
    {
        [Key]
        [StringLength(5, ErrorMessage ="Mã nhân viên phải có đúng 5 kí tự")]
        [Required(ErrorMessage="Mã nhân viên không được để trống")]
        [MinLength(5, ErrorMessage ="Mã nhân viên phải có đsung 5 kí tự")]
        public string? MaNV { get; set; }

        [Required(ErrorMessage ="Họ tên nhân viên không được để trống")]
        [StringLength(40)]
        public string? HoTen { get; set; }
       
        public DateTime? NgaySinh { get; set; }

        [StringLength(200)]
        public string? NoiSinh { get; set; }

        [StringLength(200)]
        public string? DiaChi { get; set; }

        [Required(ErrorMessage ="CCCD không được để trống")]
        [StringLength(12)]
        public string? CCCD { get; set; }

        [Required(ErrorMessage ="SDT không được để trống")]
        [RegularExpression("^[0-9]{10,15}$", ErrorMessage = "Số điện thoại không hợp lệ!")]
        [StringLength(11)]
        public string? SDT { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Email phải có đuôi @gmail.com")]
        [StringLength(40)]
        public string? Email { get; set; }

        [Required(ErrorMessage ="Ngày vào làm không được để trống")]
        public DateTime NgayVaoLam { get; set; }

        [Required(ErrorMessage ="Lương không được để trống")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Lương không được chứa ký tự đặc biệt")]
        
        public int Luong { get; set; }

        [Required]
        [StringLength(25)]
        public string? VaiTro { get; set; }

        [StringLength(15, ErrorMessage ="Tên tài khoản tối đa chỉ 15 kí tự")]
        [MinLength(5, ErrorMessage = "Tên tài khoản tối thiểu phải có 5 kí tự")]
        public string? TenTaiKhoan { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(20, ErrorMessage = "Mật khẩu tối đa chỉ chứa 20 kí tự")]
        [MinLength(10, ErrorMessage = "Mật khẩu tối thiểu phải chứa 10 kí tự")]
        public string? MatKhau { get; set; }

        [StringLength(25)]
        public string TinhTrang { get; set; } = "Mở";
        public ICollection<SANPHAM>? SANPHAMs { get; set; }
        public ICollection<HOADON>? HOADONs { get; set; }
    }
}
