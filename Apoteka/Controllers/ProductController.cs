using Apoteka.Data;
using Apoteka.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Apoteka.Controllers
{
    public class ProductController : Controller
    {
        private readonly apoteka_dbContext _context;

        public ProductController(apoteka_dbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int pg=1)
        {
            const int pageSize = 10;
            if (pg < 1)
                pg = 1;
            int pageNumCount = _context.Products.Count();

            var pager = new Pager(pageNumCount, pg, pageSize);

            int pageNumSkip = (pg -1) * pageSize;

            List<Product> products = _context.Products.Include(x => x.Category).Include(a=>a.Recipetype).Skip(pageNumSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;

            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            Product product = new Product();
            ViewData["categoryid"] = new SelectList(_context.Categories, "categoryid", "Name");
            ViewData["recipetypeid"] = new SelectList(_context.Recipetypes, "recipetypeid", "Name");
            return View(product);
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            var productid = _context.Products.Max(prodid => prodid.Productid);
            int productNo;
            
            productNo = productid + 1;

            product.Productid = productNo;
            _context.Attach(product);
            _context.Entry(product).State = EntityState.Added;
            _context.SaveChanges();
            return RedirectToAction("index");
            ViewData["categoryid"] = new SelectList(_context.Categories, "categoryid", "Name");
            ViewData["recipetypeid"] = new SelectList(_context.Recipetypes, "recipetypeid", "Name");
            return View(product);
        }

        public IActionResult Details(int Id)
        {
            Product product = _context.Products.Include(x => x.Category).Include(a => a.Recipetype).Where(p => p.Productid == Id).FirstOrDefault();

            return View(product);
        }

        [HttpGet]

        public IActionResult Edit(int Id)
        {
            Product product = _context.Products.Include(x => x.Categoryid).Include(a => a.Recipetypeid).Where(p => p.Productid == Id).FirstOrDefault();
            return View(product);
        }
        [HttpPost]

        public IActionResult Edit(Product product)
        {

            _context.Attach(product);
            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("index");
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Categories", "Name", product.Categoryid);
            //ViewData["RecipeTypeId"] = new SelectList(_context.Recipetypes, "Recipe Type", "Name", product.Recipetypeid);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Product product = _context.Products.Where(p => p.Productid == id).FirstOrDefault();
            return View(product);  
        }

        [HttpPost]
        public IActionResult Delete(Product product) 
        {
            _context.Attach(product);
            _context.Entry(product).State = EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
