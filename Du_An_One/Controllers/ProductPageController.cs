using Du_An_One.Data;
using Microsoft.AspNetCore.Mvc;
using SixLabors.Fonts;

namespace Du_An_One.Controllers
{
    public class ProductPageController : Controller
    {
        private readonly Du_An_OneContext _context;

        public ProductPageController(Du_An_OneContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            DateTime today = DateTime.Now;
            var listDiscount = _context.KHUYENMAI
                .Where(km => km.ThoiGianEnd >= today && km.ThoiGianStart <= today)
                .Select(km => new { km.MaKhuyenMai, km.PhanTramKhuyenMai })
                .ToList();


            ViewBag.ListNewProducts = _context.SANPHAM
                                                    .AsEnumerable()
                                                    .OrderByDescending(x => x.NgayNhap)
                                                    .Select(ldp => new
                                                    {
                                                        ldp,
                                                        PhanTramKhuyenMai = listDiscount.FirstOrDefault(ld => ld.MaKhuyenMai == ldp.MaKhuyenMai)?.PhanTramKhuyenMai ?? 0
                                                    })
                                                    .Take(16)
                                                    .ToList();
            ViewBag.ListDiscountProducts = _context.SANPHAM
                                                    .AsEnumerable()
                                                    .Where(sp => listDiscount.Select(ld => ld.MaKhuyenMai).Contains(sp.MaKhuyenMai))
                                                    .Take(6)    
                                                    .Select(ldp => new
                                                    {
                                                        ldp,
                                                        listDiscount.First(ld => ld.MaKhuyenMai == ldp.MaKhuyenMai).PhanTramKhuyenMai
                                                    });
            return View();
        }
        public async Task<IActionResult> ShowDetailProduct(string idProduct)
        {
            DateTime today = DateTime.Now;
            var listDiscount = _context.KHUYENMAI
                .Where(km => km.ThoiGianEnd >= today && km.ThoiGianStart <= today)
                .Select(km => new { km.MaKhuyenMai, km.PhanTramKhuyenMai })
                .ToList();

            var sanPham = _context.SANPHAM.SingleOrDefault(x => x.MaSP == idProduct);
            ViewBag.anhSanPham = (_context.HINHANH.Where(x => x.MaSP == idProduct).Select(x => x.HinhAnh.Length < 10 ? "~/img/productImage/default.jpg" : "~/img/productImage/" + x.HinhAnh));
            sanPham.HinhAnh = sanPham.HinhAnh == null || sanPham.HinhAnh.Length < 10 ? "~/img/productImage/default.jpg" : "~/img/productImage/" + sanPham.HinhAnh;


            ViewBag.ListRelateProducts = _context.SANPHAM
                                                    .AsEnumerable()
                                                    .Where(sp => sp.DanhMucHang == sanPham.DanhMucHang)
                                                    .Take(6)
                                                    .Select(ldp => new
                                                    {
                                                        ldp,
                                                        PhanTramKhuyenMai = listDiscount.FirstOrDefault(ld => ld.MaKhuyenMai == ldp.MaKhuyenMai)?.PhanTramKhuyenMai ?? 0
                                                    }); 
            return View(sanPham);
        }
    }
}
