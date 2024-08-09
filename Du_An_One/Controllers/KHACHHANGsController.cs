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
using ClosedXML.Excel;
using Irony.Parsing;
using X.PagedList;
using X.PagedList.Extensions;
using DocumentFormat.OpenXml.Office2010.Excel;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using Table = iText.Layout.Element.Table;
using iText.Layout.Properties;
using static System.Net.WebRequestMethods;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using iText.IO.Font;
using iText.Kernel.Font;
namespace Du_An_One.Controllers
{
    [Authorize]
    public class KHACHHANGsController : Controller
    {
        private readonly Du_An_OneContext _context;

        public KHACHHANGsController(Du_An_OneContext context)
        {
            _context = context;
        }

        // GET: KHACHHANGs
        public async Task<IActionResult> Index(int page = 1)
        {
            page = page < 1 ? 1 : page;
            int pagesize = 10;
            var khachhang = _context.KHACHHANG.ToPagedList(page, pagesize);
            return View(khachhang);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string keywords, int page = 1)
        {
            if (keywords == null)
            {
                return RedirectToAction("Index", "KHACHHANGs");
            }
            page = page < 1 ? 1 : page;
            int pagesize = 10;
            var query = _context.KHACHHANG.AsQueryable();
            query = query
            .Where(p => p.MaKH.Contains(keywords) || p.TenTaiKhoan.Contains(keywords));
            var ketquatimkiem = query.ToPagedList(page, pagesize);

            if (!ketquatimkiem.Any())
            {
                // Nếu không tìm thấy kết quả, trả về toàn bộ danh sách khách hàng
                ketquatimkiem = _context.KHACHHANG.ToPagedList(page, pagesize);
                TempData["ErrorMessage"] = "Không tìm thấy khách hàng với từ khóa đã nhập.";
            }

            return View(ketquatimkiem);
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
        public async Task<IActionResult> Create(string id, [Bind("MaKH,HoTen,NgaySinh,NoiSinh,DiaChi,CCCD,SDT,Email,TenTaiKhoan,MatKhau,TinhTrang")] KHACHHANG kHACHHANG)
        {

            if (ModelState.IsValid)
            {
                bool exists = await _context.KHACHHANG.AnyAsync(p => p.MaKH == kHACHHANG.MaKH);
                bool checktaikhoan = await _context.KHACHHANG.AnyAsync(p => p.TenTaiKhoan == kHACHHANG.TenTaiKhoan);
                if (checktaikhoan)
                {
                    ModelState.AddModelError("TenTaiKhoan", "Tên tài khoản này đã tồn tại. Vui lòng nhập lại");
                    return View(kHACHHANG);
                }
                if (exists)
                {
                    ModelState.AddModelError("MaKH", "Mã này đã tồn tại. Vui lòng nhập lại");
                    return View(kHACHHANG);
                }
                try
                {
                    _context.Add(kHACHHANG);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Thêm khách hàng thành công";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi thêm sản phẩm. Vui lòng thử lại.";
                }
            }
            return View();
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
            bool checktaikhoan = await _context.KHACHHANG.AnyAsync(p => p.TenTaiKhoan == kHACHHANG.TenTaiKhoan && p.MaKH != id);
            if (checktaikhoan)
            {
                ModelState.AddModelError("TenTaiKhoan", "Tên tài khoản này đã tồn tại. Vui lòng nhập lại");
                return View(kHACHHANG);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kHACHHANG);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Sửa thông tin khách hàng thành công";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi sửa thông tin khách hàng. Vui lòng thử lại.";
                }
            }
            return View();
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
            try
            {
                if (kHACHHANG != null)
                {
                    _context.KHACHHANG.Remove(kHACHHANG);
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa khách hàng thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa dữ liệu. Vui lòng thử lại.";
            }
            return View();
        }
        public IActionResult ExportExcelKhachHang()
        {
            var khachhang = _context.KHACHHANG.ToList();
            using (var workbook = new XLWorkbook())
            {
                var hanghientai = 1;
                var worksheet = workbook.Worksheets.Add("DanhSachKhachHang");
                worksheet.Cell(hanghientai, 1).Value = "Mã Khách hàng";
                worksheet.Cell(hanghientai, 2).Value = "Họ và tên";
                worksheet.Cell(hanghientai, 3).Value = "Ngày sinh";
                worksheet.Cell(hanghientai, 4).Value = "Nơi sinh";
                worksheet.Cell(hanghientai, 5).Value = "Địa chỉ";
                worksheet.Cell(hanghientai, 6).Value = "CCCD";
                worksheet.Cell(hanghientai, 7).Value = "SDT";
                worksheet.Cell(hanghientai, 8).Value = "Email";
                worksheet.Cell(hanghientai, 9).Value = "Tên tài khoản";
                worksheet.Cell(hanghientai, 10).Value = "Mật khẩu";
                worksheet.Cell(hanghientai, 11).Value = "Tình trạng";
                foreach (var customer in khachhang)
                {
                    hanghientai++;
                    worksheet.Cell(hanghientai, 1).Value = customer.MaKH;
                    worksheet.Cell(hanghientai, 2).Value = customer.HoTen;
                    worksheet.Cell(hanghientai, 3).Value = customer.NgaySinh;
                    worksheet.Cell(hanghientai, 4).Value = customer.NoiSinh;
                    worksheet.Cell(hanghientai, 5).Value = customer.DiaChi;
                    worksheet.Cell(hanghientai, 6).Value = customer.CCCD;
                    worksheet.Cell(hanghientai, 7).Value = customer.SDT;
                    worksheet.Cell(hanghientai, 8).Value = customer.Email;
                    worksheet.Cell(hanghientai, 9).Value = customer.TenTaiKhoan;
                    worksheet.Cell(hanghientai, 10).Value = customer.MatKhau;
                    worksheet.Cell(hanghientai, 11).Value = customer.TinhTrang;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachKhachHang.xlsx");
                }
            }
        }

        // GET: KHACHHANGs/InfoCustomer/5
        [Authorize]
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
            //return RedirectToAction("Index", "KHACHHANGs");
        }

        // POST: KHACHHANGs/InfoCustomer/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> InfoCustomer(KHACHHANG kHACHHANG)
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
        [Authorize]
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

            var listOrder = _context.HOADON
                .Where(x => x.MaKH == idUser && x.TinhTrang != "Giỏ hàng")
                .Join(
                    _context.CHITIETHOADON,
                    hd => hd.MaHoaDon,
                    ct => ct.MaHoaDon,
                    (hd, ct) => new
                    {
                        HOADON = hd,
                        CHITIETHOADON = ct
                    })
                .AsQueryable();

            // Filter by status if specified
            if (!String.IsNullOrEmpty(statusOrder))
            {
                listOrder = listOrder.Where(x => x.HOADON.TinhTrang == statusOrder);
            }

            // If findOrder is not null or empty, filter orders based on the findOrder parameter
            if (!String.IsNullOrEmpty(findOrder))
            {
                listOrder = listOrder.Where(x =>
                    x.HOADON.MaHoaDon.Contains(findOrder) ||
                    x.HOADON.TinhTrang.Contains(findOrder) ||
                    x.HOADON.HTTT.Contains(findOrder));
            }

            ViewBag.StatusOrder = statusOrder;

            var result = listOrder
                .AsEnumerable()
                .GroupBy(x => x.HOADON.MaHoaDon)
                .Select(group => new HOADON
                {
                    MaHoaDon = group.Key,
                    NgayTao = group.FirstOrDefault().HOADON.NgayTao,
                    TinhTrang = group.FirstOrDefault().HOADON.TinhTrang,
                    HTTT = group.FirstOrDefault().HOADON.HTTT,
                    CHITIETHOADONs = group.Select(x => x.CHITIETHOADON).ToList()
                })
                .OrderBy(x => x.MaHoaDon)
                .ToList()
                .ToPagedList(pageNumber, pageSize);

            ViewBag.IdUser = idUser;
            // Return the filtered list of orders to the view
            return View(result);
        }

        [Route("export-invoice/{orderId}")]
        public async Task<IActionResult> PrintCheckOrder(string orderId)
        {
            var order = _context.HOADON
                .Select(hd => new HOADON
                {
                    MaHoaDon = hd.MaHoaDon,
                    MaKH = hd.MaKH,
                    DiaChiNhanHang = hd.DiaChiNhanHang,
                    NgayTao = hd.NgayTao,
                    HTTT = hd.HTTT,
                    TinhTrang = hd.TinhTrang,
                    CHITIETHOADONs = _context.CHITIETHOADON.Where(x => x.MaHoaDon == orderId)
                        .Select(ct => new CHITIETHOADON
                        {
                            MaHoaDon = ct.MaHoaDon,
                            MaSP = ct.MaSP,
                            SoLuongMua = ct.SoLuongMua,
                            DonGia = ct.DonGia,
                            SANPHAM = _context.SANPHAM.FirstOrDefault(sp => sp.MaSP == ct.MaSP)
                        }).ToList(),
                    KHACHHANG = _context.KHACHHANG.FirstOrDefault(kh => kh.MaKH == hd.MaKH)
                })
                .FirstOrDefault(o => o.MaHoaDon == orderId);

            if (order == null)
            {
                return NotFound();
            }

            using (var stream = new MemoryStream())
            {
                var writer = new PdfWriter(stream);
                var pdf = new PdfDocument(writer);
                var document = new iText.Layout.Document(pdf);

                var font = PdfFontFactory.CreateFont(@"C:\Windows\Fonts\times.ttf", PdfEncodings.IDENTITY_H);
                document.SetFont(font);

                // Add header
                document.Add(new Paragraph("Hóa đơn")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFontSize(20));

                // Add order details
                document.Add(new Paragraph($"Mã đơn: {order.MaHoaDon}"));
                document.Add(new Paragraph($"Khách hàng: {order.KHACHHANG.HoTen}"));
                document.Add(new Paragraph($"Địa chỉ: {order.DiaChiNhanHang}"));
                document.Add(new Paragraph($"Ngày tạo đơn: {order.NgayTao.ToString("dd/MM/yyyy")}"));

                // Add a line separator
                document.Add(new LineSeparator(new iText.Kernel.Pdf.Canvas.Draw.SolidLine()));

                // Add product table
                var table = new Table(new float[] { 1, 3, 1, 1, 1 })
                    .UseAllAvailableWidth();

                table.AddHeaderCell(new Cell().Add(new Paragraph("Mã ID").SetFont(font)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Tên sản phẩm").SetFont(font)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Số lượng").SetFont(font)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Đơn vị giá").SetFont(font)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Tổng giá").SetFont(font)));

                foreach (var item in order.CHITIETHOADONs)
                {
                    table.AddCell(new Cell().Add(new Paragraph(item.MaSP.ToString()).SetFont(font)));
                    table.AddCell(new Cell().Add(new Paragraph(item.SANPHAM.TenSP).SetFont(font)));
                    table.AddCell(new Cell().Add(new Paragraph(item.SoLuongMua.ToString()).SetFont(font)));
                    table.AddCell(new Cell().Add(new Paragraph(item.DonGia.ToString("C")).SetFont(font)));
                    table.AddCell(new Cell().Add(new Paragraph((item.SoLuongMua * item.DonGia).ToString("C")).SetFont(font)));
                }

                document.Add(table);

                // Add footer
                document.Add(new Paragraph("Cảm ơn vì sự hỗ trợ của bạn!")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(12)
                .SetFont(font));

                document.Close();
                var bytes = stream.ToArray();
                return File(bytes, "application/pdf", $"Invoice_{order.MaHoaDon}.pdf");
            }
        }

        // GET: KHACHHANGs/ListProductsInBag/5
        [Authorize]
        public async Task<IActionResult> ListProductsInBagOfCustomer(string idUser, string? textFind)
        {
            ViewBag.IdUser = idUser;
            string codeBag = _context.HOADON.FirstOrDefault(hd => hd.MaKH == idUser && hd.TinhTrang == "Giỏ hàng")?.MaHoaDon ?? "";
            if (string.IsNullOrEmpty(codeBag))
            {
                return View(new List<CHITIETHOADON>());
            }
            var listProductInBag = _context.CHITIETHOADON
                .Where(ct => ct.MaHoaDon == codeBag)
                .Join(_context.SANPHAM, ct => ct.MaSP, sp => sp.MaSP, (ct, sp) => new
                {
                    CHITIETHOADON = ct,
                    SANPHAM = sp
                })
                .ToList();
            if (!string.IsNullOrEmpty(textFind))
            {
                listProductInBag = listProductInBag
                    .Where(ct => (ct.CHITIETHOADON.MaSP ?? "").Contains(textFind) || (ct.SANPHAM.TenSP ?? "").Contains(textFind))
                    .ToList();
            }

            var result = listProductInBag.Select(item => new CHITIETHOADON
            {
                ID = item.CHITIETHOADON.ID,
                MaHoaDon = item.CHITIETHOADON.MaHoaDon,
                MaSP = item.CHITIETHOADON.MaSP,
                SoLuongMua = item.CHITIETHOADON.SoLuongMua,
                DonGia = item.CHITIETHOADON.DonGia,
                SANPHAM = new SANPHAM
                {
                    MaSP = item.SANPHAM.MaSP,
                    TenSP = item.SANPHAM.TenSP,
                    DonGiaBan = item.SANPHAM.DonGiaBan,
                    SoLuongBan = item.SANPHAM.SoLuongBan,
                    KichCo = item.SANPHAM.KichCo,
                    MoTa = item.SANPHAM.MoTa,
                    DanhMucHang = item.SANPHAM.DanhMucHang,
                    KHUYENMAI = _context.KHUYENMAI.FirstOrDefault(km => km.MaKhuyenMai == item.SANPHAM.MaKhuyenMai)
                }
            }).ToList();

            return View(result);
        }

        [Authorize]
        public async Task<IActionResult> UpdateNumberProductInCart(int? MaHoaDon, int valueChange)
        {
            if (MaHoaDon == null || _context.CHITIETHOADON == null)
            {
                return NotFound(new { message = "Không tìm thấy hóa đơn." });
            }

            var ChiTietHoaDon = await _context.CHITIETHOADON
                .FirstOrDefaultAsync(m => m.ID == MaHoaDon);

            if (ChiTietHoaDon == null)
            {
                return NotFound(new { message = "Chi tiết hóa đơn không tồn tại." });
            }

            // Kiểm tra số lượng mới không âm
            if (valueChange < 0)
            {
                return BadRequest(new { message = "Số lượng không thể nhỏ hơn 0." });
            }

            ChiTietHoaDon.SoLuongMua = valueChange;
            await _context.SaveChangesAsync();

            return Json(ChiTietHoaDon);
        }


        public class UpdateQuantityRequest
        {
            public int Id { get; set; }
            public int Quantity { get; set; }
        }
        private bool KHACHHANGExists(string id)
        {
            return (_context.KHACHHANG?.Any(e => e.MaKH == id)).GetValueOrDefault();
        }
    }
}
