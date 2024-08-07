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
namespace Du_An_One.Controllers
{
    public class KHUYENMAIsController : Controller
    {
        private readonly Du_An_OneContext _context;

        public KHUYENMAIsController(Du_An_OneContext context)
        {
            _context = context;
        }

        // GET: KHUYENMAIs
        public async Task<IActionResult> Index(int page = 1)
        {
            page = page < 1 ? 1 : page;
            int pagesize = 10;
            var khuyenmai = _context.KHUYENMAI.ToPagedList(page, pagesize);
            return View(khuyenmai);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string keywords, int page = 1)
        {
            if (keywords == null)
            {
                return RedirectToAction("Index", "KHUYENMAIs");
            }
            page = page < 1 ? 1 : page;
            int pagesize = 10;
            var query = _context.KHUYENMAI.AsQueryable();
            query = query.Where(p => p.MaKhuyenMai.Contains(keywords));
            var ketquatimkiem = query.ToPagedList(page, pagesize);

            if (!ketquatimkiem.Any())
            {
                // Nếu không tìm thấy kết quả, trả về toàn bộ danh sách khách hàng
                ketquatimkiem = _context.KHUYENMAI.ToPagedList(page, pagesize);
                TempData["ErrorMessage"] = "Không tìm thấy mã khuyến mãi với từ khóa đã nhập.";
            }

            return View(ketquatimkiem);
        }
        // GET: KHUYENMAIs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.KHUYENMAI == null)
            {
                return NotFound();
            }

            var kHUYENMAI = await _context.KHUYENMAI
                .FirstOrDefaultAsync(m => m.MaKhuyenMai == id);
            if (kHUYENMAI == null)
            {
                return NotFound();
            }

            return View(kHUYENMAI);
        }

        // GET: KHUYENMAIs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KHUYENMAIs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaKhuyenMai,PhanTramKhuyenMai,ThoiGianStart,ThoiGianEnd")] KHUYENMAI kHUYENMAI)
        {
            DateTime today = DateTime.Now;
            if (ModelState.IsValid)
            {
                bool exists = await _context.KHUYENMAI.AnyAsync(p => p.MaKhuyenMai == kHUYENMAI.MaKhuyenMai);
                if (kHUYENMAI.ThoiGianStart <= today)
                {
                    ModelState.AddModelError("ThoiGianStart", "Thời gian bắt đầu tối thiểu phải hôm nay hoặc sau thời gian hôm nay.");
                    return View(kHUYENMAI);
                }
                if (kHUYENMAI.ThoiGianStart >= kHUYENMAI.ThoiGianEnd)
                {
                    ModelState.AddModelError("ThoiGianEnd", "Thời gian kết thúc phải đi sau thời gian bắt đầu");
                    return View(kHUYENMAI);
                }
                if (exists)
                {
                    ModelState.AddModelError("MaKhuyenMai", "Mã này đã tồn tại. Vui lòng nhập lại");
                    return View(kHUYENMAI);
                }
                try
                {
                    _context.Add(kHUYENMAI);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Thêm khuyến mãi thành công";
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm khuyến mãi. Vui lòng thử lại.";
                }
            }
            return View();
        }

        // GET: KHUYENMAIs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.KHUYENMAI == null)
            {
                return NotFound();
            }

            var kHUYENMAI = await _context.KHUYENMAI.FindAsync(id);
            if (kHUYENMAI == null)
            {
                return NotFound();
            }
            return View(kHUYENMAI);
        }

        // POST: KHUYENMAIs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaKhuyenMai,PhanTramKhuyenMai,ThoiGianStart,ThoiGianEnd")] KHUYENMAI kHUYENMAI)
        {
            DateTime today = DateTime.Now;
            if (id != kHUYENMAI.MaKhuyenMai)
            {
                return NotFound();
            }

            if (kHUYENMAI.ThoiGianStart <= today)
            {
                ModelState.AddModelError("ThoiGianStart", "Thời gian bắt đầu tối thiểu phải hôm nay hoặc sau thời gian hôm nay.");
                return View(kHUYENMAI);
            }
            if (kHUYENMAI.ThoiGianStart >= kHUYENMAI.ThoiGianEnd)
            {
                ModelState.AddModelError("ThoiGianEnd", "Thời gian kết thúc phải đi sau thời gian bắt đầu");
                return View(kHUYENMAI);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kHUYENMAI);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Sửa thông tin khuyến mãi thành công";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi sửa thông tin khuyến mãi. Vui lòng thử lại.";
                }
            }
            return View();
        }

        // GET: KHUYENMAIs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.KHUYENMAI == null)
            {
                return NotFound();
            }

            var kHUYENMAI = await _context.KHUYENMAI
                .FirstOrDefaultAsync(m => m.MaKhuyenMai == id);
            if (kHUYENMAI == null)
            {
                return NotFound();
            }

            return View(kHUYENMAI);
        }

        // POST: KHUYENMAIs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.KHUYENMAI == null)
            {
                return Problem("Entity set 'Du_An_OneContext.KHUYENMAI'  is null.");
            }
            var kHUYENMAI = await _context.KHUYENMAI.FindAsync(id);
            try
            {
                if (kHUYENMAI != null)
                {
                    _context.KHUYENMAI.Remove(kHUYENMAI);
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa khuyến mãi thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa dữ liệu. Vui lòng thử lại.";
            }
            return View();
        }

        private bool KHUYENMAIExists(string id)
        {
          return (_context.KHUYENMAI?.Any(e => e.MaKhuyenMai == id)).GetValueOrDefault();
        }
    }
}
