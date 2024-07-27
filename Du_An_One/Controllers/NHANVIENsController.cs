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

namespace Du_An_One.Controllers
{
    [Authorize]
    public class NHANVIENsController : Controller
    {
        private readonly Du_An_OneContext _context;

        public NHANVIENsController(Du_An_OneContext context)
        {
            _context = context;
        }

        // GET: NHANVIENs
        [Route("Admin/NHANVIENS")]
        public async Task<IActionResult> Index()
        {
              return _context.NHANVIEN != null ? 
                          View(await _context.NHANVIEN.Where(p=> p.TinhTrang == "Mở").ToListAsync()) :
                          Problem("Entity set 'Du_An_OneContext.NHANVIEN'  is null.");
        }

        // GET: NHANVIENs/Details/5
        
        [Route("Admin/NHANVIENS/Details/{id}")]
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
        [Route("Admin/NHANVIENs/Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: NHANVIENs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/NHANVIENs/Create")]
        public IActionResult Create(NHANVIEN obj)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "Thêm Nhân Viên Thành Công";
                _context.NHANVIEN.Add(obj);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    // Log or display the error message
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            return View(obj);
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
        public IActionResult Edit(NHANVIEN model)
        {
            if (ModelState.IsValid)
            {

                _context.NHANVIEN.Update(model);
                _context.SaveChanges();
                TempData["success"] = "Sửa Nhân Viên Thành Công";
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: NHANVIENs/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(string maNV)
        {
            var product = await _context.NHANVIEN.FindAsync(maNV);
            if (product != null)
            {
                product.TinhTrang = "Khoas";
                _context.NHANVIEN.Update(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        private bool ProductExists(string maNV)
        {
            return _context.NHANVIEN.Any(e => e.MaNV == maNV);
        }

    }
}
