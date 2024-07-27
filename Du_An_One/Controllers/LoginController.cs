using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Du_An_One.Data;
using Du_An_One.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using System.Net;

namespace Du_An_One.Controllers
{
    public class LoginController : Controller
    {
        private readonly Du_An_OneContext _db;

        public LoginController(Du_An_OneContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Login");

            }
            return View();

        }
        [HttpPost]
        public async Task< IActionResult> Index(LoginViewModel model)
        {
            List<Claim> claims = new List<Claim>();
            ClaimsIdentity claimsIdentity;
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
            };
            var qr_nhanvien = _db.NHANVIEN.FirstOrDefault(s => s.TenTaiKhoan == model.TenTaiKhoan && s.MatKhau == model.MatKhau);
            var qr_khachhang = _db.KHACHHANG.FirstOrDefault(s => s.TenTaiKhoan == model.TenTaiKhoan && s.MatKhau == model.MatKhau);
            //if (qr_nhanvien.VaiTro == "Nhân viên" && (qr_nhanvien.TinhTrang != "Khóa"))
            //{
            //    claims.Add(new Claim(ClaimTypes.NameIdentifier, model.TenTaiKhoan));
            //    claims.Add(new Claim(ClaimTypes.Role, "Nhân viên"));
            //    claims.Add(new Claim(ClaimTypes.UserData, model.TenTaiKhoan));
            //    claims.Add(new Claim(ClaimTypes.Name, qr_nhanvien.HoTen));
            //    claims.Add(new Claim(ClaimTypes.Surname, qr_nhanvien.MaNV));

            //    claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
            //    return RedirectToAction("Index", "Home");
            //}
            if (qr_nhanvien != null)
            {
                if (qr_nhanvien.VaiTro == "Nhân viên" && (qr_nhanvien.TinhTrang != "Khóa"))
                {
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, model.TenTaiKhoan));
                    claims.Add(new Claim(ClaimTypes.Role, "Nhân viên"));
                    claims.Add(new Claim(ClaimTypes.UserData, model.TenTaiKhoan));
                    claims.Add(new Claim(ClaimTypes.Name, qr_nhanvien.HoTen));
                    claims.Add(new Claim(ClaimTypes.Surname, qr_nhanvien.MaNV));

                    claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
                    return RedirectToAction("Index", "Home");
                }
                else if (qr_nhanvien.VaiTro == "Quản lý" && (qr_nhanvien.TinhTrang != "Khóa"))
                {
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, model.TenTaiKhoan));
                    claims.Add(new Claim(ClaimTypes.Role, "Quản lý"));
                    claims.Add(new Claim(ClaimTypes.UserData, model.TenTaiKhoan));
                    claims.Add(new Claim(ClaimTypes.Name, qr_nhanvien.HoTen));
                    claims.Add(new Claim(ClaimTypes.Surname, qr_nhanvien.MaNV));


                    claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
                    return RedirectToAction("TongQuan", "Admin");
                }

            }
            else if (qr_khachhang != null && (qr_khachhang.TinhTrang != "Khóa"))
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, model.TenTaiKhoan));
                claims.Add(new Claim(ClaimTypes.Role, "Khách hàng"));
                claims.Add(new Claim(ClaimTypes.UserData, model.TenTaiKhoan));
                claims.Add(new Claim(ClaimTypes.Name, qr_khachhang.HoTen));
                claims.Add(new Claim(ClaimTypes.Surname, qr_khachhang.MaKH));


                claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
                return RedirectToAction("Index", "Home");
            }

            TempData["SwalIcon"] = "error";
            TempData["SwalTitle"] = "Đăng nhập không thành công";



            return View();
            //if (model.TenTaiKhoan!=null&& model.MatKhau!=null)
            //{
            //    var user = _db.KHACHHANG
            //        .Where(u => u.TenTaiKhoan == model.TenTaiKhoan && u.MatKhau == model.MatKhau)
            //        .Select(u => new
            //        {
            //            u.TenTaiKhoan,
            //            u.MatKhau
            //        })
            //        .SingleOrDefault();

            //    var nhanVien = _db.NHANVIEN
            //        .Where(u => u.TenTaiKhoan == model.TenTaiKhoan && u.MatKhau == model.MatKhau)
            //        .Select(u => new
            //        {
            //            u.TenTaiKhoan,
            //            u.MatKhau,
            //            u.VaiTro
            //        })
            //        .SingleOrDefault();

            //    if (nhanVien != null)
            //    {
            //        // Tạo cookie chứa thông tin đăng nhập
            //        CookieOptions options = new CookieOptions();
            //        options.Expires = DateTime.Now.AddMinutes(30);

            //        HttpContext.Response.Cookies.Append("TenTaiKhoan", nhanVien.TenTaiKhoan, options);
            //        HttpContext.Response.Cookies.Append("MatKhau", nhanVien.MatKhau, options);
            //        HttpContext.Response.Cookies.Append("VaiTro", nhanVien.VaiTro, options);

            //        // Chuyển hướng dựa trên vai trò
            //        if (nhanVien.VaiTro == "Nhân viên")
            //        {
            //            return RedirectToAction("TongQuan", "Admin");
            //        }
            //        else
            //        {
            //            return RedirectToAction("Index", "Home");
            //        }
            //    }

            //    if (user != null)
            //    {
            //        // Tạo cookie chứa thông tin đăng nhập
            //        CookieOptions options = new CookieOptions();
            //        options.Expires = DateTime.Now.AddMinutes(30);

            //        HttpContext.Response.Cookies.Append("TenTaiKhoan", user.TenTaiKhoan, options);
            //        HttpContext.Response.Cookies.Append("MatKhau", user.MatKhau, options);

            //        // Chuyển hướng đến trang khách hàng
            //        return RedirectToAction("Index", "Home");
            //    }
            //    else
            //    {
            //        // Thất bại
            //        ModelState.AddModelError("", "Tên tài khoản hoặc mật khẩu không đúng.");
            //    }
            //}
            //return View(model);
        }


        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(KHACHHANG model)
        {
            Random random = new Random();
            int randomValue = random.Next(1000);
            string maKH = "KH" + randomValue.ToString("D3");
            if (model.HoTen != null && model.SDT != null && model.Email != null && model.TenTaiKhoan != null && model.MatKhau != null)
            {
                var newUser = new KHACHHANG
                {
                    MaKH = maKH,
                    HoTen = model.HoTen,
                    NgaySinh = null,
                    NoiSinh = null,
                    DiaChi = null,
                    CCCD = model.CCCD,
                    SDT = model.SDT,
                    Email = model.Email,
                    TenTaiKhoan = model.TenTaiKhoan,
                    MatKhau = model.MatKhau,
                    TinhTrang = "Mở"
                };

                _db.KHACHHANG.Add(newUser);
                _db.SaveChanges();

                return RedirectToAction("Index", "Login");
            }
            else
            {
                TempData["rePass"] = "Vui lòng nhập lại đúng mật khẩu";
                return View(model);
            }
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GuiMail(string Email)
        {
            var khachHang = _db.KHACHHANG.FirstOrDefault(kh => kh.Email == Email);
            var nhanvien = _db.NHANVIEN.FirstOrDefault(nv => nv.Email == Email);

            if (khachHang != null)
            {
                string MaXacNhan;
                Random rnd = new Random();
                MaXacNhan = rnd.Next(10000, 100000).ToString();

                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.EnableSsl = true;
                smtp.Port = 587;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential("sthe06714@gmail.com", "ezos qjgi xwet ulvt");

                MailMessage mail = new MailMessage();
                mail.To.Add(Email);
                mail.From = new MailAddress("sthe06714@gmail.com");
                mail.Subject = "Thông Báo Từ  ";

                string logoUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e5/NASA_logo.svg/1224px-NASA_logo.svg.png";

                mail.Body = "Kính gửi,<br>" +
                            "Chúng tôi xác nhận bạn đã sử dụng quên mật khẩu của chúng tôi<br>" +
                            "<strong><h2>Đây là mã xác nhận của bạn: " + MaXacNhan + "</h2></strong><br>" +
                            "Xin vui lòng không cung cấp cho người khác<br>" +
                            "Trân trọng.<br>" +
                            "Đội ngũ hỗ trợ zzz" + "<br><br>" +
                            "<img src='" + logoUrl + "' alt='Logo' />";
                mail.IsBodyHtml = true;
                await smtp.SendMailAsync(mail);
                return Json(new { success = true, confirmationCode = MaXacNhan, responseText = "Email đã được gửi thành công!" });
            }
            if (nhanvien != null)
            {
                string MaXacNhan;
                Random rnd = new Random();
                MaXacNhan = rnd.Next().ToString();

                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.EnableSsl = true;
                smtp.Port = 587;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential("sthe06714@gmail.com", "ezos qjgi xwet ulvt");

                MailMessage mail = new MailMessage();
                mail.To.Add(Email);
                mail.From = new MailAddress("sthe06714@gmail.com");
                mail.Subject = "Thông Báo Từ  ";

                string logoUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/LEGO_logo.svg/2048px-LEGO_logo.svg.png";

                mail.Body = "Kính gửi,<br>" +
                            "Chúng tôi xác nhận bạn đã sử dụng quên mật khẩu của chúng tôi<br>" +
                            "<strong><h2>Đây là mã xác nhận của bạn: " + MaXacNhan + "</h2></strong><br>" +
                            "Xin vui lòng không cung cấp cho người khác<br>" +
                            "Trân trọng.<br>" +
                            "Đội ngũ hỗ trợ zzz" + "<br><br>" +
                            "<img src='" + logoUrl + "' alt='Logo' />";
                mail.IsBodyHtml = true;
                await smtp.SendMailAsync(mail);
                return Json(new { success = true, confirmationCode = MaXacNhan, responseText = "Email đã được gửi thành công!" });
            }

            return Ok();
        }
        [HttpPost]
        public IActionResult QuenMatKhau(string Email, string NewPassword)
        {
            var khachHang = _db.KHACHHANG.FirstOrDefault(s => s.Email == Email);
            var nhanvien = _db.NHANVIEN.FirstOrDefault(s => s.Email == Email);
            if (khachHang != null)
            {
                khachHang.MatKhau = NewPassword;
                _db.KHACHHANG.Update(khachHang);
            }
            if (nhanvien != null)
            {
                nhanvien.MatKhau = NewPassword;
                _db.NHANVIEN.Update(nhanvien);
            }

            _db.SaveChanges();
            return Ok();
        }
    }
}
