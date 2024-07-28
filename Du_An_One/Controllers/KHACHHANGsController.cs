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
using X.PagedList;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace Du_An_One.Controllers
{
    public class KHACHHANGsController : Controller
    {
        private readonly Du_An_OneContext _context;

        public KHACHHANGsController(Du_An_OneContext context)
        {
            _context = context;
        }

        // GET: KHACHHANGs
        public async Task<IActionResult> Index()
        {
              return _context.KHACHHANG != null ? 
                          View(await _context.KHACHHANG.ToListAsync()) :
                          Problem("Entity set 'Du_An_OneContext.KHACHHANG'  is null.");
        }

        // GET: KHACHHANGs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.KHACHHANG == null)
            {
                return NotFound();
            }

            var kHACHHANG = await _context.KHACHHANG
                .FirstOrDefaultAsync(m => m.MaKH == id);
            if (kHACHHANG == null)
            {
                return NotFound();
            }

            return View(kHACHHANG);
        }

        // GET: KHACHHANGs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KHACHHANGs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaKH,HoTen,NgaySinh,NoiSinh,DiaChi,CCCD,SDT,Email,TenTaiKhoan,MatKhau,TinhTrang")] KHACHHANG kHACHHANG)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kHACHHANG);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kHACHHANG);
        }

        // GET: KHACHHANGs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.KHACHHANG == null)
            {
                return NotFound();
            }

            var kHACHHANG = await _context.KHACHHANG.FindAsync(id);
            if (kHACHHANG == null)
            {
                return NotFound();
            }
            return View(kHACHHANG);
        }

        // POST: KHACHHANGs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaKH,HoTen,NgaySinh,NoiSinh,DiaChi,CCCD,SDT,Email,TenTaiKhoan,MatKhau,TinhTrang")] KHACHHANG kHACHHANG)
        {
            if (id != kHACHHANG.MaKH)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kHACHHANG);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KHACHHANGExists(kHACHHANG.MaKH))
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
            return View(kHACHHANG);
        }

        // GET: KHACHHANGs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.KHACHHANG == null)
            {
                return NotFound();
            }

            var kHACHHANG = await _context.KHACHHANG
                .FirstOrDefaultAsync(m => m.MaKH == id);
            if (kHACHHANG == null)
            {
                return NotFound();
            }

            return View(kHACHHANG);
        }

        // POST: KHACHHANGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.KHACHHANG == null)
            {
                return Problem("Entity set 'Du_An_OneContext.KHACHHANG'  is null.");
            }
            var kHACHHANG = await _context.KHACHHANG.FindAsync(id);
            if (kHACHHANG != null)
            {
                _context.KHACHHANG.Remove(kHACHHANG);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: KHACHHANGs/InfoCustomer/5
        public async Task<IActionResult> InfoCustomer(string id)
        {
            if (id == null || _context.KHACHHANG == null)
            {
                return NotFound();
            }

            var kHACHHANG = await _context.KHACHHANG
                .FirstOrDefaultAsync(m => m.MaKH == id);
            if (kHACHHANG == null)
            {
                return NotFound();
            }
            ViewBag.IdUser = id;
            return View(kHACHHANG);
        }

        // POST: KHACHHANGs/InfoCustomer/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InfoCustomer( KHACHHANG kHACHHANG)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kHACHHANG);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KHACHHANGExists(kHACHHANG.MaKH))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewBag.IdUser = kHACHHANG.MaKH;
                return View(kHACHHANG);
            }
            return View(kHACHHANG);
        }

        // GET: KHACHHANGs/ListOrdersOfCustomer/5
        public async Task<IActionResult> ListOrdersOfCustomer(int? page, string? idUser, string? statusOrder, string? findOrder)
        {
            idUser = String.IsNullOrEmpty(idUser)
                ? _context.HOADON.GroupBy(x => x.MaKH)
                      .OrderByDescending(g => g.Count())
                      .Select(g => g.Key)
                      .FirstOrDefault()
                : idUser;

            int pageSize = 16;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;

            // Retrieve orders for the specified user
            IQueryable<Models.HOADON> listOrder = _context.HOADON
                .AsNoTracking()
                .Include(h => h.CHITIETHOADONs) // Include CHITIETHOADONs to ensure they are loaded
                .Where(x => x.MaKH == idUser);

            // Filter by status if specified
            if (!String.IsNullOrEmpty(statusOrder))
            {
                listOrder = listOrder.Where(x => x.TinhTrang == statusOrder);
            }

            // If findOrder is not null or empty, filter orders based on the findOrder parameter
            if (!String.IsNullOrEmpty(findOrder))
            {
                listOrder = listOrder.Where(x =>
                    x.MaHoaDon.Contains(findOrder) ||
                    x.TinhTrang.Contains(findOrder) ||
                    x.HTTT.Contains(findOrder));
            }

            ViewBag.StatusOrder = statusOrder;

            // Apply pagination
            PagedList<Models.HOADON> lstPaged = new PagedList<Models.HOADON>(listOrder, pageNumber, pageSize);

            ViewBag.IdUser = idUser;
            // Return the filtered list of orders to the view
            return View(lstPaged);
        }

        // GET: KHACHHANGs/PrintCheckOrder/5
        public async Task<IActionResult> PrintCheckOrder(string idCheck) { return View(); }

        // GET: KHACHHANGs/ListProductsInBag/5
        public async Task<IActionResult> ListProductsInBagOfCustomer(string idUser, string? textFind)
        {
            ViewBag.IdUser = idUser;
            string codeBag = _context.HOADON.FirstOrDefault(hd => hd.MaKH == idUser && hd.TinhTrang == "Chờ thanh toán")?.MaHoaDon ?? "";
            if (string.IsNullOrEmpty(codeBag))
            {
                return View(new List<CHITIETHOADON>());
            }
            var listProductInBag = _context.CHITIETHOADON.Include(sp => sp.SANPHAM).Where(ct => ct.MaHoaDon == codeBag).ToList();
            if (!string.IsNullOrEmpty(textFind))
            {
                listProductInBag = listProductInBag.Where(ct => (ct.MaSP??"").Contains(textFind) && (ct.SANPHAM?.TenSP??"").Contains(textFind)).ToList();
            }
            return View(listProductInBag);
        }
        private bool KHACHHANGExists(string id)
        {
          return (_context.KHACHHANG?.Any(e => e.MaKH == id)).GetValueOrDefault();
        }
    }
}
