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
    public class HINHANHsController : Controller
    {
        private readonly Du_An_OneContext _context;

        public HINHANHsController(Du_An_OneContext context)
        {
            _context = context;
        }

        // GET: HINHANHs
        public async Task<IActionResult> Index()
        {
              return _context.HINHANH != null ? 
                          View(await _context.HINHANH.ToListAsync()) :
                          Problem("Entity set 'Du_An_OneContext.HINHANH'  is null.");
        }

        // GET: HINHANHs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HINHANH == null)
            {
                return NotFound();
            }

            var hINHANH = await _context.HINHANH
                .FirstOrDefaultAsync(m => m.MaHinhAnh == id);
            if (hINHANH == null)
            {
                return NotFound();
            }

            return View(hINHANH);
        }

        // GET: HINHANHs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HINHANHs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHinhAnh,HinhAnh,MaSP")] HINHANH hINHANH)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hINHANH);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hINHANH);
        }

        // GET: HINHANHs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HINHANH == null)
            {
                return NotFound();
            }

            var hINHANH = await _context.HINHANH.FindAsync(id);
            if (hINHANH == null)
            {
                return NotFound();
            }
            return View(hINHANH);
        }

        // POST: HINHANHs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaHinhAnh,HinhAnh,MaSP")] HINHANH hINHANH)
        {
            if (id != hINHANH.MaHinhAnh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hINHANH);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HINHANHExists(hINHANH.MaHinhAnh))
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
            return View(hINHANH);
        }

        // GET: HINHANHs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HINHANH == null)
            {
                return NotFound();
            }

            var hINHANH = await _context.HINHANH
                .FirstOrDefaultAsync(m => m.MaHinhAnh == id);
            if (hINHANH == null)
            {
                return NotFound();
            }

            return View(hINHANH);
        }

        // POST: HINHANHs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HINHANH == null)
            {
                return Problem("Entity set 'Du_An_OneContext.HINHANH'  is null.");
            }
            var hINHANH = await _context.HINHANH.FindAsync(id);
            if (hINHANH != null)
            {
                _context.HINHANH.Remove(hINHANH);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HINHANHExists(int id)
        {
          return (_context.HINHANH?.Any(e => e.MaHinhAnh == id)).GetValueOrDefault();
        }
    }
}
