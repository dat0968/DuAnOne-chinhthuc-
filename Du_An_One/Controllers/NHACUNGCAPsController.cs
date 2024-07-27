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
    public class NHACUNGCAPsController : Controller
    {
        private readonly Du_An_OneContext _context;

        public NHACUNGCAPsController(Du_An_OneContext context)
        {
            _context = context;
        }

        // GET: NHACUNGCAPs
        public async Task<IActionResult> Index()
        {
              return _context.NHACUNGCAP != null ? 
                          View(await _context.NHACUNGCAP.ToListAsync()) :
                          Problem("Entity set 'Du_An_OneContext.NHACUNGCAP'  is null.");
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
                _context.Add(nHACUNGCAP);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nHACUNGCAP);
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
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NHACUNGCAPExists(nHACUNGCAP.MaNhaCC))
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
            return View(nHACUNGCAP);
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
            if (nHACUNGCAP != null)
            {
                _context.NHACUNGCAP.Remove(nHACUNGCAP);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NHACUNGCAPExists(string id)
        {
          return (_context.NHACUNGCAP?.Any(e => e.MaNhaCC == id)).GetValueOrDefault();
        }
    }
}
