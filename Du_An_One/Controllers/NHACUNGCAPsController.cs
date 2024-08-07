using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Du_An_One.Data;
using Du_An_One.Models;
using ClosedXML;
using X.PagedList;
using X.PagedList.Extensions;
using ClosedXML.Excel;
namespace Du_An_One.Controllers
{
    public class NHACUNGCAPsController : Controller
    {
        private readonly Du_An_OneContext _context;

        public NHACUNGCAPsController(Du_An_OneContext context)
        {
            _context = context;
        }

        // GET: NHACUNGCAPs
        public async Task<IActionResult> Index(int page = 1)
        {
            page = page < 1 ? 1 : page;
            int pagesize = 10;
            var nhacungcap = _context.NHACUNGCAP.ToPagedList(page, pagesize);
            return View(nhacungcap);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string keywords, int page = 1)
        {
            if (keywords == null)
            {
                return RedirectToAction("Index", "NHACUNGCAPs");
            }
            page = page < 1 ? 1 : page;
            int pagesize = 10;
            var query = _context.NHACUNGCAP.AsQueryable();
            query = query
            .Where(p => p.MaNhaCC.Contains(keywords) || p.TenNhaCC.Contains(keywords));
            var ketquatimkiem = query.ToPagedList(page, pagesize);

            if (!ketquatimkiem.Any())
            {
                
                ketquatimkiem = _context.NHACUNGCAP.ToPagedList(page, pagesize);
                TempData["ErrorMessage"] = "Không tìm thấy nhà cung cấp với từ khóa đã nhập.";
            }

            return View(ketquatimkiem);
        }
        // GET: NHACUNGCAPs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.NHACUNGCAP == null)
            {
                return NotFound();
            }

            var nHACUNGCAP = await _context.NHACUNGCAP
                .FirstOrDefaultAsync(m => m.MaNhaCC == id);
            if (nHACUNGCAP == null)
            {
                return NotFound();
            }

            return View(nHACUNGCAP);
        }

        // GET: NHACUNGCAPs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NHACUNGCAPs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaNhaCC,TenNhaCC,DiaChi,Email,SDT,NgayThanhLap,NguoiDaiDien,ThoiGianCungCap,TinhTrang")] NHACUNGCAP nHACUNGCAP)
        {
            if (ModelState.IsValid)
            {
                bool exists = await _context.NHACUNGCAP.AnyAsync(p => p.MaNhaCC == nHACUNGCAP.MaNhaCC);
               
               
                if (exists)
                {
                    ModelState.AddModelError("MaNhaCC", "Mã này đã tồn tại. Vui lòng nhập lại");
                    return View(nHACUNGCAP);
                }
                try
                {
                    _context.Add(nHACUNGCAP);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Thêm nhà cung cấp thành công";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm sản phẩm. Vui lòng thử lại.";
                }
            }
            return View();
        }

        // GET: NHACUNGCAPs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.NHACUNGCAP == null)
            {
                return NotFound();
            }

            var nHACUNGCAP = await _context.NHACUNGCAP.FindAsync(id);
            if (nHACUNGCAP == null)
            {
                return NotFound();
            }
            return View(nHACUNGCAP);
        }

        // POST: NHACUNGCAPs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaNhaCC,TenNhaCC,DiaChi,Email,SDT,NgayThanhLap,NguoiDaiDien,ThoiGianCungCap,TinhTrang")] NHACUNGCAP nHACUNGCAP)
        {
            if (id != nHACUNGCAP.MaNhaCC)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nHACUNGCAP);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Sửa thông tin nhà cung cấp thành công";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi sửa thông tin nhà cung cấp. Vui lòng thử lại.";
                }
            }
            return View();
        }

        // GET: NHACUNGCAPs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.NHACUNGCAP == null)
            {
                return NotFound();
            }

            var nHACUNGCAP = await _context.NHACUNGCAP
                .FirstOrDefaultAsync(m => m.MaNhaCC == id);
            if (nHACUNGCAP == null)
            {
                return NotFound();
            }

            return View(nHACUNGCAP);
        }

        // POST: NHACUNGCAPs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.NHACUNGCAP == null)
            {
                return Problem("Entity set 'Du_An_OneContext.NHACUNGCAP'  is null.");
            }
            var nHACUNGCAP = await _context.NHACUNGCAP.FindAsync(id);
            try
            {
                if (nHACUNGCAP != null)
                {
                    _context.NHACUNGCAP.Remove(nHACUNGCAP);
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa nhà cung cấp thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa dữ liệu. Vui lòng thử lại.";
            }
            return View();
        }
        public IActionResult ExportExcelNhaCungCap()
        {
            var nhacungcap = _context.NHACUNGCAP.ToList();
            using (var workbook = new XLWorkbook())
            {
                var hanghientai = 1;
                var worksheet = workbook.Worksheets.Add("DanhSachNhaCungCap");
                worksheet.Cell(hanghientai, 1).Value = "Mã nhà cung cấp";
                worksheet.Cell(hanghientai, 2).Value = "Tên nhà cung cấp";
                worksheet.Cell(hanghientai, 3).Value = "Địa chỉ";
                worksheet.Cell(hanghientai, 4).Value = "Email";
                worksheet.Cell(hanghientai, 5).Value = "SDT";
                worksheet.Cell(hanghientai, 6).Value = "Ngày thành lập";
                worksheet.Cell(hanghientai, 7).Value = "Người đại diện";
                worksheet.Cell(hanghientai, 8).Value = "Thời gian cung cấp";
                worksheet.Cell(hanghientai, 9).Value = "Tình trạng";
              
                foreach (var cungcap in nhacungcap)
                {
                    hanghientai++;
                    worksheet.Cell(hanghientai, 1).Value = cungcap.MaNhaCC;
                    worksheet.Cell(hanghientai, 2).Value = cungcap.TenNhaCC;
                    worksheet.Cell(hanghientai, 3).Value = cungcap.DiaChi;
                    worksheet.Cell(hanghientai, 4).Value = cungcap.Email;
                    worksheet.Cell(hanghientai, 5).Value = cungcap.SDT;
                    worksheet.Cell(hanghientai, 6).Value = cungcap.NgayThanhLap;
                    worksheet.Cell(hanghientai, 7).Value = cungcap.NguoiDaiDien;
                    worksheet.Cell(hanghientai, 8).Value = cungcap.ThoiGianCungCap;
                    worksheet.Cell(hanghientai, 9).Value = cungcap.TinhTrang;
                    
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachCungCap.xlsx");
                }
            }
        }
        private bool NHACUNGCAPExists(string id)
        {
            return (_context.NHACUNGCAP?.Any(e => e.MaNhaCC == id)).GetValueOrDefault();
        }
    }
}
