using Microsoft.AspNetCore.Mvc;
using project_ver1.Models;
using System;
using System.Data.Common;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Net.Mail;


namespace project_ver1.Controllers
{
    public class HomeController : Controller
    {
        // private readonly ILogger<HomeController> _logger;
        private readonly HomeDbContext _context;
        private readonly EmailService _emailService;
        private static Dictionary<string, string> resetTokens = new Dictionary<string, string>(); // 用於暫存重置密碼的 token

        //新增AppDbContext 
        public HomeController(HomeDbContext context, EmailService emailService)
        {
            //_logger = logger; 
            _context = context;
            _emailService = emailService;

            //新增

        }



        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Index()
        {
            SetUserViewBag();
            return View();
        }

        public ActionResult Park_information()
        {
            SetUserViewBag();
            return View();
        }


        public ActionResult member()
        {
            return View();
        }
        //[HttpPost]
        public IActionResult About()
        {
            SetUserViewBag();
            // _context.Contacts.Add(item);
            //_context.SaveChanges();
            return View();
        }

        public ActionResult Order()
        {
            SetUserViewBag();
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                var orders = _context.AgriculturalOrder
                                      .Where(order => order.CustomerID == userId)
                                      .ToList();

                var AgriculturalOrderModels = new List<AgriculturalOrderModel>();

                foreach (var order in orders)
                {
                    var orderDetails = _context.AgriculturalOrderDetail
                                              .Where(detail => detail.OrderID == order.ID)
                                              .ToList();
                    var productIds = orderDetails.Select(detail => detail.ProductID).ToList();

                    var products = _context.AgriculturalProduct
                                           .Where(product => productIds.Contains(product.ID))
                                           .ToList();

                    var productCategories = _context.AgriculturalProductsCategory
                                                    .Where(category => products.Select(product => product.CategoryID).Contains(category.ID))
                                                    .ToList();

                    AgriculturalOrderModels.Add(new AgriculturalOrderModel
                    {
                        Order = order,
                        OrderDetails = orderDetails,
                        Products = products,
                        ProductCategories = productCategories
                    });
                }

                var RoomOrders = _context.RoomOrder
                                      .Where(order => order.CustomerID == userId)
                                      .ToList();

                var RoomOrderModels = new List<RoomOrderModel>();

                foreach (var roomorder in RoomOrders)
                {


                    var orderDetails = _context.RoomOrderDetails
                                              .Where(detail => detail.OrderID == roomorder.ID)
                                              .ToList();

                    var roomIds = orderDetails.Select(detail => detail.RoomID).ToList();

                    var rooms = _context.Rooms
                                        .Where(room => roomIds.Contains(room.ID))
                                        .ToList();

                    var roomCategories = _context.RoomCategory
                                              .Where(category => rooms.Select(room => room.CategoryID).Contains(category.CategoryID))
                                              .ToList();

                    RoomOrderModels.Add(new RoomOrderModel
                    {
                        RoomOrder = roomorder,
                        RoomOrderDetails = orderDetails,
                        Rooms = rooms,
                        RoomCategory = roomCategories
                    });
                }

                var combinedViewModel = new CombinedOrderViewModel
                {
                    AgriculturalOrders = AgriculturalOrderModels,
                    RoomOrders = RoomOrderModels
                };

                return View(combinedViewModel);
            }
            else
            {
                return RedirectToAction("member", "Home");
            }
        }

        public IActionResult News(int CategoryID)
        {

            SetUserViewBag();
            List<News> newsList;

            if (CategoryID == 2)
            {
                newsList = _context.News.Where(news => news.CategoryID == 2).ToList();
            }
            else
            {
                newsList = _context.News.Where(news => news.CategoryID == 1).ToList();
            }


            return View(newsList);

        }







        [HttpPost]
        public IActionResult About(Contact appdb)
        {

            // 在這裡處理從表單提交的數據
            // 您可以將其儲存到資料庫中，發送郵件，或者進行其他適當的處理

            // 這裡只是一個示例，您可以根據自己的需要進行更改
            DateTime now = DateTime.Now;
            Contact contact = new Contact
            {
                ID = appdb.ID,
                QuestionCategory = appdb.QuestionCategory,
                Name = appdb.Name,
                Sex = appdb.Sex,
                Phone = appdb.Phone,
                QuestionContent = appdb.QuestionContent,
                SetTime = now
            };
            _context.Contact.Add(contact);
            _context.SaveChanges();
            // 假設您想要將提交的數據傳遞到另一個視圖中以顯示
            return Redirect("/Home/Index");



        }

        public IActionResult Account()
        {

            // 已登入>跳到會員頁面，未登入>跳到登入頁面
            if (HttpContext.Session.GetInt32("UserId") != null)
            {

                return RedirectToAction("Members_Information", "Home");
            }
            else if (HttpContext.Session.GetInt32("EmployeeId") != null)
            {
                return RedirectToAction("Backstage", "Home");
            }
            else
            {

                return RedirectToAction("member", "Home");
            }


        }
        public IActionResult Backstage()
        {
            SetUserViewBag();
            if (HttpContext.Session.GetInt32("EmployeeId") != null)
            {
                var backcontact = _context.Contact
                                     .Where(item => !item.Finished)
                                     .ToList();

                return View(backcontact);
            }
            else
            {
                return RedirectToAction("member", "Home");
            }
        }

        public IActionResult BackstageContact()
        {
            SetUserViewBag();
            if (HttpContext.Session.GetInt32("EmployeeId") != null)
            {
                var backcontact = _context.Contact.ToList();


                return View(backcontact);
            }
            else
            {
                return RedirectToAction("member", "Home");
            }
        }

        //員工頁面完成，顧客問題表單
        [HttpPost]
        public IActionResult ContactEdit(int contactId)
        {

            var contactToUpdate = _context.Contact.FirstOrDefault(c => c.ID == contactId);

            if (contactToUpdate != null)
            {

                contactToUpdate.Finished = true;


                _context.SaveChanges();
            }


            return RedirectToAction("BackstageContact");
        }
        public IActionResult Members_join()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Members_join(Customer myuser)
        {
            ViewBag.DuplicateMessage = "該電子信箱已被註冊";

            // 檢查電子信箱是否已經存在於資料庫中
            var existingCustomer = _context.Customer.FirstOrDefault(c => c.Email == myuser.Email);
            if (existingCustomer != null)
            {
                //ModelState.AddModelError("Email", "該電子信箱已經被註冊。");
                return View(myuser);
            }

            // 儲存用戶註冊資訊到資料庫
            _context.Customer.Add(myuser);
            _context.SaveChanges();
            HttpContext.Session.SetString("UserName", myuser.Name);
            HttpContext.Session.SetInt32("UserId", myuser.ID);

            TempData["SuccessMessage"] = "註冊成功";
            // 註冊成功後重定向到首頁或其他頁面
            //return View("Members_Information", "Home");

            // 返回註冊頁面並顯示錯誤信息
            return View(myuser);
        }



        public IActionResult Members_Information()
        {
            SetUserViewBag();
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                var user = _context.Customer.FirstOrDefault(u => u.ID == userId);
                return View(user);
            }
            else
            {
                return RedirectToAction("member", "Home");
            }

        }

        //會員資料修改頁面
        public IActionResult CustomerEdit()
        {
            SetUserViewBag();
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                var user = _context.Customer.FirstOrDefault(u => u.ID == userId);
                return View(user);
            }
            else
            {
                return RedirectToAction("member", "Home");
            }
        }
        //會員資料修改
        [HttpPost]
        public IActionResult CustomerEdit(Customer customer)
        {
            SetUserViewBag();
            if (ModelState.IsValid)
            {
                var user = _context.Customer.FirstOrDefault(u => u.ID == customer.ID);
                if (user != null)
                {
                    user.Name = customer.Name;
                    user.Email = customer.Email;
                    user.Phone = customer.Phone;
                    user.Address = customer.Address;
                    user.Pwd = customer.Pwd;

                    _context.SaveChanges();
                    HttpContext.Session.SetString("UserName", user.Name);
                }
                return RedirectToAction("Members_Information");
            }
            return View(customer);
        }


        [HttpPost]
        public IActionResult member(LoginViewModel model)
        {
            var user = _context.Customer.FirstOrDefault(u => u.Email == model.Email && u.Pwd == model.Password);
            var employee = _context.Employee.FirstOrDefault(u => u.Email == model.Email && u.Pwd == model.Password);

            if (employee != null)
            {
                HttpContext.Session.SetInt32("EmployeeId", employee.ID);
                HttpContext.Session.SetString("EmployeeName", employee.Name);
                return RedirectToAction("Backstage", "Home");
            }
            else
            {
                if (user != null)
                {
                    // 登入成功，資訊保存到Session中
                    //HttpContext.Session.SetString("key", "value");
                    HttpContext.Session.SetInt32("UserId", user.ID);
                    HttpContext.Session.SetString("UserName", user.Name);

                    return RedirectToAction("Members_Information", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = "帳號或密碼不正確";
                    return View();
                }
            }
        }

        //改密碼
        public class EmailService
        {
            private readonly IConfiguration _configuration;

            public EmailService(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public async Task SendEmailAsync(string toEmail, string subject, string message)
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("果茶庄園重設密碼", _configuration["Smtp:Username"]));
                emailMessage.To.Add(new MailboxAddress("", toEmail));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart("html")
                {
                    Text = message
                };

                // MailKit
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect(_configuration["Smtp:Host"], int.Parse(_configuration["Smtp:Port"]), false);
                    client.Authenticate(_configuration["Smtp:Username"], _configuration["Smtp:Password"]);

                    await client.SendAsync(emailMessage);
                    client.Disconnect(true);
                }
            }
        }

        //藏資料
        private static string GenerateResetToken()
        {
            return Guid.NewGuid().ToString();
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Customer.FirstOrDefault(c => c.Email == model.Email);
                if (user != null)
                {
                    var resetToken = GenerateResetToken(); // 生成隨機碼
                    resetTokens[resetToken] = user.Email; // 將 token 存到字典中
                    var resetLink = Url.Action("ResetPassword", "Home", new { token = resetToken }, Request.Scheme);

                    var message = $"請點擊以下連結來重設您的密碼: <a href='{resetLink}'>重設密碼</a>";
                    await _emailService.SendEmailAsync(user.Email, "重設密碼", message);
                  

                    return View("ForgotPasswordConfirmation");
                }
                else
                {
                    ViewBag.ErrorMessage = "此信箱尚未註冊";
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            if (ModelState.IsValid)
            {

                // 找到對應的Email
                if (resetTokens.TryGetValue(token, out string email))
                {
                    var model = new ResetPasswordViewModel { Token = token, Email = email };
                    return View(model);
                }
                else
                {
                    //  token 無效
                    return RedirectToAction("Index");
                }
                
            }
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
           
            var user = _context.Customer.FirstOrDefault(c => c.Email == model.Email);
            if (user != null)
            {
                // 修改密碼
                user.Pwd = model.Password;


                _context.SaveChanges();
            }
            TempData["SuccessMessage"] = "成功修改密碼";
            return View();

        }



      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Logout()
        {
            // 登出，刪除Session中的用戶資訊
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("EmployeeId");
            HttpContext.Session.Remove("EmployeeName");
            return RedirectToAction("Index", "Home");
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
                var EmployeeName = HttpContext.Session.GetString("EmployeeName");
                var EmployeeId = HttpContext.Session.GetInt32("EmployeeId");

                ViewBag.UserName = EmployeeName;
                ViewBag.UserId = EmployeeId;
            }
        }


    }


}
