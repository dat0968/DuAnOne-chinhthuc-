using Du_An_One.Data;
using Du_An_One.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using System.Diagnostics;

namespace Du_An_One.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Du_An_OneContext _context;

        public HomeController(ILogger<HomeController> logger, Du_An_OneContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            DateTime today = DateTime.Now;
            return View(_context.SANPHAM
                .OrderByDescending(x => x.NgayNhap)
                .Take(6)
                .Select(sp => new SANPHAM
                {
                    MaSP = sp.MaSP,
                    SoLuongBan = sp.SoLuongBan,
                    DonGiaBan = sp.DonGiaBan,
                    DanhMucHang = sp.DanhMucHang,
                    HinhAnh = sp.HinhAnh,
                    KichCo = sp.KichCo,
                    MoTa = sp.MoTa,
                    MaKhuyenMai = sp.MaKhuyenMai,
                    KHUYENMAI = _context.KHUYENMAI
                                    .Where(km => km.MaKhuyenMai == sp.MaKhuyenMai)
                                    .Select(km => new KHUYENMAI
                                    {
                                        MaKhuyenMai = km.MaKhuyenMai,
                                        PhanTramKhuyenMai = (km.ThoiGianStart <= today && km.ThoiGianEnd >= today) ? km.PhanTramKhuyenMai : 0
                                    })
                                    .FirstOrDefault()
                })
                .ToList());
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("Error/404")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
