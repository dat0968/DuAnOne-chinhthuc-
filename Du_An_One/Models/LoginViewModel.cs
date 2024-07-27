using System.ComponentModel.DataAnnotations;

namespace Du_An_One.Models
{
    public class LoginViewModel
    {
        [Required]
        public string TenTaiKhoan { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }

    }
}
