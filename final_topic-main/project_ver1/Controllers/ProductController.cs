using Microsoft.AspNetCore.Mvc;
using project_ver1.Models;
using System;
using System.Data.Common;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using static project_ver1.Controllers.ProductController;
using System.Collections.Generic;

// import HttpContext.Session.GetObject<T>('key') and HttpContext.Session.SetObject<T>('key', 'value')
using MyWebsite.Extensions;
using NuGet.Packaging;

namespace project_ver1.Controllers
{
    public class ProductController : Controller
    {
        private readonly HomeDbContext _context;
        private OrderData orderData;
        public ProductController(HomeDbContext context)
        {
            //_logger = logger; 
            _context = context;

        }
        public IActionResult Index()
        {
            var ap = _context.AgriculturalProduct.ToList();
            SetUserViewBag();
            return View(ap);
        }

        public IActionResult ShoppingCart()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {

                SetUserViewBag();
                return View();
            }
            else
            {

                return Redirect("/Home/member");
            }
        }


        [HttpPost]
        public IActionResult Index(int myCategoryID)
        {
            var product = _context.AgriculturalProduct.ToList();
            if (myCategoryID > 0)
            {
                product = null;
                product = _context.AgriculturalProduct.Where(p => p.CategoryID == myCategoryID).ToList();
            }
            SetUserViewBag();
            return View(product);
        }

        public ActionResult EachProductDetails(int? id)
        {
            var product = _context.AgriculturalProduct.Find(id);
            SetUserViewBag();
            return View(product);
        }

        private void Get_AgriculturalOrder()
        {
            //新增

        }
        [HttpPost]
        public IActionResult ShoppingCart(SetOrder setOrder)
        {
            SetUserViewBag();
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                if (HttpContext.Session.GetObject<OrderData>("cart") == null)
                {
                    orderData = new OrderData();
                    orderData.SumPrice = 0;
                    orderData.CustomerID = (int)HttpContext.Session.GetInt32("UserId");
                    orderData.OderTime = DateTime.Now;
                    orderData.OrderFinished = false;
                    orderData.EmployeeID = null;
                }
                else
                {
                    orderData = HttpContext.Session.GetObject<OrderData>("cart");
                }

                DetailData setDetails = new DetailData
                {
                    ProductID = setOrder.ProductID,
                    Count = setOrder.Count,
                    Price = setOrder.Price
                };

                if (orderData.Details == null)
                {
                    orderData.Details = new List<DetailData>();
                }

                var existingDetail = orderData.Details.FirstOrDefault(d => d.ProductID == setOrder.ProductID);
                if (existingDetail != null)
                {
                    if (setOrder.Count == 0)
                    {
                        orderData.SumPrice -= existingDetail.Count * existingDetail.Price;
                        orderData.Details.Remove(existingDetail);
                    }
                    else
                    {
                        orderData.SumPrice -= existingDetail.Count * existingDetail.Price;
                        existingDetail.Count = setOrder.Count;
                        existingDetail.Price = setOrder.Price;
                        orderData.SumPrice += existingDetail.Count * existingDetail.Price;
                    }
                }
                else
                {
                    if (setOrder.Count != 0)
                    {
                        orderData.Details.Add(setDetails);
                        orderData.SumPrice += setOrder.Count * setOrder.Price;
                    }
                }

                HttpContext.Session.SetObject("cart", orderData);

                var list = new List<object>();
                var countList = new List<int>();
                var priceList = new List<int>();

                foreach (var item in orderData.Details)
                {
                    var query = _context.AgriculturalProduct.Find(item.ProductID);
                    list.Add(query);
                    countList.Add(item.Count);
                    priceList.Add((int)item.Price);
                }

                ViewBag.Product_Count = countList;
                ViewBag.Product_Price = priceList;

                return View(list);
            }
            else
            {
                return Redirect("/Home/member");
            }
        }



        public class SetOrder
        {

            public int Count { get; set; }
            public int ProductID { get; set; }
            public int Price { get; set; }

        }

        public class OrderData : Agricultural_Order
        {
            // multiple order details in an order
            public List<DetailData> Details { get; set; }
            public OrderData()
            {
                Details = new List<DetailData>();
            }
        }
        public class DetailData : Agricultural_Order_Details
        {

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

        [HttpPost]
        public IActionResult SaveToDatabase()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {

                var orderData = HttpContext.Session.GetObject<OrderData>("cart");

                if (orderData != null)
                {

                    var agriculturalOrder = new Agricultural_Order
                    {
                        CustomerID = orderData.CustomerID,
                        OderTime = orderData.OderTime,
                        OrderFinished = orderData.OrderFinished,
                        EmployeeID = orderData.EmployeeID,
                        SumPrice = orderData.SumPrice
                    };


                    _context.AgriculturalOrder.Add(agriculturalOrder);
                    _context.SaveChanges();


                    foreach (var detail in orderData.Details)
                    {
                        var agriculturalOrderDetail = new Agricultural_Order_Details
                        {
                            OrderID = agriculturalOrder.ID,
                            ProductID = detail.ProductID,
                            Count = detail.Count,
                            Price = detail.Price
                        };

                        _context.AgriculturalOrderDetail.Add(agriculturalOrderDetail);
                    }

                    _context.SaveChanges();


                    HttpContext.Session.Remove("cart");
                }


                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/Home/member");
            }
        }



        //        Agricultural_Order ao = new Agricultural_Order();
        //                if (HttpContext.Session.GetInt32("OrderID") == null)
        //                {

        //                    // create a new Agricultural_Order data and insert into Agricultural_Order
        //                    ao.CustomerID = (int) HttpContext.Session.GetInt32("UserId");
        //        ao.OderTime = DateTime.Now;
        //                    ao.OrderFinished = false;
        //                    ao.EmployeeID = null;
        //                    ao.SumPrice = 0;
        //                    _context.AgriculturalOrder.Add(ao);
        //                    _context.SaveChanges();

        //                    //set OrderID in session Storage
        //                    HttpContext.Session.SetInt32("OrderID", ao.ID);
        //                }
        //    // create a new Agricultural_Order_Details
        //    Agricultural_Order_Details aod = new Agricultural_Order_Details
        //    {
        //        OrderID = (int)HttpContext.Session.GetInt32("OrderID"),
        //        ProductID = setOrder.ProductID,
        //        Count = setOrder.Count,
        //        Price = setOrder.Price
        //    };
        //    //if this aod is not exists (the same OrderID and ProductID)
        //    var getDetail = _context.AgriculturalOrderDetail.Find(aod.OrderID, aod.ProductID);
        //    var getOrder = _context.AgriculturalOrder.Find(HttpContext.Session.GetInt32("OrderID"));

        //                if (getDetail == null)
        //                {
        //                    // add the price 
        //                    getOrder.SumPrice += aod.Price* aod.Count;
        //    //insert into the table
        //    _context.AgriculturalOrderDetail.Add(aod);
        //                }
        //                else
        //{
        //    // minus the price
        //    getOrder.SumPrice -= getDetail.Price * getDetail.Count;
        //    // edit the row
        //    getDetail.Count = aod.Count;
        //    getDetail.Price = aod.Price;
        //    // add the price
        //    getOrder.SumPrice += getDetail.Price * getDetail.Count;
        //}

        //_context.SaveChanges();




    }
}

