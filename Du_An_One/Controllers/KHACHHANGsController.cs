using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Du_An_One.Data;
using Du_An_One.Models;
using Microsoft.AspNetCore.Authorization;
using ClosedXML.Excel;
using Irony.Parsing;
using X.PagedList;
using X.PagedList.Extensions;
namespace Du_An_One.Controllers
{
    public class KHACHHANGsController : Controller
    {
        private readonly Du_An_OneContext _context;

        public KHACHHANGsController(Du_An_OneContext context)
        {
            _context = context;
        }

        // GET: KHACHHANGs
        public async Task<IActionResult> Index(int page = 1)
        {
            page = page < 1 ? 1 : page;
            int pagesize = 10;
            var khachhang = _context.KHACHHANG.ToPagedList(page, pagesize);
            return View(khachhang);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string keywords, int page = 1)
        {
            if(keywords == null)
            {
                return RedirectToAction("Index", "KHACHHANGs");
            }
            page = page < 1 ? 1 : page;
            int pagesize = 10;
            var query = _context.KHACHHANG.AsQueryable();
            query = query
            .Where(p => p.MaKH.Contains(keywords) || p.TenTaiKhoan.Contains(keywords));
            var ketquatimkiem = query.ToPagedList(page, pagesize);

            if (!ketquatimkiem.Any())
            {
                // Nếu không tìm thấy kết quả, trả về toàn bộ danh sách khách hàng
                ketquatimkiem = _context.KHACHHANG.ToPagedList(page, pagesize);
                TempData["ErrorMessage"] = "Không tìm thấy khách hàng với từ khóa đã nhập.";
            }

            return View(ketquatimkiem);
        }

        // GET: KHACHHANGs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.KHACHHANG == null)
            {
                return NotFound();
            }

            var kHACHHANG = await _context.KHACHHANG
                .FirstOrDefaultAsync(m => m.MaKH == id);
            if (kHACHHANG == null)
            {
                return NotFound();
            }

            return View(kHACHHANG);
        }

        // GET: KHACHHANGs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KHACHHANGs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id, [Bind("MaKH,HoTen,NgaySinh,NoiSinh,DiaChi,CCCD,SDT,Email,TenTaiKhoan,MatKhau,TinhTrang")] KHACHHANG kHACHHANG)
        {
            
            if (ModelState.IsValid)
            {
                bool exists = await _context.KHACHHANG.AnyAsync(p => p.MaKH == kHACHHANG.MaKH);
                bool checktaikhoan = await _context.KHACHHANG.AnyAsync(p => p.TenTaiKhoan == kHACHHANG.TenTaiKhoan);
                if (checktaikhoan)
                {
                    ModelState.AddModelError("TenTaiKhoan", "Tên tài khoản này đã tồn tại. Vui lòng nhập lại");
                    return View(kHACHHANG);
                }
                if (exists)
                {
                    ModelState.AddModelError("MaKH", "Mã này đã tồn tại. Vui lòng nhập lại");
                    return View(kHACHHANG);
                }
                try
                {
                    _context.Add(kHACHHANG);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Thêm khách hàng thành công";
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm sản phẩm. Vui lòng thử lại.";
                }
            }
            return View();
        }

        // GET: KHACHHANGs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.KHACHHANG == null)
            {
                return NotFound();
            }

            var kHACHHANG = await _context.KHACHHANG.FindAsync(id);
            if (kHACHHANG == null)
            {
                return NotFound();
            }
            return View(kHACHHANG);
        }

        // POST: KHACHHANGs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaKH,HoTen,NgaySinh,NoiSinh,DiaChi,CCCD,SDT,Email,TenTaiKhoan,MatKhau,TinhTrang")] KHACHHANG kHACHHANG)
        {
            if (id != kHACHHANG.MaKH)
            {
                return NotFound();
            }
            bool checktaikhoan = await _context.KHACHHANG.AnyAsync(p => p.TenTaiKhoan == kHACHHANG.TenTaiKhoan && p.MaKH != id);
            if (checktaikhoan)
            {
                ModelState.AddModelError("TenTaiKhoan", "Tên tài khoản này đã tồn tại. Vui lòng nhập lại");
                return View(kHACHHANG);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kHACHHANG);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Sửa thông tin khách hàng thành công";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi sửa thông tin khách hàng. Vui lòng thử lại.";
                }
            }
            return View();
        }

        // GET: KHACHHANGs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.KHACHHANG == null)
            {
                return NotFound();
            }

            var kHACHHANG = await _context.KHACHHANG
                .FirstOrDefaultAsync(m => m.MaKH == id);
            if (kHACHHANG == null)
            {
                return NotFound();
            }

            return View(kHACHHANG);
        }

        // POST: KHACHHANGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.KHACHHANG == null)
            {
                return Problem("Entity set 'Du_An_OneContext.KHACHHANG'  is null.");
            }
            var kHACHHANG = await _context.KHACHHANG.FindAsync(id);
            try
            {
                if (kHACHHANG != null)
                {
                    _context.KHACHHANG.Remove(kHACHHANG);
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa khách hàng thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa dữ liệu. Vui lòng thử lại.";
            }
            return View();
        }
        public IActionResult ExportExcelKhachHang()
        {
            var khachhang = _context.KHACHHANG.ToList();
            using (var workbook = new XLWorkbook())
            {
                var hanghientai = 1;
                var worksheet = workbook.Worksheets.Add("DanhSachKhachHang");
                worksheet.Cell(hanghientai, 1).Value = "Mã Khách hàng";
                worksheet.Cell(hanghientai, 2).Value = "Họ và tên";
                worksheet.Cell(hanghientai, 3).Value = "Ngày sinh";
                worksheet.Cell(hanghientai, 4).Value = "Nơi sinh";
                worksheet.Cell(hanghientai, 5).Value = "Địa chỉ";
                worksheet.Cell(hanghientai, 6).Value = "CCCD";
                worksheet.Cell(hanghientai, 7).Value = "SDT";
                worksheet.Cell(hanghientai, 8).Value = "Email";
                worksheet.Cell(hanghientai, 9).Value = "Tên tài khoản";
                worksheet.Cell(hanghientai, 10).Value = "Mật khẩu";
                worksheet.Cell(hanghientai, 11).Value = "Tình trạng";
                foreach (var customer in khachhang)
                {
                    hanghientai++;
                    worksheet.Cell(hanghientai, 1).Value = customer.MaKH;
                    worksheet.Cell(hanghientai, 2).Value = customer.HoTen;
                    worksheet.Cell(hanghientai, 3).Value = customer.NgaySinh;
                    worksheet.Cell(hanghientai, 4).Value = customer.NoiSinh;
                    worksheet.Cell(hanghientai, 5).Value = customer.DiaChi;
                    worksheet.Cell(hanghientai, 6).Value = customer.CCCD;
                    worksheet.Cell(hanghientai, 7).Value = customer.SDT;
                    worksheet.Cell(hanghientai, 8).Value = customer.Email;
                    worksheet.Cell(hanghientai, 9).Value = customer.TenTaiKhoan;
                    worksheet.Cell(hanghientai, 10).Value = customer.MatKhau;
                    worksheet.Cell(hanghientai, 11).Value = customer.TinhTrang;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachKhachHang.xlsx");
                }
            }
        }

        private bool KHACHHANGExists(string id)
        {
          return (_context.KHACHHANG?.Any(e => e.MaKH == id)).GetValueOrDefault();
        }
    }
}
