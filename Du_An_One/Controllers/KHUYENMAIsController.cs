using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Du_An_One.Data;
using Du_An_One.Models;

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
        public async Task<IActionResult> Index()
        {
              return _context.KHUYENMAI != null ? 
                          View(await _context.KHUYENMAI.ToListAsync()) :
                          Problem("Entity set 'Du_An_OneContext.KHUYENMAI'  is null.");
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
            if (ModelState.IsValid)
            {
                _context.Add(kHUYENMAI);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kHUYENMAI);
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
            if (id != kHUYENMAI.MaKhuyenMai)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kHUYENMAI);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KHUYENMAIExists(kHUYENMAI.MaKhuyenMai))
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
            return View(kHUYENMAI);
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
            if (kHUYENMAI != null)
            {
                _context.KHUYENMAI.Remove(kHUYENMAI);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KHUYENMAIExists(string id)
        {
          return (_context.KHUYENMAI?.Any(e => e.MaKhuyenMai == id)).GetValueOrDefault();
        }
    }
}
