using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Du_An_One.Data;
using Du_An_One.Models;
using X.PagedList;
using ClosedXML.Excel;

namespace Du_An_One.Controllers
{
    public class SANPHAMsController : Controller
    {
        private readonly Du_An_OneContext _context;

        public SANPHAMsController(Du_An_OneContext context)
        {
            _context = context;
        }

        #region//Danh mục sản phẩm
        // GET: SANPHAMs
        public async Task<IActionResult> Index(int? page, string brand = "", string quantitySold = "", string price = "", string maSanPham = "")
        {
            int pageSize = 16;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;

            IQueryable<SANPHAM> listProducts = _context.SANPHAM.AsNoTracking();

            if (!String.IsNullOrEmpty(brand))
            {
                listProducts = listProducts.Where(x => x.DanhMucHang == brand);
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

            if (!String.IsNullOrEmpty(maSanPham))
            {
                listProducts = listProducts.Where(x => x.MaSP.Contains(maSanPham));
            }
            var productsList = await listProducts.ToListAsync();
            PagedList<SANPHAM> lstPaged = new PagedList<SANPHAM>(productsList, pageNumber, pageSize);
            return View(lstPaged);
            //return _context.SANPHAM != null ? 
            //              View(await _context.SANPHAM.ToListAsync()) :
            //              Problem("Entity set 'Du_An_OneContext.SANPHAM'  is null.");
        }
        #endregion

        #region//Chi tiết sản phẩm
        // GET: SANPHAMs/Details/5
        public async Task<IActionResult> Details(string MaSP)
        {
            var sanPham = _context.SANPHAM.SingleOrDefault(x => x.MaSP == MaSP);
            if (sanPham.HinhAnh != null)
            {
                sanPham.HinhAnh = "~/img/productImage/" + sanPham.HinhAnh;
            }
            else
            {
                sanPham.HinhAnh = "~/img/productImage/default.jpg";
            }
            ViewBag.anhSanPham = _context.HINHANH.Where(x => x.MaSP == MaSP).Select(x => x.HinhAnh).ToList();
            return View(sanPham);
        }
        #endregion
        #region//Hiển thị thumbnail chi tiết sản phẩm
        public IActionResult DetailsViewThubnail(string MaSP)
        {
            var sanPham = _context.SANPHAM.SingleOrDefault(x => x.MaSP == MaSP);
            if (sanPham.HinhAnh != null)
            {
                sanPham.HinhAnh = "~/img/productImage/" + sanPham.HinhAnh;
            }
            else
            {
                sanPham.HinhAnh = "~/img/productImage/default.jpg";
            }
            ViewBag.anhSanPham = _context.HINHANH.Where(x => x.MaSP == MaSP).Select(x => x.HinhAnh).ToList();
            return PartialView(sanPham);
        }
        #endregion
        #region//Tạo sản phẩm
        // GET: SANPHAMs/Create
        public IActionResult Create()
        {
            var danhMucHang = new List<string> { "TUFT", "Adidas", "Adius", "Surius" };
            ViewBag.DanhMucHang = new SelectList(danhMucHang);

            var khuyenMaiList = _context.KHUYENMAI.ToList();
            ViewBag.MaKhuyenMai = new SelectList(khuyenMaiList, "MaKhuyenMai", "PhanTramKhuyenMai");

            var nhanVienList = _context.NHANVIEN.ToList();
            ViewBag.MaNv = new SelectList(nhanVienList, "MaNV", "HoTen");
            return View();
        }

        // POST: SANPHAMs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SANPHAM sanPham)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (sanPham.FileImage != null)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/productImage/", sanPham.FileImage.FileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await sanPham.FileImage.CopyToAsync(stream);
                        }

                        sanPham.HinhAnh = sanPham.FileImage.FileName;
                    }

                    _context.SANPHAM.Add(sanPham);
                    _context.SaveChanges();

                    if (sanPham.FileImages != null && sanPham.FileImages.Count > 0)
                    {
                        foreach (var file in sanPham.FileImages)
                        {
                            if (file.Length > 0)
                            {
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/productImage/", file.FileName);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }

                                var hinhAnh = new HINHANH
                                {
                                    HinhAnh = file.FileName,
                                    MaSP = sanPham.MaSP
                                };

                                _context.HINHANH.Add(hinhAnh);
                            }
                        }

                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                // Log the ModelState errors
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine($"ModelState Error: {error.ErrorMessage}");
                    }
                }
            }

            // Re-initialize SelectLists if ModelState is invalid
            var danhMucHang = new List<string> { "TUFT", "Adidas", "Adius", "Surius" };
            ViewBag.DanhMucHang = new SelectList(danhMucHang);

            var khuyenMaiList = _context.KHUYENMAI.ToList();
            ViewBag.MaKhuyenMai = new SelectList(khuyenMaiList, "MaKhuyenMai", "PhanTramKhuyenMai");

            var nhanVienList = _context.NHANVIEN.ToList();
            ViewBag.MaNv = new SelectList(nhanVienList, "MaNV", "HoTen");

            return View(sanPham);
        }
        #endregion

        #region//Sửa sản phẩm
        // GET: SANPHAMs/Edit/5
        public async Task<IActionResult> Edit(string MaSP)
        {
            if (string.IsNullOrEmpty(MaSP))
            {
                return NotFound();
            }

            var sanPham = await _context.SANPHAM.FindAsync(MaSP);
            if (sanPham == null)
            {
                return NotFound();
            }

            ViewBag.DanhMucHang = new SelectList(new string[] { "TUFT", "Adidas", "Adius", "Surius" });
            ViewBag.MaKhuyenMai = new SelectList((await _context.KHUYENMAI.ToListAsync()) ?? new List<KHUYENMAI>(), "MaKhuyenMai", "PhanTramKhuyenMai");
            ViewBag.MaNv = new SelectList(await _context.NHANVIEN.ToListAsync() ?? new List<NHANVIEN>(), "MaNV", "HoTen");

            ViewBag.PartImages = await _context.HINHANH.Where(x => x.MaSP == MaSP).Select(x => x.HinhAnh).ToListAsync();
            return View(sanPham);
        }


        // POST: SANPHAMs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SANPHAM sanPham)
        {
            if (sanPham == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (sanPham.FileImage != null && sanPham.FileImage.FileName != sanPham.HinhAnh)
                    {
                        // Đường dẫn thư mục bạn muốn lưu file
                        var path = Path.Combine(
                            Directory.GetCurrentDirectory(), "wwwroot/img/productImage/", sanPham.FileImage.FileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await sanPham.FileImage.CopyToAsync(stream);
                        }

                        // Cập nhật đường dẫn file ảnh vào thuộc tính HinhAnh
                        sanPham.HinhAnh = sanPham.FileImage.FileName;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                _context.Entry(sanPham).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Log the ModelState errors
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine($"ModelState Error: {error.ErrorMessage}");
                    }
                }
            }

            // Nạp lại ViewBag nếu ModelState không hợp lệ
            ViewBag.DanhMucHang = new SelectList(new string[] { "TUFT", "Adidas", "Adius", "Surius" });
            ViewBag.MaKhuyenMai = new SelectList(await _context.KHUYENMAI.ToListAsync(), "MaKhuyenMai", "PhanTramKhuyenMai");
            ViewBag.MaNv = new SelectList(await _context.NHANVIEN.ToListAsync(), "MaNv", "HoTen");

            ViewBag.PartImages = await _context.HINHANH.Where(x => x.MaSP == sanPham.MaSP).Select(x => x.HinhAnh).ToListAsync();

            return View(sanPham);
        }
        #endregion

        #region//Xóa sản phẩm
        // GET: SANPHAMs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.SANPHAM == null)
            {
                return NotFound();
            }

            var sANPHAM = await _context.SANPHAM
                .FirstOrDefaultAsync(m => m.MaSP == id);
            if (sANPHAM == null)
            {
                return NotFound();
            }

            return View(sANPHAM);
        }

        // POST: SANPHAMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.SANPHAM == null)
            {
                return Problem("Entity set 'Du_An_OneContext.SANPHAM'  is null.");
            }
            var sANPHAM = await _context.SANPHAM.FindAsync(id);
            if (sANPHAM != null)
            {
                _context.SANPHAM.Remove(sANPHAM);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region//Xuất danh sách sản phẩm
        public IActionResult ExportProductsToExcel()
        {
            var products = _context.SANPHAM.ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DanhSachSanPham");
                var currentRow = 1;

                // Header
                worksheet.Cell(currentRow, 1).Value = "Mã sản phầm";
                worksheet.Cell(currentRow, 2).Value = "Tên sản phẩm";
                worksheet.Cell(currentRow, 3).Value = "Số lượng bán";
                worksheet.Cell(currentRow, 4).Value = "Đơn giá bán";
                worksheet.Cell(currentRow, 5).Value = "Ngày nhập";
                worksheet.Cell(currentRow, 8).Value = "Hình ảnh";
                worksheet.Cell(currentRow, 6).Value = "Hãng";
                worksheet.Cell(currentRow, 7).Value = "Kích cỡ";
                worksheet.Cell(currentRow, 9).Value = "Mô tả";
                worksheet.Cell(currentRow, 10).Value = "Mã khuyến mãi";
                worksheet.Cell(currentRow, 11).Value = "Mã nhân viên";

                // Content
                foreach (var product in products)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = product.MaSP;
                    worksheet.Cell(currentRow, 2).Value = product.TenSP;
                    worksheet.Cell(currentRow, 3).Value = product.SoLuongBan;
                    worksheet.Cell(currentRow, 4).Value = product.DonGiaBan;
                    worksheet.Cell(currentRow, 5).Value = product.NgayNhap;
                    worksheet.Cell(currentRow, 6).Value = product.DanhMucHang;
                    worksheet.Cell(currentRow, 7).Value = product.KichCo;
                    worksheet.Cell(currentRow, 8).Value = product.HinhAnh;
                    worksheet.Cell(currentRow, 9).Value = product.MoTa;
                    worksheet.Cell(currentRow, 10).Value = product.MaKhuyenMai;
                    worksheet.Cell(currentRow, 11).Value = product.MaNV;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachSanPham.xlsx");
                }
            }
        }

        #endregion

        private bool SANPHAMExists(string id)
        {
          return (_context.SANPHAM?.Any(e => e.MaSP == id)).GetValueOrDefault();
        }
    }
}
