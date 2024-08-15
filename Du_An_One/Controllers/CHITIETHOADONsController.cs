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
using X.PagedList.Extensions;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using System.Security.Claims;
using static System.Net.WebRequestMethods;
using DocumentFormat.OpenXml.Bibliography;

namespace Du_An_One.Controllers
{
    public class CHITIETHOADONsController : Controller
    {
        private readonly Du_An_OneContext _context;

        public CHITIETHOADONsController(Du_An_OneContext context)
        {
            _context = context;
        }

        // GET: CHITIETHOADONs
        public async Task<IActionResult> Index(int page = 1)
        {
            DateTime today = DateTime.Now;

            page = page < 1 ? 1 : page;
            int pagesize = 10;
            var chitiethoadons = _context.CHITIETHOADON
                    .AsEnumerable()
                    .Select(ct => new CHITIETHOADON
                    {
                        ID = ct.ID,
                        MaHoaDon = ct.MaHoaDon,
                        MaSP = ct.MaSP,
                        SoLuongMua = ct.SoLuongMua,
                        DonGia = ct.DonGia,
                        SANPHAM = _context.SANPHAM.Select(sp => new SANPHAM
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
                        }).FirstOrDefault(),
                        HOADON = _context.HOADON.FirstOrDefault(hd =>hd.MaHoaDon == ct.MaHoaDon)
                    })
        .ToPagedList(page, pagesize);

            return View(chitiethoadons);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string keywords, int page = 1)
        {
            if (keywords == null)
            {
                return RedirectToAction("Index", "CHITIETHOADONs");
            }
            page = page < 1 ? 1 : page;
            int pagesize = 10;
            var query = _context.CHITIETHOADON
        .Include(c => c.SANPHAM)
        .ThenInclude(s => s.KHUYENMAI)
        .Include(c => c.HOADON)
        .AsQueryable();
            query = query
            .Where(p => p.MaHoaDon.Contains(keywords) || p.HOADON.MaKH.Contains(keywords));
            var ketquatimkiem = query.ToPagedList(page, pagesize);

            if (!ketquatimkiem.Any())
            {
                
                ketquatimkiem = _context.CHITIETHOADON
        .Include(c => c.SANPHAM)
        .ThenInclude(s => s.KHUYENMAI)
        .Include(c => c.HOADON)
        .ToPagedList(page, pagesize);
                TempData["ErrorMessage"] = "Không tìm thấy hóa đơn với từ khóa đã nhập.";
            }

            return View(ketquatimkiem);
        }
        // GET: CHITIETHOADONs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CHITIETHOADON == null)
            {
                return NotFound();
            }

            var cHITIETHOADON = await _context.CHITIETHOADON.Include(c => c.SANPHAM)
        .ThenInclude(s => s.KHUYENMAI)
        .Include(c => c.HOADON)
       
        .FirstOrDefaultAsync(m => m.ID == id);

            if (cHITIETHOADON == null)
            {
                return NotFound();
            }

            return View(cHITIETHOADON);
        }

        // GET: CHITIETHOADONs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CHITIETHOADONs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,MaHoaDon,MaSP,SoLuongMua,DonGia")] CHITIETHOADON cHITIETHOADON)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cHITIETHOADON);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cHITIETHOADON);
        }

        // GET: CHITIETHOADONs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CHITIETHOADON == null)
            {
                return NotFound();
            }

            var cHITIETHOADON = await _context.CHITIETHOADON
        .Include(c => c.SANPHAM)
        .ThenInclude(s => s.KHUYENMAI)
        .Include(c => c.HOADON)
        .FirstOrDefaultAsync(m => m.ID == id);
            if (cHITIETHOADON == null)
            {
                return NotFound();
            }
            return View(cHITIETHOADON);
        }

        // POST: CHITIETHOADONs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,MaHoaDon,MaSP,SoLuongMua,DonGia, MaNV")] CHITIETHOADON cHITIETHOADON)
        {
            if (id != cHITIETHOADON.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cHITIETHOADON);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Sửa thông tin chi tiết hóa đơn thành công";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CHITIETHOADONExists(cHITIETHOADON.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaHoaDon"] = new SelectList(_context.HOADON, "MaHoaDon", cHITIETHOADON.MaHoaDon);
            ViewData["MaSP"] = new SelectList(_context.SANPHAM, "MaSP", cHITIETHOADON.MaSP);
         
            return View(cHITIETHOADON);
        }

        // GET: CHITIETHOADONs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CHITIETHOADON == null)
            {
                return NotFound();
            }

            var cHITIETHOADON = await _context.CHITIETHOADON.Include(c => c.SANPHAM)
                            .ThenInclude(s => s.KHUYENMAI)
                            .Include(c => c.HOADON)
                            .FirstOrDefaultAsync(m => m.ID == id);
            if (cHITIETHOADON == null)
            {
                return NotFound();
            }

            return View(cHITIETHOADON);
        }

        // POST: CHITIETHOADONs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CHITIETHOADON == null)
            {
                return Problem("Entity set 'Du_An_OneContext.CHITIETHOADON'  is null.");
            }
            var cHITIETHOADON = await _context.CHITIETHOADON.FindAsync(id);
            try
            {
                if (cHITIETHOADON != null)
                {
                    _context.CHITIETHOADON.Remove(cHITIETHOADON);
                }


                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa chi tiết hóa đơn thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa dữ liệu. Vui lòng thử lại.";
            }
            return View();
        }
        public IActionResult ExportExcelHoaDon()
        {

            var hoadonchitiet = _context.CHITIETHOADON
        .Include(c => c.SANPHAM)
        .ThenInclude(s => s.KHUYENMAI)
        .Include(c => c.HOADON)
        .ToList();
            using (var workbook = new XLWorkbook())
            {
                var hanghientai = 1;
                var worksheet = workbook.Worksheets.Add("DanhSachHoaDon");
                worksheet.Cell(hanghientai, 1).Value = "Mã hóa đơn";
                worksheet.Cell(hanghientai, 2).Value = "Mã sản phẩm";
                worksheet.Cell(hanghientai, 3).Value = "Số lượng mua";
                worksheet.Cell(hanghientai, 4).Value = "Đơn giá";
                worksheet.Cell(hanghientai, 5).Value = "Ngày tạo";
                worksheet.Cell(hanghientai, 6).Value = "Tổng tiền";
                worksheet.Cell(hanghientai, 7).Value = "Khách hàng";
                worksheet.Cell(hanghientai, 8).Value = "Thu ngân";
                worksheet.Cell(hanghientai, 9).Value = "HTTT";
                worksheet.Cell(hanghientai, 10).Value = "Địa chỉ nhân hàng";
                worksheet.Cell(hanghientai, 11).Value = "Tình trạng";
                foreach (var hoadon in hoadonchitiet)
                {
                    hanghientai++;
                    var donGia = (hoadon.SANPHAM?.DonGiaBan ?? 0) * (1 - (hoadon.SANPHAM?.KHUYENMAI?.PhanTramKhuyenMai ?? 0) / 100);
                    var tongTien = donGia * (hoadon.SoLuongMua);

                    worksheet.Cell(hanghientai, 1).Value = hoadon.MaHoaDon;
                    worksheet.Cell(hanghientai, 2).Value = hoadon.MaSP;
                    worksheet.Cell(hanghientai, 3).Value = hoadon.SoLuongMua;
                    worksheet.Cell(hanghientai, 4).Value = donGia;
                    worksheet.Cell(hanghientai, 5).Value = hoadon.HOADON.NgayTao.ToString("dd/MM/yyyy"); // Formatting the date
                    worksheet.Cell(hanghientai, 6).Value = tongTien;
                    worksheet.Cell(hanghientai, 7).Value = hoadon.HOADON.MaKH;
                    worksheet.Cell(hanghientai, 8).Value = hoadon.HOADON.MaNV;
                    worksheet.Cell(hanghientai, 9).Value = hoadon.HOADON.HTTT;
                    worksheet.Cell(hanghientai, 10).Value = hoadon.HOADON.DiaChiNhanHang;
                    worksheet.Cell(hanghientai, 11).Value = hoadon.HOADON.TinhTrang;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachHoaDon.xlsx");
                }
            }
        }


        [HttpPost]
        public async Task<IActionResult> CustomerDelete(int id)
        {
            if (_context.CHITIETHOADON == null)
            {
                return Json(new { success = false, message = "Entity set 'Du_An_OneContext.CHITIETHOADON' is null." });
            }

            var cHITIETHOADON = await _context.CHITIETHOADON.FindAsync(id);
            if (cHITIETHOADON != null)
            {
                _context.CHITIETHOADON.Remove(cHITIETHOADON);
                if (_context.CHITIETHOADON.Count(ct => ct.MaHoaDon == cHITIETHOADON.MaHoaDon) == 0)
                {
                    _context.HOADON.Remove(_context.HOADON.FirstOrDefault(hd => hd.MaHoaDon == cHITIETHOADON.MaHoaDon));
                }
                await _context.SaveChangesAsync();
                return Json(new
                {
                    success = true,
                    alert = new { type = "success", title = "Thông báo", message = "Đã xóa thành công" }
                });
            }

            return Json(new
            {
                success = false,
                alert = new { type = "info", title = "Thông báo", message = "Không tìm thấy sản phẩm" }
            });
        }


        public async Task<IActionResult> CustomerAdd(string MaSP)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Nếu người dùng chưa đăng nhập, trả về thông báo cảnh báo
                return Json(new
                {
                    success = false,
                    alert = new { type = "warning", title = "Thông báo", message = "Vui lòng đăng nhập để có thể mua hàng." }
                });
            }
            if (User.FindFirst(ClaimTypes.Role)?.Value != "Khách hàng")
            {
                // Nếu người dùng chưa đăng nhập, trả về thông báo cảnh báo
                return Json(new
                {
                    success = false,
                    alert = new { type = "info", title = "Thông báo", message = "Chức năng dành cho khách, vui lòng làm việc trong phận sự." }
                });
            }
            DateTime today = DateTime.Now;
            var MaNguoiDung = User.FindFirst(ClaimTypes.Surname)?.Value;
            var Product = _context.SANPHAM.FirstOrDefault(sp => sp.MaSP == MaSP);
            var khachHang = _context.KHACHHANG.FirstOrDefault(x => x.MaKH == MaNguoiDung);

            // Lấy danh sách khuyến mãi còn hiệu lực
            var DanhSachMaKhuyenMai = _context.KHUYENMAI
                .Where(km => km.ThoiGianStart <= today && km.ThoiGianEnd >= today)
                .ToList();

            // Kiểm tra xem giỏ hàng của khách hàng có tồn tại không
            var CustomerBag = _context.HOADON
                .FirstOrDefault(hd => hd.MaKH == MaNguoiDung && hd.TinhTrang == "Giỏ hàng");

            if (CustomerBag != null)
            {
                // Kiểm tra xem sản phẩm đã có trong chi tiết hóa đơn không
                var existingDetail = _context.CHITIETHOADON
                    .FirstOrDefault(cd => cd.MaHoaDon == CustomerBag.MaHoaDon && cd.MaSP == MaSP);

                if (existingDetail != null)
                {
                    // Nếu sản phẩm đã có, tăng số lượng lên 1
                    existingDetail.SoLuongMua++;
                }
                else
                {
                    // Nếu sản phẩm chưa có, thêm vào chi tiết hóa đơn
                    var phanTramKhuyenMai = DanhSachMaKhuyenMai
                        .FirstOrDefault(lkm => lkm.MaKhuyenMai == Product.MaKhuyenMai)?.PhanTramKhuyenMai ?? 0;

                    _context.CHITIETHOADON.Add(new CHITIETHOADON
                    {
                        MaHoaDon = CustomerBag.MaHoaDon,
                        DonGia = Product.DonGiaBan * (1 - phanTramKhuyenMai / 100),
                        MaSP = MaSP,
                        SoLuongMua = 1
                    });
                }
            }
            else
            {
                // Nếu chưa có giỏ hàng, tạo mới giỏ hàng và thêm sản phẩm vào
                Random rd = new Random();
                const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

                var _newCheck = new HOADON
                {
                    MaHoaDon = String.Join("", Enumerable.Range(0, 5).Select(_ => chars[rd.Next(chars.Length)])),
                    DiaChiNhanHang =  "",
                    NgayTao = today,
                    HTTT = "Tiền mặt",
                    MaKH = MaNguoiDung,
                    TinhTrang = "Giỏ hàng" // Cần thêm tình trạng cho hóa đơn mới
                };

                _context.HOADON.Add(_newCheck);
                await _context.SaveChangesAsync(); // Lưu hóa đơn mới vào cơ sở dữ liệu

                var phanTramKhuyenMai = DanhSachMaKhuyenMai
                    .FirstOrDefault(lkm => lkm.MaKhuyenMai == Product.MaKhuyenMai)?.PhanTramKhuyenMai ?? 0;

                _context.CHITIETHOADON.Add(new CHITIETHOADON
                {
                    MaHoaDon = _newCheck.MaHoaDon,
                    MaSP = MaSP, // Thêm dòng này
                    DonGia = Product.DonGiaBan * (1 - phanTramKhuyenMai / 100),
                    SoLuongMua = 1
                });
            }

            // Lưu tất cả thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // Trả về thông báo thành công
            return Json(new
            {
                success = true,
                alert = new { type = "success", title = "Thông báo", message = "Đã thêm sản phẩm vào giỏ hàng thành công." }
            });
        }

        private bool CHITIETHOADONExists(int id)
        {
            return (_context.CHITIETHOADON?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
