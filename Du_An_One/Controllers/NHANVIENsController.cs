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
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using DocumentFormat.OpenXml.Office2010.Excel;
using ClosedXML.Excel;
using X.PagedList.Extensions;
using X.PagedList;

namespace Du_An_One.Controllers
{
    [Authorize(Roles = "Quản lý")]
    public class NHANVIENsController : Controller
    {
        private readonly Du_An_OneContext _context;

        public NHANVIENsController(Du_An_OneContext context)
        {
            _context = context;
        }

        // GET: NHANVIENs
        public async Task<IActionResult> Index(int page = 1)
        {
            page = page < 1 ? 1 : page; 
            int pagesize = 10;
            var nhanvien = _context.NHANVIEN.ToPagedList(page, pagesize);
            return View(nhanvien);
            //return _context.NHANVIEN != null ?
            //            View(await _context.NHANVIEN.Where(p => p.TinhTrang == "Mở").ToListAsync()) :
            //            Problem("Entity set 'Du_An_OneContext.NHANVIEN'  is null.");
        }

        [HttpPost]
        public async Task<IActionResult> Index(string keywords, int page = 1)
        {
            if (keywords == null)
            {
                return RedirectToAction("Index", "NHANVIENs");
            }
            page = page < 1 ? 1 : page;
            int pagesize = 10;
            var query = _context.NHANVIEN.AsQueryable();
            query = query
            .Where(p => p.MaNV.Contains(keywords) || p.TenTaiKhoan.Contains(keywords));
            var ketquatimkiem = query.ToPagedList(page, pagesize);

            if (!ketquatimkiem.Any())
            {
                // Nếu không tìm thấy kết quả, trả về toàn bộ danh sách nhân viên
                ketquatimkiem = _context.NHANVIEN.ToPagedList(page, pagesize);
                TempData["ErrorMessage"] = "Không tìm thấy nhân viên với từ khóa đã nhập.";
            }

            return View(ketquatimkiem);
        }
        //public IActionResult ShowNhanVienPaging(int page = 1)
        //{
        //    int pagesize = 1;
        //    var nhanvien = _context.NHANVIEN.ToPagedList(page, pagesize);
        //    return View(nhanvien);
        //}
        // GET: NHANVIENs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.NHANVIEN == null)
            {
                return NotFound();
            }

            var nHANVIEN = await _context.NHANVIEN
                .FirstOrDefaultAsync(m => m.MaNV == id);
            if (nHANVIEN == null)
            {
                return NotFound();
            }

            return View(nHANVIEN);
        }

        // GET: NHANVIENs/Create
      
        public IActionResult Create()
        {
            return View();
        }

        // POST: NHANVIENs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]


        public async Task<IActionResult> Create([Bind("MaNV,HoTen,NgaySinh,NoiSinh,DiaChi,CCCD,SDT,Email,NgayVaoLam,Luong,VaiTro,TenTaiKhoan,MatKhau,TinhTrang")] NHANVIEN nHANVIEN)
        {

            if (ModelState.IsValid)
            {
                bool exists = await _context.NHANVIEN.AnyAsync(p => p.MaNV == nHANVIEN.MaNV);
                bool checktaikhoan = await _context.NHANVIEN.AnyAsync(p => p.TenTaiKhoan == nHANVIEN.TenTaiKhoan);
                if (nHANVIEN.Luong < 0)
                {
                    ModelState.AddModelError("Luong", "Lương không được là giá trị âm");
                    return View(nHANVIEN);
                }
                if (checktaikhoan)
                {
                    ModelState.AddModelError("TenTaiKhoan", "Tên tài khoản này đã tồn tại. Vui lòng nhập lại");
                    return View(nHANVIEN);
                }
                if (exists)
                {
                    ModelState.AddModelError("MaNV", "Mã này đã tồn tại. Vui lòng nhập lại");
                    return View(nHANVIEN);
                }
                try
                {
                    _context.Add(nHANVIEN);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Thêm nhân viên thành công";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm nhân viên. Vui lòng thử lại.";
                }
            }
            return View(nHANVIEN);
        }

        // GET: NHANVIENs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.NHANVIEN == null)
            {
                return NotFound();
            }

            var nHANVIEN = await _context.NHANVIEN.FindAsync(id);
            if (nHANVIEN == null)
            {
                return NotFound();
            }
            return View(nHANVIEN);
        }

        // POST: NHANVIENs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaNV,HoTen,NgaySinh,NoiSinh,DiaChi,CCCD,SDT,Email,NgayVaoLam,Luong,VaiTro,TenTaiKhoan,MatKhau,TinhTrang")] NHANVIEN nHANVIEN)
        {
            if (id != nHANVIEN.MaNV)
            {
                return NotFound();
            }
            bool checktaikhoan = await _context.NHANVIEN.AnyAsync(p => p.TenTaiKhoan == nHANVIEN.TenTaiKhoan && p.MaNV != id);
            if (checktaikhoan)
            {
                ModelState.AddModelError("TenTaiKhoan", "Tên tài khoản này đã tồn tại. Vui lòng nhập lại");
                return View(nHANVIEN);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nHANVIEN);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Sửa thông tin nhân viên thành công";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi sửa thông tin nhân viên. Vui lòng thử lại.";
                }
            }
            return View();
        }

        // GET: NHANVIENs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.NHANVIEN == null)
            {
                return NotFound();
            }

            var nHANVIEN = await _context.NHANVIEN
                .FirstOrDefaultAsync(m => m.MaNV == id);
            if (nHANVIEN == null)
            {
                return NotFound();
            }

            return View(nHANVIEN);
        }

        // POST: NHANVIENs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.NHANVIEN == null)
            {
                return Problem("Entity set 'Du_An_OneContext.NHANVIEN'  is null.");
            }
            var nHANVIEN = await _context.NHANVIEN.FindAsync(id);
            try
            {
                if (nHANVIEN != null)
                {
                    _context.NHANVIEN.Remove(nHANVIEN);
                }
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa nhân viên thành công!";
                return RedirectToAction("Index"); // Hoặc trang bạn muốn chuyển hướng đến
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa dữ liệu. Vui lòng thử lại.";
                
            }
            return View();
        }
        public IActionResult ExportExcelNhanVien()
        {
            var nhanvien = _context.NHANVIEN.ToList();
            using (var workbook = new XLWorkbook())
            {
                var hanghientai = 1;
                var worksheet = workbook.Worksheets.Add("DanhSachNhanVien");
                worksheet.Cell(hanghientai, 1).Value = "Mã nhân viên";
                worksheet.Cell(hanghientai, 2).Value = "Họ và tên";
                worksheet.Cell(hanghientai, 3).Value = "Ngày sinh";
                worksheet.Cell(hanghientai, 4).Value = "Nơi sinh";
                worksheet.Cell(hanghientai, 5).Value = "Địa chỉ";
                worksheet.Cell(hanghientai, 6).Value = "CCCD";
                worksheet.Cell(hanghientai, 7).Value = "SDT";
                worksheet.Cell(hanghientai, 8).Value = "Email";
                worksheet.Cell(hanghientai, 9).Value = "Ngày vào làm";
                worksheet.Cell(hanghientai, 10).Value = "Lương";
                worksheet.Cell(hanghientai, 11).Value = "Vai trò";
                worksheet.Cell(hanghientai, 12).Value = "Tên tài khoản";
                worksheet.Cell(hanghientai, 13).Value = "Mật khẩu";
                worksheet.Cell(hanghientai, 14).Value = "Tình trạng";
                foreach (var customer in nhanvien)
                {
                    hanghientai++;
                    worksheet.Cell(hanghientai, 1).Value = customer.MaNV;
                    worksheet.Cell(hanghientai, 2).Value = customer.HoTen;
                    worksheet.Cell(hanghientai, 3).Value = customer.NgaySinh;
                    worksheet.Cell(hanghientai, 4).Value = customer.NoiSinh;
                    worksheet.Cell(hanghientai, 5).Value = customer.DiaChi;
                    worksheet.Cell(hanghientai, 6).Value = customer.CCCD;
                    worksheet.Cell(hanghientai, 7).Value = customer.SDT;
                    worksheet.Cell(hanghientai, 8).Value = customer.Email;
                    worksheet.Cell(hanghientai, 9).Value = customer.NgayVaoLam.ToString("dd/MM/yyyy");
                    worksheet.Cell(hanghientai, 10).Value = customer.Luong;
                    worksheet.Cell(hanghientai, 11).Value = customer.VaiTro;
                    worksheet.Cell(hanghientai, 12).Value = customer.TenTaiKhoan;
                    worksheet.Cell(hanghientai, 13).Value = customer.MatKhau;
                    worksheet.Cell(hanghientai, 14).Value = customer.TinhTrang;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachNhanVien.xlsx");
                }
            }
        }
    
        private bool NHANVIENExists(string id)
        {
            return (_context.NHANVIEN?.Any(e => e.MaNV == id)).GetValueOrDefault();
        }
    }
}
