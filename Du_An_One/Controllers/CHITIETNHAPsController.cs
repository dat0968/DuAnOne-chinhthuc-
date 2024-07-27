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
    public class CHITIETNHAPsController : Controller
    {
        private readonly Du_An_OneContext _context;

        public CHITIETNHAPsController(Du_An_OneContext context)
        {
            _context = context;
        }

        // GET: CHITIETNHAPs
        public async Task<IActionResult> Index()
        {
              return _context.CHITIETNHAP != null ? 
                          View(await _context.CHITIETNHAP.ToListAsync()) :
                          Problem("Entity set 'Du_An_OneContext.CHITIETNHAP'  is null.");
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
                _context.Add(cHITIETNHAP);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(string id, [Bind("MaChiTietNhap,MaNhaCC,MaSP,SoLuongNhap,DonGiaNhap")] CHITIETNHAP cHITIETNHAP)
        {
            if (id != cHITIETNHAP.MaChiTietNhap)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cHITIETNHAP);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CHITIETNHAPExists(cHITIETNHAP.MaChiTietNhap))
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
            return View(cHITIETNHAP);
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
            if (cHITIETNHAP != null)
            {
                _context.CHITIETNHAP.Remove(cHITIETNHAP);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CHITIETNHAPExists(string id)
        {
          return (_context.CHITIETNHAP?.Any(e => e.MaChiTietNhap == id)).GetValueOrDefault();
        }
    }
}
