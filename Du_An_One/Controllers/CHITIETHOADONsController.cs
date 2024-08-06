using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Du_An_One.Data;
using Du_An_One.Models;
using Newtonsoft.Json;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

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

        [HttpPost]
        public async Task<IActionResult> CustomerDelete(int id)
        {
            if (_context.CHITIETHOADON == null)
            {
                return Json(new { success = false, message = "Entity set 'Du_An_OneContext.CHITIETHOADON' is null." });
            }

            var cHITIETHOADON = await _context.CHITIETHOADON.FindAsync(id);
            if (cHITIETHOADON != null)
            {
                _context.CHITIETHOADON.Remove(cHITIETHOADON);
                await _context.SaveChangesAsync();
                return Json(new
                {
                    success = true,
                    alert = new { type = "success", title = "Thông báo", message = "Đã xóa thành công" }
                });
            }

            return Json(new
            {
                success = false,
                alert = new { type = "info", title = "Thông báo", message = "Không tìm thấy sản phẩm" }
            });
        }
        public async Task<IActionResult> CustomerAdd(string MaSP)
        {
            if(!User.Identity.IsAuthenticated)
            {
                // Nếu người dùng chưa đăng nhập, trả về thông báo cảnh báo
                return Json(new
                {
                    success = false,
                    alert = new { type = "warning", title = "Thông báo", message = "Vui lòng đăng nhập để có thể mua hàng." }
                });
            }

            DateTime today = DateTime.Now;
            var MaNguoiDung = User.FindFirst(ClaimTypes.Surname)?.Value;
            var Product = _context.SANPHAM.FirstOrDefault(sp => sp.MaSP == MaSP);

            // Lấy danh sách khuyến mãi còn hiệu lực
            var DanhSachMaKhuyenMai = _context.KHUYENMAI
                .Where(km => km.ThoiGianStart <= today && km.ThoiGianEnd >= today)
                .ToList();

            // Kiểm tra xem giỏ hàng của khách hàng có tồn tại không
            var CustomerBag = _context.HOADON
                .FirstOrDefault(hd => hd.MaKH == MaNguoiDung && hd.TinhTrang == "Chờ thanh toán");

            if (CustomerBag != null)
            {
                // Kiểm tra xem sản phẩm đã có trong chi tiết hóa đơn không
                var existingDetail = _context.CHITIETHOADON
                    .FirstOrDefault(cd => cd.MaHoaDon == CustomerBag.MaHoaDon && cd.MaSP == MaSP);

                if (existingDetail != null)
                {
                    // Nếu sản phẩm đã có, tăng số lượng lên 1
                    existingDetail.SoLuongMua++;
                }
                else
                {
                    // Nếu sản phẩm chưa có, thêm vào chi tiết hóa đơn
                    var phanTramKhuyenMai = DanhSachMaKhuyenMai
                        .FirstOrDefault(lkm => lkm.MaKhuyenMai == Product.MaKhuyenMai)?.PhanTramKhuyenMai ?? 0;

                    _context.CHITIETHOADON.Add(new CHITIETHOADON
                    {
                        MaHoaDon = CustomerBag.MaHoaDon,
                        DonGia = Product.DonGiaBan * (1 - phanTramKhuyenMai / 100),
                        MaSP = MaSP,
                        SoLuongMua = 1
                    });
                }
            }
            else
            {
                // Nếu chưa có giỏ hàng, tạo mới giỏ hàng và thêm sản phẩm vào
                Random rd = new Random();
                const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

                var _newCheck = new HOADON
                {
                    MaHoaDon = String.Join("", Enumerable.Range(0, 5).Select(_ => chars[rd.Next(chars.Length)])),
                    DiaChiNhanHang = "",
                    NgayTao = today,
                    HTTT = "Tiền mặt",
                    MaKH = MaNguoiDung,
                    TinhTrang = "Chờ thanh toán" // Cần thêm tình trạng cho hóa đơn mới
                };

                _context.HOADON.Add(_newCheck);
                await _context.SaveChangesAsync(); // Lưu hóa đơn mới vào cơ sở dữ liệu

                var phanTramKhuyenMai = DanhSachMaKhuyenMai
                    .FirstOrDefault(lkm => lkm.MaKhuyenMai == Product.MaKhuyenMai)?.PhanTramKhuyenMai ?? 0;

                _context.CHITIETHOADON.Add(new CHITIETHOADON
                {
                    MaHoaDon = _newCheck.MaHoaDon,
                    DonGia = Product.DonGiaBan * (1 - phanTramKhuyenMai / 100),
                    SoLuongMua = 1
                });
            }

            // Lưu tất cả thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // Trả về thông báo thành công
            return Json(new
            {
                success = true,
                alert = new { type = "success", title = "Thông báo", message = "Đã thêm sản phẩm vào giỏ hàng thành công." }
            });
        }


        private bool CHITIETHOADONExists(int id)
        {
            return (_context.CHITIETHOADON?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
