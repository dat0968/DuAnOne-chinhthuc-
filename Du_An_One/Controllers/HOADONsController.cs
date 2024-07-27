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
    public class HOADONsController : Controller
    {
        private readonly Du_An_OneContext _context;

        public HOADONsController(Du_An_OneContext context)
        {
            _context = context;
        }

        // GET: HOADONs
        public async Task<IActionResult> Index()
        {
              return _context.HOADON != null ? 
                          View(await _context.HOADON.ToListAsync()) :
                          Problem("Entity set 'Du_An_OneContext.HOADON'  is null.");
        }

        // GET: HOADONs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.HOADON == null)
            {
                return NotFound();
            }

            var hOADON = await _context.HOADON
                .FirstOrDefaultAsync(m => m.MaHoaDon == id);
            if (hOADON == null)
            {
                return NotFound();
            }

            return View(hOADON);
        }

        // GET: HOADONs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HOADONs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHoaDon,DiaChiNhanHang,NgayTao,HTTT,TinhTrang,MaNV,MaKH")] HOADON hOADON)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hOADON);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hOADON);
        }

        // GET: HOADONs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.HOADON == null)
            {
                return NotFound();
            }

            var hOADON = await _context.HOADON.FindAsync(id);
            if (hOADON == null)
            {
                return NotFound();
            }
            return View(hOADON);
        }

        // POST: HOADONs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaHoaDon,DiaChiNhanHang,NgayTao,HTTT,TinhTrang,MaNV,MaKH")] HOADON hOADON)
        {
            if (id != hOADON.MaHoaDon)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hOADON);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HOADONExists(hOADON.MaHoaDon))
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
            return View(hOADON);
        }

        // GET: HOADONs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.HOADON == null)
            {
                return NotFound();
            }

            var hOADON = await _context.HOADON
                .FirstOrDefaultAsync(m => m.MaHoaDon == id);
            if (hOADON == null)
            {
                return NotFound();
            }

            return View(hOADON);
        }

        // POST: HOADONs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.HOADON == null)
            {
                return Problem("Entity set 'Du_An_OneContext.HOADON'  is null.");
            }
            var hOADON = await _context.HOADON.FindAsync(id);
            if (hOADON != null)
            {
                _context.HOADON.Remove(hOADON);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HOADONExists(string id)
        {
          return (_context.HOADON?.Any(e => e.MaHoaDon == id)).GetValueOrDefault();
        }
    }
}
