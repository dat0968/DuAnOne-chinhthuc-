using Du_An_One.Data;
using Du_An_One.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.Fonts;
using X.PagedList;

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
            var twoListProduts = new List<List<SANPHAM>>
            {
                _context.SANPHAM
                    .AsEnumerable()
                    .OrderByDescending(x => x.NgayNhap)
                    .Select(sp => new SANPHAM
                    {
                        MaSP = sp.MaSP,
                        TenSP = sp.TenSP,
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
                    .Take(16)
                    .ToList(),
                _context.SANPHAM
                    .AsEnumerable()
                    .Where(sp => listDiscount.Select(ld => ld.MaKhuyenMai).Contains(sp.MaKhuyenMai))
                    .Take(6)
                    .Select(sp => new SANPHAM
                    {
                        MaSP = sp.MaSP,
                        TenSP = sp.TenSP,
                        SoLuongBan = sp.SoLuongBan,
                        DonGiaBan = sp.DonGiaBan,
                        DanhMucHang = sp.DanhMucHang,
                        HinhAnh = sp.HinhAnh,
                        KichCo = sp.KichCo,
                        MoTa = sp.MoTa,
                        MaKhuyenMai = sp.MaKhuyenMai,
                        KHUYENMAI = _context.KHUYENMAI.FirstOrDefault(km => km.MaKhuyenMai == sp.MaKhuyenMai)
                    })
                    .ToList()
            };
            return View(twoListProduts);
        }
        public async Task<IActionResult> ShowDetailProduct(string idProduct)
        {
            DateTime today = DateTime.Now;
            var listDiscount = _context.KHUYENMAI
                .Where(km => km.ThoiGianEnd >= today && km.ThoiGianStart <= today)
                .Select(km => new { km.MaKhuyenMai, km.PhanTramKhuyenMai })
                .ToList();

            var sanPham = _context.SANPHAM
                .Select(sp => new SANPHAM
                {
                    MaSP = sp.MaSP,
                    TenSP = sp.TenSP,
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
                .SingleOrDefault(x => x.MaSP == idProduct);
            ViewBag.anhSanPham = (_context.HINHANH.Where(x => x.MaSP == idProduct).Select(x => x.HinhAnh.Length < 10 ? "~/img/productImage/default.jpg" : "~/img/productImage/" + x.HinhAnh));
            
            var listRelateProducts = _context.SANPHAM
                                                    .AsEnumerable()
                                                    .Where(sp => sp.DanhMucHang == sanPham.DanhMucHang)
                                                    .Take(6)
                                                    .Select(ldp => new SANPHAM
                                                    {
                                                        MaSP = ldp.MaSP,
                                                        TenSP = ldp.TenSP,
                                                        SoLuongBan = ldp.SoLuongBan,
                                                        DonGiaBan = ldp.DonGiaBan,
                                                        DanhMucHang = ldp.DanhMucHang,
                                                        HinhAnh = ldp.HinhAnh,
                                                        KichCo = ldp.KichCo,
                                                        MoTa = ldp.MoTa,
                                                        KHUYENMAI = _context.KHUYENMAI
                                                            .Where(km => km.MaKhuyenMai == ldp.MaKhuyenMai)
                                                            .Select(km => new KHUYENMAI
                                                            {
                                                                MaKhuyenMai = km.MaKhuyenMai,
                                                                PhanTramKhuyenMai = (km.ThoiGianStart <= today && km.ThoiGianEnd >= today) ? km.PhanTramKhuyenMai : 0
                                                            })
                                                            .FirstOrDefault()
                                                    });

            return View((sanPham, listRelateProducts ));
        }
        public async Task<IActionResult> ToolFindProduct(int? page, List<string> firms, string? quantitySold, string? price, string? searchText)
        {
            int pageSize = 16;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;

            DateTime today = DateTime.Now;
            var listDiscount = _context.KHUYENMAI
                .AsEnumerable()
                .Where(km => km.ThoiGianEnd >= today && km.ThoiGianStart <= today)
                .Select(km => new { km.MaKhuyenMai, km.PhanTramKhuyenMai })
                .ToList();
            IQueryable<SANPHAM> listProducts = _context.SANPHAM
                .Select(sp => new SANPHAM
                {
                    MaSP = sp.MaSP,
                    TenSP = sp.TenSP,
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
                .AsQueryable();

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
                listProducts = listProducts.Where(x => (x.TenSP ?? "").Contains(searchText));
            }

            var productsList = await listProducts.ToListAsync();
            PagedList<SANPHAM> lstPaged = new PagedList<SANPHAM>(productsList, pageNumber, pageSize);
            return View(lstPaged);
        }
        [HttpPost]
        public async Task<IActionResult> ToolFindProductAJAX(List<string> firms, string? quantitySold, string? price, string? searchText)
        {
            Console.WriteLine($"Firms: {string.Join(", ", firms)}, QuantitySold: {quantitySold}, Price: {price}, SearchText: {searchText}");

            DateTime today = DateTime.Now;
            var listDiscount = _context.KHUYENMAI
                .Where(km => km.ThoiGianEnd >= today && km.ThoiGianStart <= today)
                .Select(km => new { km.MaKhuyenMai, km.PhanTramKhuyenMai })
                .ToList();
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
                listProducts = listProducts.Where(x => (x.TenSP ?? "").Contains(searchText));
            }
            var listProductsByFirm = listProducts.ToList();

            var result = listProductsByFirm
                                    .AsEnumerable()
                                    .OrderByDescending(x => x.NgayNhap)
                                    .Select(ldp => new
                                    {
                                        ldp,
                                        PhanTramKhuyenMai = listDiscount.FirstOrDefault(ld => ld.MaKhuyenMai == ldp.MaKhuyenMai)?.PhanTramKhuyenMai ?? 0
                                    })
                                    .ToList();

            return PartialView("_ProductListPartial", result);
        }




    }
}
