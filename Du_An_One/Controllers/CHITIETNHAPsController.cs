using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Du_An_One.Data;
using Du_An_One.Models;
using DocumentFormat.OpenXml.Office2010.Excel;
using ClosedXML.Excel;
using X.PagedList.Extensions;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;
namespace Du_An_One.Controllers
{
    [Authorize(Roles = "Quản lý")]
    public class CHITIETNHAPsController : Controller
    {
        private readonly Du_An_OneContext _context;

        public CHITIETNHAPsController(Du_An_OneContext context)
        {
            _context = context;
        }

        // GET: CHITIETNHAPs
        public async Task<IActionResult> Index(int page = 1)
        {
            page = page < 1 ? 1 : page;
            int pagesize = 10;
            var phieunhap = _context.CHITIETNHAP.ToPagedList(page, pagesize);
            return View(phieunhap);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string keywords, int page = 1)
        {
            if (keywords == null)
            {
                return RedirectToAction("Index", "CHITIETNHAPs");
            }
            page = page < 1 ? 1 : page;
            int pagesize = 10;
            var query = _context.CHITIETNHAP.AsQueryable();
            query = query
            .Where(p => p.MaNhaCC.Contains(keywords));
            var ketquatimkiem = query.ToPagedList(page, pagesize);

            if (!ketquatimkiem.Any())
            {
              
                ketquatimkiem = _context.CHITIETNHAP.ToPagedList(page, pagesize);
                TempData["ErrorMessage"] = "Không tìm thấy phiếu nhập với từ khóa đã nhập.";
            }

            return View(ketquatimkiem);
        }
        // GET: CHITIETNHAPs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.CHITIETNHAP == null)
            {
                return NotFound();
            }

            var cHITIETNHAP = await _context.CHITIETNHAP
                .FirstOrDefaultAsync(m => m.MaChiTietNhap == id);
            if (cHITIETNHAP == null)
            {
                return NotFound();
            }

            return View(cHITIETNHAP);
        }

        // GET: CHITIETNHAPs/Create
        public IActionResult Create()
        {
           
      
            return View();
        }

        // POST: CHITIETNHAPs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaChiTietNhap,MaNhaCC,MaSP,SoLuongNhap,DonGiaNhap")] CHITIETNHAP cHITIETNHAP)
        {
            if (ModelState.IsValid)
            {
                bool exists = await _context.CHITIETNHAP.AnyAsync(p => p.MaChiTietNhap == cHITIETNHAP.MaChiTietNhap);
                bool checkmanhacungcap = await _context.NHACUNGCAP.AnyAsync(p => p.MaNhaCC == cHITIETNHAP.MaNhaCC);
                bool checkmasp = await _context.SANPHAM.AnyAsync(p => p.MaSP == cHITIETNHAP.MaSP);
                if (exists)
                {
                    ModelState.AddModelError("MaNV", "Mã này đã tồn tại. Vui lòng nhập lại");
                    return View(cHITIETNHAP);
                }
                if (!checkmanhacungcap)
                {
                    ModelState.AddModelError("MaNhaCC", "Mã này không tồn tại. Vui lòng nhập lại");
                    return View(cHITIETNHAP);
                }
                if (!checkmasp)
                {
                    ModelState.AddModelError("MaSP", "Mã này không tồn tại. Vui lòng nhập lại");
                    return View(cHITIETNHAP);
                }
                try
                {
                    _context.Add(cHITIETNHAP);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Thêm mã chi tiết nhập thành công";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm phiếu nhập. Vui lòng thử lại.";
                }
            }
            
            return View(cHITIETNHAP);
        }

        // GET: CHITIETNHAPs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.CHITIETNHAP == null)
            {
                return NotFound();
            }

            var cHITIETNHAP = await _context.CHITIETNHAP.FindAsync(id);
            if (cHITIETNHAP == null)
            {
                return NotFound();
            }
            return View(cHITIETNHAP);
        }

        // POST: CHITIETNHAPs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaHoaDon, DiaChiNhanHang, NgayTao, HTTT, TinhTrang, MaNV, MaKH")] HOADON hOADON)
        {
            if (id != hOADON.MaHoaDon)
            {
                return NotFound();
            }

            // Kiểm tra sự tồn tại của nhân viên và khách hàng
            if (hOADON.MaNV != null && !await _context.NHANVIEN.AnyAsync(e => e.MaNV == hOADON.MaNV))
            {
                ModelState.AddModelError("MaNV", "Mã nhân viên không tồn tại.");
            }

            if (hOADON.MaKH != null && !await _context.KHACHHANG.AnyAsync(c => c.MaKH == hOADON.MaKH))
            {
                ModelState.AddModelError("MaKH", "Mã khách hàng không tồn tại.");
            }

            if (hOADON.TinhTrang!=null)
            {
                try
                {
                    var existingHoaDon = await _context.HOADON.FindAsync(id);
                    if (existingHoaDon == null)
                    {
                        return NotFound();
                    }

                    // Cập nhật thông tin hóa đơn, chỉ cập nhật thuộc tính TinhTrang
                    existingHoaDon.TinhTrang = hOADON.TinhTrang;

                    // Không sử dụng SetValues, chỉ cập nhật thuộc tính TinhTrang
                    // _context.Entry(existingHoaDon).CurrentValues.SetValues(hOADON);

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Sửa thông tin hóa đơn thành công";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi sửa thông tin hóa đơn. Vui lòng thử lại.";
                }
            }

            // Trả về view với thông tin hiện tại nếu có lỗi
            return View(hOADON);
        }


        // GET: CHITIETNHAPs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.CHITIETNHAP == null)
            {
                return NotFound();
            }

            var cHITIETNHAP = await _context.CHITIETNHAP
                .FirstOrDefaultAsync(m => m.MaChiTietNhap == id);
            if (cHITIETNHAP == null)
            {
                return NotFound();
            }

            return View(cHITIETNHAP);
        }

        // POST: CHITIETNHAPs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.CHITIETNHAP == null)
            {
                return Problem("Entity set 'Du_An_OneContext.CHITIETNHAP'  is null.");
            }
            var cHITIETNHAP = await _context.CHITIETNHAP.FindAsync(id);
            try
            {
                if (cHITIETNHAP != null)
                {
                    _context.CHITIETNHAP.Remove(cHITIETNHAP);
                }
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa phiếu nhập thành công!";
                return RedirectToAction("Index"); 
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa dữ liệu. Vui lòng thử lại.";
               
            }
            return View();
        }
        public IActionResult ExportExcelChiTietNhap()
        {
            var phieunhap = _context.CHITIETNHAP.ToList();
            using (var workbook = new XLWorkbook())
            {
                var hanghientai = 1;
                var worksheet = workbook.Worksheets.Add("DanhSachPhieuNhap");

                // Adding headers
                worksheet.Cell(hanghientai, 1).Value = "Mã chi tiết nhập";
                worksheet.Cell(hanghientai, 2).Value = "Mã nhà cung cấp";
                worksheet.Cell(hanghientai, 3).Value = "Mã sản phẩm";
                worksheet.Cell(hanghientai, 4).Value = "Số lượng nhập";
                worksheet.Cell(hanghientai, 5).Value = "Đơn giá nhập";
                worksheet.Cell(hanghientai, 6).Value = "Tổng tiền";

                foreach (var chitietnhap in phieunhap)
                {
                    hanghientai++;
                    var donGia = chitietnhap.DonGiaNhap;
                    var tongTien = donGia * chitietnhap.SoLuongNhap;

                    worksheet.Cell(hanghientai, 1).Value = chitietnhap.MaChiTietNhap;
                    worksheet.Cell(hanghientai, 2).Value = chitietnhap.MaNhaCC;
                    worksheet.Cell(hanghientai, 3).Value = chitietnhap.MaSP;
                    worksheet.Cell(hanghientai, 4).Value = chitietnhap.SoLuongNhap;
                    worksheet.Cell(hanghientai, 5).Value = donGia;
                    worksheet.Cell(hanghientai, 6).Value = tongTien;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachPhieuNhap.xlsx");
                }
            }

        }
        private bool CHITIETNHAPExists(string id)
        {
          return (_context.CHITIETNHAP?.Any(e => e.MaChiTietNhap == id)).GetValueOrDefault();
        }
    }
}
