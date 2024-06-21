using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_ver1.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Security.Cryptography;

namespace project_ver1.Controllers
{
    public class RoomController : Controller
    {
        private readonly HomeDbContext _context;

        public RoomController(HomeDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(List<Rooms> availableRooms = null)
        {
            SetUserViewBag();
            /////////////////////////<------新增測試
            var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
            //需填入你的網址
            var website = $"https://localhost:44325/";
            var order = new Dictionary<string, string>
    {
        //綠界需要的參數
        { "MerchantTradeNo",  orderId},
        { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},
        { "TotalAmount",  "100"},
        { "TradeDesc",  "無"},
        { "ItemName",  "測試商品"},
        { "ExpireDate",  "3"},
        { "CustomField1",  ""},
        { "CustomField2",  ""},
        { "CustomField3",  ""},
        { "CustomField4",  ""},
        { "ReturnURL",  $"{website}/api/Ecpay/AddPayInfo"},
        { "OrderResultURL", $"{website}/Home/PayInfo/{orderId}"},
        { "PaymentInfoURL",  $"{website}/api/Ecpay/AddAccountInfo"},
        { "ClientRedirectURL",  $"{website}/Home/AccountInfo/{orderId}"},
        { "MerchantID",  "2000132"},
        { "IgnorePayment",  "GooglePay#WebATM#CVS#BARCODE"},
        { "PaymentType",  "aio"},
        { "ChoosePayment",  "ALL"},
        { "EncryptType",  "1"},
         };
            //檢查碼
            order["CheckMacValue"] = GetCheckMacValue(order);
            
            return View(availableRooms ?? new List<Rooms>());
        }
        private string GetCheckMacValue(Dictionary<string, string> order)
        {
            var param = order.Keys.OrderBy(x => x).Select(key => key + "=" + order[key]).ToList();
            var checkValue = string.Join("&", param);
            //測試用的 HashKey
            var hashKey = "5294y06JbISpM5x9";
            //測試用的 HashIV
            var HashIV = "v77hoKGq4kWxNNIS";
            checkValue = $"HashKey={hashKey}" + "&" + checkValue + $"&HashIV={HashIV}";
            checkValue = HttpUtility.UrlEncode(checkValue).ToLower();
            checkValue = GetSHA256(checkValue);
            return checkValue.ToUpper();
        }
        private string GetSHA256(string value)
        {
            var result = new StringBuilder();
            var sha256 = SHA256Managed.Create();
            var bts = Encoding.UTF8.GetBytes(value);
            var hash = sha256.ComputeHash(bts);
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
        [HttpPost]
        public async Task<IActionResult> SearchAvailableRooms(DateTime checkInDate, DateTime checkOutDate, int CategoryID)
        {
            var availableRooms = await (
                from room in _context.Rooms
                join orderDetail in _context.RoomOrderDetails on room.ID equals orderDetail.RoomID
                join order in _context.RoomOrder on orderDetail.OrderID equals order.ID
                where order.CheckIn > new DateTime(2023, 9, 11) &&
                      order.CheckOut > new DateTime(2023, 12, 20) &&
                      room.CategoryID == 1
                select new
                {
                    room.CategoryID,
                    order.CheckIn,
                    order.CheckOut,
                    RoomID = orderDetail.RoomID
                }
            ).ToListAsync();

            return View("Index", availableRooms);
        }




        private void SetUserViewBag()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var userName = HttpContext.Session.GetString("UserName");
                var userId = HttpContext.Session.GetInt32("UserId");

                ViewBag.UserName = userName;
                ViewBag.UserId = userId;
            }

            if (HttpContext.Session.GetInt32("EmployeeId") != null)
            {
                var employeeName = HttpContext.Session.GetString("EmployeeName");
                var employeeId = HttpContext.Session.GetInt32("EmployeeId");

                ViewBag.EmployeeName = employeeName;
                ViewBag.EmployeeId = employeeId;
            }
        }
    }
}