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
            page = page < 1 ? 1 : page;
            int pagesize = 10;
            var chitiethoadons = _context.CHITIETHOADON
        .Include(c => c.SANPHAM)
        .ThenInclude(s => s.KHUYENMAI)
        .Include(c => c.HOADON)
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
        private bool CHITIETHOADONExists(int id)
        {
          return (_context.CHITIETHOADON?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
