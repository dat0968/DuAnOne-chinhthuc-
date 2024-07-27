using Du_An_One.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> ToolFindProduct(List<string> firms, string? quantitySold, string? price, string? searchText)
        {
            var listProducts = _context.SANPHAM.AsQueryable();


            if (firms != null && firms.Count > 0)
            {
                listProducts = listProducts.Where(x => firms.Contains(x.DanhMucHang));
            }

            if (!String.IsNullOrEmpty(quantitySold))
            {
                switch (quantitySold)
                {
                    case "1-50":
                        listProducts = listProducts.Where(x => x.SoLuongBan >= 1 && x.SoLuongBan <= 50);
                        break;
                    case "51-100":
                        listProducts = listProducts.Where(x => x.SoLuongBan >= 51 && x.SoLuongBan <= 100);
                        break;
                    case "101-200":
                        listProducts = listProducts.Where(x => x.SoLuongBan >= 101 && x.SoLuongBan <= 200);
                        break;
                    case "201+":
                        listProducts = listProducts.Where(x => x.SoLuongBan > 200);
                        break;
                }
            }

            if (!String.IsNullOrEmpty(price))
            {
                switch (price)
                {
                    case "0-100000":
                        listProducts = listProducts.Where(x => x.DonGiaBan >= 0 && x.DonGiaBan <= 100000);
                        break;
                    case "100001-200000":
                        listProducts = listProducts.Where(x => x.DonGiaBan >= 100001 && x.DonGiaBan <= 200000);
                        break;
                    case "200001-500000":
                        listProducts = listProducts.Where(x => x.DonGiaBan >= 200001 && x.DonGiaBan <= 500000);
                        break;
                    case "500001+":
                        listProducts = listProducts.Where(x => x.DonGiaBan > 500001);
                        break;
                }
            }
            if (!string.IsNullOrEmpty(searchText))
            {
                listProducts = listProducts.Where(x => (x.TenSP??"").Contains(searchText));
            }
            var listProductsByFirm = listProducts.ToList();
            return View(listProductsByFirm);
        }

    }
}
