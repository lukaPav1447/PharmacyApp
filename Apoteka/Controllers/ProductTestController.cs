using Apoteka.Data;
using Apoteka.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Apoteka.Controllers
{
    public class ProductTestController : Controller
    {
        private readonly apoteka_dbContext _context;

        public ProductTestController(apoteka_dbContext context)
        {
            _context = context;
        }

        // GET: ProductTest
        public async Task<IActionResult> Index()
        {
            var apoteka_dbContext = _context.Products.Include(p => p.Category).Include(p => p.Recipetype);
            return View(await apoteka_dbContext.ToListAsync());
        }

        // GET: ProductTest/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Recipetype)
                .FirstOrDefaultAsync(m => m.Productid == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: ProductTest/Create
        public IActionResult Create()
        {
            ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "Name");
            ViewData["Recipetypeid"] = new SelectList(_context.Recipetypes, "Recipetypeid", "Name");
            return View();
        }

        // POST: ProductTest/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Productid,Name,Description,Baseprice,Quantity,Categoryid,Recipetypeid")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "Categoryid", product.Categoryid);
            ViewData["Recipetypeid"] = new SelectList(_context.Recipetypes, "Recipetypeid", "Recipetypeid", product.Recipetypeid);
            return View(product);
        }

        //GET: ProductTest/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "Categoryid", product.Categoryid);
            ViewData["Recipetypeid"] = new SelectList(_context.Recipetypes, "Recipetypeid", "Recipetypeid", product.Recipetypeid);
            return View(product);
        }

        // POST: ProductTest/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        public async Task<IActionResult> Edit([Bind("Productid,Name,Description,Baseprice,Quantity,Categoryid,Recipetypeid")] Product product)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Productid))
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
            ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "Categoryid", product.Categoryid);
            ViewData["Recipetypeid"] = new SelectList(_context.Recipetypes, "Recipetypeid", "Recipetypeid", product.Recipetypeid);
            return View(product);
        }

        // GET: ProductTest/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Recipetype)
                .FirstOrDefaultAsync(m => m.Productid == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: ProductTest/Delete/5
        [HttpDelete]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'apoteka_dbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.Productid == id)).GetValueOrDefault();
        }

        public IActionResult DohvatiProizvode(int id)
        {
            var products = _context.Products.ToList();
            if (id != 0)
            {
                products = products.Where(x => x.Productid == id).ToList();
            }

            return Json(products);
        }
    }
}
