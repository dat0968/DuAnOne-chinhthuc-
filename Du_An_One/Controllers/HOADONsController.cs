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
using Microsoft.AspNetCore.Authorization;
namespace Du_An_One.Controllers
{
    [Authorize]
    public class HOADONsController : Controller
    {
        private readonly Du_An_OneContext _context;

        public HOADONsController(Du_An_OneContext context)
        {
            _context = context;
        }

        // GET: HOADONs
        public async Task<IActionResult> Index(int page = 1)
        {
            page = page < 1 ? 1 : page;
            int pagesize = 10;
            var hoadon = _context.HOADON.ToPagedList(page, pagesize);
            return View(hoadon);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string keywords, int page = 1)
        {
            if (keywords == null)
            {
                return RedirectToAction("Index", "HOADONs");
            }
            page = page < 1 ? 1 : page;
            int pagesize = 10;
            var query = _context.HOADON.AsQueryable();
            query = query.Where(p => p.MaHoaDon.Contains(keywords) || p.MaNV.Contains(keywords) || p.MaKH.Contains(keywords));
            var ketquatimkiem = query.ToPagedList(page, pagesize);
            if (!ketquatimkiem.Any())
            {
                ketquatimkiem = _context.HOADON.ToPagedList(page, pagesize);
                TempData["ErrorMessage"] = " Không tìm thấy hóa đơn cần tìm";
            }
            return View(ketquatimkiem);
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
                bool exists = await _context.HOADON.AnyAsync(p => p.MaHoaDon == hOADON.MaHoaDon);


                if (exists)
                {
                    ModelState.AddModelError("MaHoaDon", "Mã này đã tồn tại. Vui lòng nhập lại");
                    return View(hOADON);
                }
                try
                {
                    _context.Add(hOADON);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Thêm hoán đơn thành công";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm nhân viên. Vui lòng thử lại.";
                }
            }
            //ViewData["MaHoaDon"] = new SelectList(_context.HOADON, "MaHoaDon", cHITIETHOADON.MaHoaDon);
            //ViewData["MaSP"] = new SelectList(_context.SANPHAM, "MaSP", cHITIETHOADON.MaSP);
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
            //ViewData["MaNV"] = new SelectList(_context.NHANVIEN, "MaNV", "MaNV", hOADON.MaNV);
            //ViewData["MaKH"] = new SelectList(_context.KHACHHANG, "MaKH", "MaKH", hOADON.MaKH);

            return View(hOADON);
        }

        // POST: HOADONs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaHoaDon, TinhTrang")] HOADON hOADON)
        {
            if (id != hOADON.MaHoaDon)
            {
                return NotFound();
            }

            // Kiểm tra sự tồn tại của nhân viên và khách hàng
            if (hOADON.MaNV != null && !await _context.NHANVIEN.AnyAsync(e => e.MaNV == hOADON.MaNV))
            {
                ModelState.AddModelError("MaNV", "Mã nhân viên không tồn tại.");
            }

            if (hOADON.MaKH != null && !await _context.KHACHHANG.AnyAsync(c => c.MaKH == hOADON.MaKH))
            {
                ModelState.AddModelError("MaKH", "Mã khách hàng không tồn tại.");
            }

            if (hOADON.TinhTrang!=null)
            {
                try
                {
                    var existingHoaDon = await _context.HOADON.FindAsync(id);
                    if (existingHoaDon == null)
                    {
                        return NotFound();
                    }

                    // Cập nhật thông tin hóa đơn, chỉ cập nhật thuộc tính TinhTrang
                    existingHoaDon.TinhTrang = hOADON.TinhTrang;
                    // Nếu không cần cập nhật các thuộc tính khác, không sử dụng SetValues
                    _context.HOADON.Update(existingHoaDon); // Thay vì _context.Entry(existingHoaDon).CurrentValues.SetValues(hOADON);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Sửa thông tin hóa đơn thành công";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi sửa thông tin hóa đơn. Vui lòng thử lại.";
                }
            }

            // Trả về view với thông tin hiện tại nếu có lỗi
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
                return Problem("Entity set 'Du_An_OneContext.HOADONN'  is null.");
            }
            var hOADON = await _context.HOADON.FindAsync(id);
            try
            {
                if (hOADON != null)
                {
                    _context.HOADON.Remove(hOADON);
                }
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa hóa đơn thành công!";
                return RedirectToAction("Index"); // Hoặc trang bạn muốn chuyển hướng đến
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa dữ liệu. Vui lòng thử lại.";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult CancelOrder(string orderId)
        {
            var order = _context.HOADON.Find(orderId);
            if (order != null)
            {
                _context.HOADON.Remove(order);
                _context.SaveChanges();
                return RedirectToAction("ListOrdersOfCustomer","KHACHHANGs"); // Hoặc trang bạn muốn chuyển đến sau khi hủy đơn
            }
            return NotFound();
        }


        private bool HOADONExists(string id)
        {
          return (_context.HOADON?.Any(e => e.MaHoaDon == id)).GetValueOrDefault();
        }
    }
}
