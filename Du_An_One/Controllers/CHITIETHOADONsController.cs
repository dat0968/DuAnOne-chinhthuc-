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
    public class CHITIETHOADONsController : Controller
    {
        private readonly Du_An_OneContext _context;

        public CHITIETHOADONsController(Du_An_OneContext context)
        {
            _context = context;
        }

        // GET: CHITIETHOADONs
        public async Task<IActionResult> Index()
        {
              return _context.CHITIETHOADON != null ? 
                          View(await _context.CHITIETHOADON.ToListAsync()) :
                          Problem("Entity set 'Du_An_OneContext.CHITIETHOADON'  is null.");
        }

        // GET: CHITIETHOADONs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CHITIETHOADON == null)
            {
                return NotFound();
            }

            var cHITIETHOADON = await _context.CHITIETHOADON
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

            var cHITIETHOADON = await _context.CHITIETHOADON.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,MaHoaDon,MaSP,SoLuongMua,DonGia")] CHITIETHOADON cHITIETHOADON)
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
            return View(cHITIETHOADON);
        }

        // GET: CHITIETHOADONs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CHITIETHOADON == null)
            {
                return NotFound();
            }

            var cHITIETHOADON = await _context.CHITIETHOADON
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
            if (cHITIETHOADON != null)
            {
                _context.CHITIETHOADON.Remove(cHITIETHOADON);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CHITIETHOADONExists(int id)
        {
          return (_context.CHITIETHOADON?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
