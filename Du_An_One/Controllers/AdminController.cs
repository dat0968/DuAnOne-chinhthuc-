using ClosedXML.Excel;
using Du_An_One.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
namespace Du_An_One.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly Du_An_OneContext _context;
        public AdminController(Du_An_OneContext context) { 
            _context = context;
        }
        public IActionResult TongQuan(string? MaNV = "")
        {
            #region//Thống kê đơn giản
            ViewBag.CountData = new
            {
                Users = _context.KHACHHANG.Count(),
                ActiveUsers = _context.KHACHHANG.Count(x => x.TinhTrang == "Mở"),
                Staffs = _context.NHANVIEN.Count(),
                Sales = 666,
                FinishOrder = _context.HOADON.Count(hd => hd.TinhTrang == "Đã thanh toán")
            };
            #endregion

            #region//Thống kê theo tình trạng hóa đơn
            ViewBag.CountOrderByStatus = new
            {
                Total = _context.HOADON.Count(),
                WaitPayOrder = _context.HOADON.Count(h => h.TinhTrang == "Chờ thanh toán"),
                WrapOrder = _context.HOADON.Count(h => h.TinhTrang == "Chuẩn bị hàng"),
                DeliverOrder = _context.HOADON.Count(h => h.TinhTrang == "Đang giao hàng"),
                WaitDeliverOrder = _context.HOADON.Count(h => h.TinhTrang == "Chờ giao hàng"),
                PayOrder = _context.HOADON.Count(h => h.TinhTrang == "Đã thanh toán"),
                CancelOrder = _context.HOADON.Count(h => h.TinhTrang == "Đã hủy"),
                BackOrder = _context.HOADON.Count(h => h.TinhTrang == "Hoàn trả/Hoàn tiền")
            };
            #endregion

            #region//Thống kê doanh thu theo ngày
            string?[] listCodeOrderToday = _context.HOADON
                .Where(hd => hd.TinhTrang == "Đã thanh toán" && hd.NgayTao.Date == DateTime.Now.Date)
                .Select(hd => hd.MaHoaDon).ToArray();
            DateTime NgayDauTuan = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
            DateTime NgayCuoiTuan = DateTime.Now.AddDays(7 - (int)DateTime.Now.DayOfWeek);
            var listCodeOrderWeek = _context.HOADON
                .Where(hd => hd.TinhTrang == "Đã thanh toán" && hd.NgayTao.Date >= NgayDauTuan.Date && hd.NgayTao.Date <= NgayCuoiTuan.Date)
                .Select(hd => new { hd.MaHoaDon, hd.NgayTao })
                .OrderBy(hd => hd.NgayTao).ToList();

            var weekSales = _context.CHITIETHOADON
                .AsEnumerable()
                .Join(listCodeOrderWeek, ct => ct.MaHoaDon, lcw => lcw.MaHoaDon, (ct, lcw) => new
                {
                    lcw.MaHoaDon,
                    DoanhThu = ct.SoLuongMua * ct.DonGia,
                    lcw.NgayTao
                })
                .AsEnumerable()
                .GroupBy(g => new { g.NgayTao.Date })
                .Select(g => new
                {
                    NgayTao = g.Key.Date.ToString("yyyy-MM-dd"),
                    DoanhThu = Math.Round(g.Sum(x => x.DoanhThu), 2)
                }).ToList();

            ViewBag.WeekSales = new
            {
                Today = _context.CHITIETHOADON
                    .Where(ct => listCodeOrderToday.Contains(ct.MaHoaDon))
                    .Sum(ct => ct.SoLuongMua * ct.DonGia),
                Week = weekSales
            };

            #endregion

            #region//Thống kê tình trạng các hóa đơn gần đây
            ViewBag.TransactionHistory = _context.HOADON
                .OrderByDescending(x => x.NgayTao)
                .Take(7)
                .SelectMany(hd => _context.CHITIETHOADON
                    .Where(ct => ct.MaHoaDon == hd.MaHoaDon)
                    .Select(ct => new
                    {
                        hd.MaHoaDon,
                        DTime = hd.NgayTao,
                        Amount = ct.SoLuongMua * ct.DonGia,
                        hd.TinhTrang
                    }))
                .GroupBy(item => new { item.MaHoaDon, item.DTime, item.TinhTrang })
                .Select(group => new
                {
                    MaHoaDon = group.Key.MaHoaDon,
                    DTime = group.Key.DTime,
                    TotalAmount = Math.Round(group.Sum(g => g.Amount), 2),
                    TinhTrang = group.Key.TinhTrang
                });
            #endregion

            //Thong Ke

            int thisMonth = DateTime.Now.Month;
            int thisYear = DateTime.Now.Year;
            ViewBag.CustomerStat = new int[]
            {
                _context.KHACHHANG.Count(),

            };

            ViewBag.SalesRevenueStat = new double[]
            {
                _context.CHITIETHOADON
                    .Where(ct => _context.HOADON.Where(h => h.TinhTrang.Contains("Đã thanh toán")).Select(h => h.MaHoaDon).Contains(ct.MaHoaDon)).Sum(ct => (ct.SoLuongMua * ct.DonGia)),
                _context.CHITIETHOADON
                    .Where(ct => _context.HOADON.Where(h => h.NgayTao.Date == DateTime.Today && h.TinhTrang.Contains("Đã thanh toán")).Select(h => h.MaHoaDon).Contains(ct.MaHoaDon)).Sum(ct => (ct.SoLuongMua * ct.DonGia)),
                _context.CHITIETHOADON
                    .Where(ct => _context.HOADON.Where(h => h.NgayTao.Month == thisMonth && h.NgayTao.Year == thisYear && h.TinhTrang.Contains("Đã thanh toán")).Select(h => h.MaHoaDon).Contains(ct.MaHoaDon)).Sum(ct => (ct.SoLuongMua * ct.DonGia)),
                _context.CHITIETHOADON
                    .Where(ct => _context.HOADON.Where(h => h.NgayTao.Year == thisYear && h.TinhTrang.Contains("Đã thanh toán")).Select(h => h.MaHoaDon).Contains(ct.MaHoaDon)).Sum(ct => (ct.SoLuongMua * ct.DonGia)),
            };

            ViewBag.OrderStat = new int[]
            {
                _context.HOADON.Count(h => h.TinhTrang.Contains("Đã thanh toán")),
                _context.HOADON.Count(h => h.NgayTao.Date == DateTime.Today && h.TinhTrang.Contains("Đã thanh toán")),
                _context.HOADON.Count(h => h.NgayTao.Month == thisMonth && h.NgayTao.Year == thisYear && h.TinhTrang.Contains("Đã thanh toán")),
                _context.HOADON.Count(h => h.NgayTao.Year == thisYear && h.TinhTrang.Contains("Đã thanh toán")),
            };
            var HOADON = _context.HOADON
                .Where(h => h.TinhTrang.Contains("Đã thanh toán") && h.MaNV.Contains(MaNV))
                .GroupBy(h => new { h.MaHoaDon, h.MaNV })
                .Select(g => new
                {
                    g.Key.MaHoaDon,
                    g.Key.MaNV,
                    SoDon = g.Count(),
                    SoDonTrongNgay = g.Count(d => d.NgayTao == DateTime.Today),
                    SoDonTrongThang = g.Count(d => d.NgayTao.Month == thisMonth && d.NgayTao.Year == thisYear),
                    SoDonTrongNam = g.Count(d => d.NgayTao.Year == thisYear)
                })
                .ToList(); // Chuyển kết quả thành danh sách để xử lý phía client

            var employeeStats = HOADON
                .Join(_context.CHITIETHOADON, lco => lco.MaHoaDon, ct => ct.MaHoaDon, (lco, ct) => new
                {
                    lco.MaNV,
                    lco.SoDon,
                    lco.SoDonTrongNgay,
                    lco.SoDonTrongThang,
                    lco.SoDonTrongNam,
                    ct.SoLuongMua,
                    ct.DonGia
                })
                .AsEnumerable() // Sử dụng AsEnumerable để tiếp tục xử lý phía client
                .GroupBy(e => new {
                    e.MaNV,
                    e.SoDon,
                    e.SoDonTrongNgay,
                    e.SoDonTrongThang,
                    e.SoDonTrongNam,
                })
                .Join(_context.NHANVIEN.AsEnumerable(), e => e.Key.MaNV, nv => nv.MaNV, (e, nv) => new
                {
                    nv.MaNV,
                    nv.HoTen,
                    NumOrder = e.Count(),
                    e.Key.SoDonTrongNgay,
                    e.Key.SoDonTrongThang,
                    e.Key.SoDonTrongNam,
                    TotalAmount = e.Sum(x => x.SoLuongMua * x.DonGia)
                })
                .ToList();
            ViewBag.EmployeeStat = employeeStats;
            return View();
        }
        public IActionResult XuatDanhSachThongKeNhanVien()
        {
            int thisMonth = DateTime.Now.Month;
            int thisYear = DateTime.Now.Year;
            var HOADON = _context.HOADON
                .Where(h => h.TinhTrang.Contains("Đã thanh toán"))
                .GroupBy(h => new { h.MaHoaDon, h.MaNV })
                .Select(g => new
                {
                    g.Key.MaHoaDon,
                    g.Key.MaNV,
                    SoDon = g.Count(),
                    SoDonTrongNgay = g.Count(d => d.NgayTao == DateTime.Today),
                    SoDonTrongThang = g.Count(d => d.NgayTao.Month == thisMonth && d.NgayTao.Year == thisYear),
                    SoDonTrongNam = g.Count(d => d.NgayTao.Year == thisYear)
                })
                .ToList(); // Chuyển kết quả thành danh sách để xử lý phía client

            var employeeStats = HOADON
                .Join(_context.CHITIETHOADON, lco => lco.MaHoaDon, ct => ct.MaHoaDon, (lco, ct) => new
                {
                    lco.MaNV,
                    lco.SoDon,
                    lco.SoDonTrongNgay,
                    lco.SoDonTrongThang,
                    lco.SoDonTrongNam,
                    ct.SoLuongMua,
                    ct.DonGia
                })
                .AsEnumerable() // Sử dụng AsEnumerable để tiếp tục xử lý phía client
                .GroupBy(e => new {
                    e.MaNV,
                    e.SoDon,
                    e.SoDonTrongNgay,
                    e.SoDonTrongThang,
                    e.SoDonTrongNam,
                })
                .Join(_context.NHANVIEN.AsEnumerable(), e => e.Key.MaNV, nv => nv.MaNV, (e, nv) => new
                {
                    nv.MaNV,
                    nv.HoTen,
                    NumOrder = e.Count(),
                    e.Key.SoDonTrongNgay,
                    e.Key.SoDonTrongThang,
                    e.Key.SoDonTrongNam,
                    TotalAmount = e.Sum(x => x.SoLuongMua * x.DonGia)
                })
                .ToList();

            if (employeeStats == null || !employeeStats.Any())
            {
                // Nếu TempData không có dữ liệu, có thể thực hiện truy vấn lại hoặc thông báo lỗi
                return NotFound("Không có dữ liệu để xuất");
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DanhSachThongKeNhanVien");
                var currentRow = 1;

                // Header
                worksheet.Cell(currentRow, 1).Value = "Mã nhân viên";
                worksheet.Cell(currentRow, 2).Value = "Tên nhân viên";
                worksheet.Cell(currentRow, 3).Value = "Số lượng đơn";
                worksheet.Cell(currentRow, 4).Value = "Số đơn trong ngày";
                worksheet.Cell(currentRow, 5).Value = "Số đơn trong tháng";
                worksheet.Cell(currentRow, 6).Value = "Số đơn trong năm";
                worksheet.Cell(currentRow, 7).Value = "Tổng tiền bán được";

                // Content
                foreach (var emp in employeeStats)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = emp.MaNV;
                    worksheet.Cell(currentRow, 2).Value = emp.HoTen;
                    worksheet.Cell(currentRow, 3).Value = emp.NumOrder;
                    worksheet.Cell(currentRow, 4).Value = emp.SoDonTrongNgay;
                    worksheet.Cell(currentRow, 5).Value = emp.SoDonTrongThang;
                    worksheet.Cell(currentRow, 6).Value = emp.SoDonTrongNam;
                    worksheet.Cell(currentRow, 7).Value = emp.TotalAmount;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachThongKeNhanVien.xlsx");
                }
            }
        }
        public IActionResult DanhSachHoaDonCuaNhanVien(string MaNV)
        {
            var listCodeCheckOfStaff = _context.HOADON
                .Where(x => x.MaNV == MaNV && x.TinhTrang.Contains("Đã thanh toán"))
                .Select(x => new { x.MaHoaDon, x.NgayTao })
                .ToList();
            ViewBag.DuLieuHomNay = listCodeCheckOfStaff
                .Where(x => x.NgayTao.Date == DateTime.Now.Date)
                .SelectMany(hd => _context.CHITIETHOADON
                    .Where(ct => ct.MaHoaDon == hd.MaHoaDon)
                    .Select(ct => new
                    {
                        hd.MaHoaDon,
                        Tien = ct.SoLuongMua * ct.DonGia,
                        Ngay = hd.NgayTao
                    }))
                .GroupBy(g => g.Ngay.Date)
                .Select(group => new
                {
                    SoHoaDon = group.GroupBy(g => g.MaHoaDon).Count(),
                    TongTien = Math.Round(group.Sum(g => g.Tien), 2)
                })
                .ToList();//Note
            /*.Join(_context.CHITIETHOADON, h => h.MaHoaDon, ct => ct.MaHoaDon, (h, ct) => new { SoHoaDon = h.MaHoaDon.Count(), TongTien = (ct.SoLuongMua * ct.DonGia) })
            .ToList();*/
            ViewBag.DuLieuTruocDo = listCodeCheckOfStaff.Where(x => x.NgayTao.Date != DateTime.Now.Date).Join(_context.CHITIETHOADON, h => h.MaHoaDon, ct => ct.MaHoaDon, (h, ct) => new { h, ct }).GroupBy(g => g.h.NgayTao.Date).Select(x => new { Ngay = x.Key, SoHoaDon = x.Count(), TongTien = x.Sum(y => y.ct.SoLuongMua * y.ct.DonGia) }).OrderByDescending(x => x.Ngay).ToList();

            return View(_context.NHANVIEN.First(x => x.MaNV == MaNV));
        }
    }
}
