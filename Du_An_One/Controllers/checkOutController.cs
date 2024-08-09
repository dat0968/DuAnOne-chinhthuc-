using DocumentFormat.OpenXml.Drawing.Charts;
using Du_An_One.Data;
using Du_An_One.Models;
using Humanizer.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Du_An_One.Controllers
{
    public class checkOutController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly Du_An_OneContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public checkOutController(Du_An_OneContext context, IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }


        public IActionResult Index()
        {
            // Logic để hiển thị trang checkout
            return View();
        }





        // POST: Checkout
        [HttpGet]
        public IActionResult Checkout(string idUser, string MaHoaDon)
        {
            var model = new CheckoutViewModel
            {
                MaHoaDon = MaHoaDon,
                MaKH = idUser
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Checkout(CheckoutViewModel model)
        {
            var urls = $"{"https://localhost:7079/KHACHHANGs/ListProductsInBagOfCustomer?idUser=" + model.MaKH}";
            if (ModelState.IsValid)
            {
                if (model.HTTT == "Bank")
                {

                    var htttHoaDon = _context.HOADON.FirstOrDefault(x=>x.MaHoaDon== model.MaHoaDon);
                    htttHoaDon.DiaChiNhanHang = model.DiaChiNhanHang;
                    _context.SaveChanges();
                    return RedirectToAction("GetVnPayPaymentUrl", new { orderId = model.MaHoaDon });
                }
                else if (model.HTTT == "COD")
                {
                    var existingOrder = _context.HOADON
                                       .FirstOrDefault(hd => hd.MaKH == model.MaKH && hd.TinhTrang == "Giỏ hàng");

                    if (existingOrder != null)
                    {
                        existingOrder.DiaChiNhanHang = model.DiaChiNhanHang;
                        existingOrder.HTTT = model.HTTT;
                        existingOrder.NgayTao = DateTime.Now;
                        existingOrder.TinhTrang = "Đang xử lý";
                    }
                    else
                    {
                        var hoaDon = new HOADON
                        {
                            MaHoaDon = Guid.NewGuid().ToString("N").Substring(0, 5),
                            DiaChiNhanHang = model.DiaChiNhanHang,
                            NgayTao = DateTime.Now,
                            HTTT = model.HTTT,
                            TinhTrang = "Chờ thanh toán",
                            MaKH = model.MaKH,
                        };

                        _context.HOADON.Add(hoaDon);
                    }
                    _context.SaveChanges();
                }



            }
            return Redirect($"{urls}");
        }
        public IActionResult OrderConfirmation(string orderId)
        {
            // Tìm đơn hàng theo mã đơn hàng
            var order = _context.HOADON.FirstOrDefault(hd => hd.MaHoaDon == orderId);

            if (order == null)
            {
                return NotFound();
            }

            order.TinhTrang = "Đã xác nhận (COD)";
            _context.SaveChanges();

            return View(order);
        }


        private string CreateVnPayPaymentUrl(string orderInfo)
        {
            var vnp_Url = _configuration["Vnpay:Url"];
            var vnp_Returnurl = _configuration["Vnpay:ReturnUrl"];
            var vnp_TmnCode = _configuration["Vnpay:TmnCode"];
            var vnp_HashSecret = _configuration["Vnpay:HashSecret"];
            var HoaDon = _context.CHITIETHOADON.FirstOrDefault(x => x.MaHoaDon == orderInfo);
            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (Convert.ToInt32(HoaDon.DonGia) * 100).ToString());
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            // Kiểm tra xem HttpContextAccessor có tồn tại không trước khi sử dụng
            if (_contextAccessor?.HttpContext?.Connection?.RemoteIpAddress != null)
            {
                vnpay.AddRequestData("vnp_IpAddr", _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
            }
            else
            {
                vnpay.AddRequestData("vnp_IpAddr", "127.0.0.1");
            }
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", orderInfo);
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", orderInfo);

            return vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
        }


        public IActionResult GetVnPayPaymentUrl(string orderId)
        {
            var paymentUrl = CreateVnPayPaymentUrl(orderId);
            return Redirect($"{paymentUrl}");
        }
        public async Task<IActionResult> VnPayReturn()
        {
            var vnp_HashSecret = _configuration["Vnpay:HashSecret"];
            var vnpayData = HttpContext.Request.Query;
            VnPayLibrary vnpay = new VnPayLibrary();

            foreach (var (key, value) in vnpayData)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value);
                }
            }

            string inputHash = vnpayData["vnp_SecureHash"];
            if (vnpay.ValidateSignature(inputHash, vnp_HashSecret))
            {
                string txnRef = vnpayData["vnp_TxnRef"];
                string responseCode = vnpayData["vnp_ResponseCode"];

                var donHang = _context.HOADON.FirstOrDefault(x => x.MaHoaDon == txnRef);
                if (donHang == null)
                {
                    return NotFound();
                }

                if (responseCode == "00")
                {
                    donHang.HTTT = "Bank";
                    donHang.TinhTrang = "Đã thanh toán";
                    await _context.SaveChangesAsync();

                    // Chuyển hướng tới trang Result với kết quả thành công
                    return RedirectToAction("Result", new { success = true });
                }
                else
                {
                    donHang.HTTT = "Bank";
                    donHang.TinhTrang = "Thanh toán thất bại";
                    await _context.SaveChangesAsync();

                    // Chuyển hướng tới trang Result với kết quả thất bại
                    return RedirectToAction("Result", new { success = false });
                }
            }
            return BadRequest("Invalid signature");
        }

        public IActionResult Result(bool success)
        {
            // Trả về view hiển thị kết quả thanh toán
            ViewBag.Success = success;
            return View();
        }



    }
}

public class VnPayLibrary
{
    private readonly SortedList<string, string> _requestData = new SortedList<string, string>();
    private readonly SortedList<string, string> _responseData = new SortedList<string, string>();

    public void AddRequestData(string key, string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            _requestData.Add(key, value);
        }
    }

    public void AddResponseData(string key, string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            _responseData.Add(key, value);
        }
    }

    public string CreateRequestUrl(string baseUrl, string hashSecret)
    {
        var data = new StringBuilder();
        foreach (var kv in _requestData)
        {
            if (data.Length > 0)
            {
                data.Append("&");
            }
            data.Append(kv.Key + "=" + Uri.EscapeDataString(kv.Value));
        }

        var rawData = data.ToString();
        var signData = HmacSHA512(hashSecret, rawData);
        var paymentUrl = $"{baseUrl}?{rawData}&vnp_SecureHash={signData}";
        return paymentUrl;
    }

    public bool ValidateSignature(string inputHash, string secretKey)
    {
        var data = new StringBuilder();
        foreach (var kv in _responseData)
        {
            if (kv.Key != "vnp_SecureHash")
            {
                if (data.Length > 0)
                {
                    data.Append("&");
                }
                data.Append(kv.Key + "=" + Uri.EscapeDataString(kv.Value));
            }
        }

        var rawData = data.ToString();
        var myChecksum = HmacSHA512(secretKey, rawData);
        return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
    }

    private static string HmacSHA512(string key, string inputData)
    {
        var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(inputData));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}
