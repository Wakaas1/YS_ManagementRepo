using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Common;
using YSManagmentSystem.BLL.OrderService;
using YSManagmentSystem.BLL.Products;
using YSManagmentSystem.Domain.Order;
using YSManagmentSystem.web.DTO;
using YSManagmentSystem.web.Models.DataTable;
using System.Linq;
using System.Web;
using DocumentFormat.OpenXml.InkML;
using YSManagmentSystem.Domain.Product;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

namespace YSManagmentSystem.web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderServices _order;
        private readonly ICustomerServices _customer;
        private readonly IProductServices _product;
       
        public OrderController(IOrderServices order,ICustomerServices customer,IProductServices product)
        {
            _order = order;
            _customer = customer;
            _product = product;
        }

        public IActionResult Index(DTReq req)
        {
            var loc = _order.GetAllOrderDT(req);
            return View(loc);
        }

        public IActionResult CreateOrder(int id)
        {
            ViewBag.CId = new SelectList(_customer.GetAllCustomer().ToList(), "Id", "CustomerName");
            if (id == 0)
            {
                System.Guid guid = System.Guid.NewGuid();
                var order = new tbl_Order();
                order.OrderDate = DateTime.Now;
                order.OrderNumber = guid.ToString();
                order.CustomerId = 0;
                order.Status = false;
                int r = _order.CreateNewOrder(id);
                var list = _order.GetOrderByItem(r);
                ViewBag.id = r;
                TempData["Oid"] = r;
                return View(list);
                
            }
            else
            {
                TempData["OId"] = id;
                var list = _order.GetOrderByItem(id);
                var tot = list.Sum(x => x.Total);
                ViewBag.tot = tot;
                return View(list);
            }
            
        }

        [HttpPost]
        public IActionResult AddCustomer(int CId)
        {
            int li = (int)TempData["OId"];
            ViewBag.CId = new SelectList(_customer.GetAllCustomer().ToList(), "Id", "CustomerName");
            _order.AddCustomer(li,CId);
            int id = li;
            return Redirect(Url.Action("CreateOrder", "Order", new { id }, "https"));
        }

        //[HttpPost]
        //public IActionResult CreateOrder(int id, AddOrder ord)
        //{
        //    ViewBag.PId = new SelectList(_product.GetAllProducts().ToList(), "Id", "ProductName");
        //    ViewBag.OId = new SelectList(_order.GetAllOrder().ToList(), "Id");
        //    System.Guid guid = System.Guid.NewGuid();
        //    var order = new tbl_Order();
        //    order.OrderDate = DateTime.Now;            
        //    order.OrderNumber = guid.ToString();
        //    order.CustomerId = id;
        //    order.Status = false;
        //    var r = _order.AddOrder(order);
        //    var product = _product.GetProductByID(ord.ProductId);
        //    var orderItem = new OrderItem();
        //    orderItem.OrderId = r;
        //    orderItem.ProductId = ord.ProductId;
        //    orderItem.Cost = (float)product.Price;
        //    orderItem.Quantity = ord.Quantity;
        //    orderItem.Total = (float)(ord.Quantity * product.Price);

        //    _order.AddOrderItem(orderItem);
        //    ViewBag.Oid = r;

        //    return View();
        //}

        //public IActionResult CreateOrderItem(int id)
        //{

        //    var list = _order.GetOrderByItem(id);



        //    return View(list);
        //}

        [HttpGet]
        public IActionResult AddOrderItem(int id)
        {
            ViewBag.CId = new SelectList(_customer.GetAllCustomer().ToList(), "Id", "CustomerName");
            ViewBag.PId = new SelectList(_product.GetAllProducts().ToList(), "Id", "ProductName");

            return View();
        }
        [HttpPost]
        public IActionResult AddOrderItem(AddOrderItem addOrder)
        {
            int li = (int)TempData["Oid"];
            ViewBag.PId = new SelectList(_product.GetAllProducts().ToList(), "Id", "ProductName");
            //ViewBag.OId = new SelectList(_order.GetAllOrders().ToList(), "Id");
            var product = _product.GetProductByID(addOrder.ProductId);
            var ordi = new OrderItem();
            ordi.OrderId = li;
            ordi.ProductId = addOrder.ProductId;
            ordi.Quantity = addOrder.Quaintity;
            ordi.Cost = (float)product.Price;
            ordi.Discount = (ordi.Cost * ordi.Discount)/100;
            ordi.Total = ordi.Quantity * (ordi.Cost - ordi.Discount);
            var i = _order.AddOrderItem(ordi);
            var id = _order.GetOrderItemByID(i).OrderId;


            TempData["OID"] = li;

            return Redirect(Url.Action("CreateOrder", "Order", new { id }, "https"));
        }
        [HttpGet]
        public IActionResult OrderItem(int id)
        {
            var ord = _order.GetOrderByID(id);
            var cus = _customer.GetCustomerByID(ord.CustomerId);
            ViewBag.CName = cus.CustomerName;
            ViewBag.ONum = ord.OrderNumber;
            ViewBag.GT = ord.Total;
            var list = _order.GetOrderByItem(id);
            return View(list);
        }

        //[HttpGet]
        //public IActionResult AddOrEditLocation(int? id)
        //{
        //    if (id > 0)
        //    {
        //        var loc = _location.GetLocationByID(id.GetValueOrDefault());
        //        return View(loc);
        //    }
        //    else
        //    {
        //        ViewBag.BId = new SelectList(_product.GetAllLocation().ToList(), "Id", "Location");

        //        return View();

        //    }

        //}

        //[HttpPost]

        //public IActionResult AddOrEditLocation(int? id, Locations loc)
        //{
        //    long result = 0;
        //    int Status;
        //    string Value;

        //    if (id > 0)
        //    {
        //        result = _location.UpdateLocation(loc);
        //        if (result > 0)
        //        {
        //            Status = 200;
        //            Value = Url.Content("~/Design/View/");
        //        }
        //        else
        //        {
        //            Status = 500;
        //            Value = "There is some error at server side";
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.BId = new SelectList(_product.GetAllLocation().ToList(), "Id", "Location");

        //        result = _location.AddLocation(loc);
        //        if (result > 0)
        //        {
        //            Status = 200;
        //            Value = Url.Content("~/Design/View/");
        //        }
        //        else
        //        {
        //            Status = 500;
        //            Value = "There is some error at server side";
        //        }
        //    }
        //    return Json(new { status = Status, value = Value });
        //}


        //[HttpGet]
        //public IActionResult DetailLocation(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var loc = _location.GetLocationByID(id.GetValueOrDefault());
        //    if (loc == null)

        //        return NotFound();

        //    return View(loc);
        //}


        //[HttpPost]
        //public IActionResult DeleteLocation(int id)
        //{
        //    if (_location.DeleteLocation(id) > 0)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}

        //Data Table, Searching, sorting, Paging, Total Count,Filtering


        //public IActionResult Add(int id)
        //{
        //    Product product = _product.GetProductByID(id);

        //    List<OrderItem> cart = HttpContext.Session.GetJson<List<OrderItem>>("Order") ?? new List<OrderItem>();

        //    OrderItem cartItem = cart.Where(x => x.Id == id).FirstOrDefault();

        //    if (cartItem == null)
        //    {
        //        cart.Add(new OrderItem(product));
        //    }
        //    else
        //    {
        //        cartItem.Quantity += 1;
        //    }

        //    HttpContext.Session.SetJson("Order", cart);

        //    if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
        //        return RedirectToAction("Index");

        //    return ViewComponent("SmallCart");
        //}


        // GET: OrderController/Checkout
      

        [HttpPost]    
        public IActionResult Checkout(int id)
        {
            int li = (int)TempData["OID"];

            return Redirect(Url.Action("OrderItem", "Order", new { li }, "https")); ;
        }

        // GET /cart/Clear
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Order");

            //return RedirectToAction("Page", "Pages");
            //return Redirect("/");
            if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                return Redirect(Request.Headers["Referer"].ToString());
            return Ok();
        }
        [HttpPost]
        public JsonResult GetAllOrder()
        {
            var request = new DTReq();
            request.draw = Convert.ToInt32(Request.Form["draw"]);
            request.StartRowIndex = Convert.ToInt32(Request.Form["start"]);
            request.SortExpression = Request.Form["order[0][dir]"];
            request.PageSize = Convert.ToInt32(Request.Form["length"]);
            request.SearchText = Request.Form["search[value]"];



            var loc = _order.GetAllOrderDT(request).Result;
            return Json(loc);
        }

    }
}
