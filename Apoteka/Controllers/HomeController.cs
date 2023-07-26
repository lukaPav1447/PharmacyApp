using Apoteka.Data;
using Apoteka.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Apoteka.Controllers
{
    public class HomeController : Controller
    {
        private readonly apoteka_dbContext _context;

        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public HomeController(apoteka_dbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //if (idKategorije != 0)
            //{
            //    var proizvodi = await _context.Products.Include(p => p.Category).Include(p => p.Recipetype).Where(p => p.Categoryid == idKategorije).ToListAsync();
            //    ViewData["Categories"] = _context.Categories.ToList();
            //    return View(proizvodi);
            //}
            //else
            //{
            //    var proizvodi = await _context.Products.Include(p => p.Category).Include(p => p.Recipetype).ToListAsync();
            //    ViewData["Categories"] = _context.Categories.ToList();
            //    return View(proizvodi);
            //}

            var categories = _context.Categories.ToList(); // Dohvati sve kategorije iz baze podataka

            ViewBag.Categories = categories; // Pohrani kategorije u ViewBag

            return View();
        }

        [HttpGet]
        public JsonResult GetTopSoldProductsByMonth(int month)
        {
            var startDate = new DateTime(DateTime.Now.Year, month, 1);
            var endDate = startDate.AddMonths(1);

            var topSoldProducts = _context.Orderitems
                .Include(oi => oi.Product)
                .Where(oi => oi.Order.Orderdate >= startDate && oi.Order.Orderdate < endDate)
                .GroupBy(oi => new { oi.Productid, oi.Product.Name })
                .Select(g => new {
                    ProductID = g.Key.Productid,
                    Naziv = g.Key.Name,
                    Quantity = g.Sum(oi => oi.Quantity)
                })
                .OrderByDescending(g => g.Quantity) // Sortiraj prema količini (od najveće do najmanje)
                .Take(5) // Dohvati prvih 5 najprodavanijih proizvoda
                .ToList();

            return Json(topSoldProducts);
        }



        [HttpPost]
        public JsonResult GetSoldProductQuantitiesByDay(DateTime date, int categoryId)
        {

            var test = HttpContext;
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            //int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            //int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");

            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var startDate = date.Date;
            var endDate = startDate.AddDays(1);

            IQueryable<Orderitem> soldProductsQuery = _context.Orderitems
                .Include(oi => oi.Product)
                .Where(oi => oi.Order.Orderdate >= startDate && oi.Order.Orderdate < endDate);

            if (categoryId != 0)
            {
                soldProductsQuery = soldProductsQuery.Where(oi => oi.Product.Categoryid == categoryId);
            }

            var soldProducts = soldProductsQuery
                .GroupBy(oi => new { oi.Productid, oi.Product.Name })
                .Select(g => new {
                    ProductID = g.Key.Productid,
                    Naziv = g.Key.Name,
                    Quantity = g.Sum(oi => oi.Quantity)
                })
                .ToList();

            if (!string.IsNullOrEmpty(searchValue))
            {
                soldProducts = soldProducts.Where(m => m.Naziv.ToLower().Contains(searchValue) || m.Quantity.ToString().Contains(searchValue)).ToList();
            }

            // get total count of records after search
            recordsTotal = soldProducts.Count();

            //if (!string.IsNullOrEmpty(sortColumn))
            //{
            //    if (!string.IsNullOrEmpty(sortColumnDirection))
            //    {

            //        soldProducts = sortColumnDirection.Equals("asc") ? soldProducts.OrderBy(a => a.GetType().GetProperty(sortColumn).GetValue(a, null)).ToList() : soldProducts.OrderByDescending(a => a.GetType().GetProperty(sortColumn).GetValue(a, null)).ToList();

            //    }
            //}

            //pagination
            var empList = soldProducts.Skip(skip).Take(pageSize).ToList();
            var returnObj = new
            {
                draw = draw,
                recordsFiltered = recordsTotal,
                recordsTotal = recordsTotal,
                data = empList
            };

            return Json(returnObj);
        }

        [HttpPost]
        public JsonResult GetSoldProductQuantitiesByMonth(int month)
        {

            var test = HttpContext;
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            //int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            //int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");

            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var startDate = new DateTime(DateTime.Now.Year, month, 1);
            var endDate = startDate.AddMonths(1);
            
            var soldProducts = _context.Orderitems
                .Include(oi => oi.Product)
                .Where(oi => oi.Order.Orderdate >= startDate && oi.Order.Orderdate < endDate)
                .GroupBy(oi => new { oi.Productid, oi.Product.Name })
                .Select(g => new {
                    ProductID = g.Key.Productid,
                    Naziv = g.Key.Name,
                    Quantity = g.Sum(oi => oi.Quantity)
                })
                .ToList();

            if (!string.IsNullOrEmpty(searchValue))
            {
                soldProducts = soldProducts.Where(m => m.Naziv.ToLower().Contains(searchValue) || m.Quantity.ToString().Contains(searchValue)).ToList();
            }

            // get total count of records after search
            recordsTotal = soldProducts.Count();

            //if (!string.IsNullOrEmpty(sortColumn))
            //{
            //    if (!string.IsNullOrEmpty(sortColumnDirection))
            //    {

            //        soldProducts = sortColumnDirection.Equals("asc") ? soldProducts.OrderBy(a => a.GetType().GetProperty(sortColumn).GetValue(a, null)).ToList() : soldProducts.OrderByDescending(a => a.GetType().GetProperty(sortColumn).GetValue(a, null)).ToList();

            //    }
            //}

            //pagination
            var empList = soldProducts.Skip(skip).Take(pageSize).ToList();
            var returnObj = new
            {
                draw = draw,
                recordsFiltered = recordsTotal,
                recordsTotal = recordsTotal,
                data = empList
            };

            return Json(returnObj);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}