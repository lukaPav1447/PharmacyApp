using Apoteka.Data;
using Apoteka.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Apoteka.Controllers
{
    public class ProductForOrderItemsController : Controller
    {
        private readonly apoteka_dbContext _context;

        public ProductForOrderItemsController(apoteka_dbContext context)
        {
            _context = context;
        }

        // GET: ProductForOrderItems
        public async Task<IActionResult> Index()
        {
            //var apoteka_dbContext = _context.Products.Include(p => p.Category).Include(p => p.Recipetype);
            //return View(await apoteka_dbContext.ToListAsync());
            var proizvodi = await _context.Products.Include(p => p.Category).Include(p => p.Recipetype).ToListAsync();
            ViewData["Categories"] = _context.Categories.ToList();
            return View(proizvodi);

        }
        [HttpPost]
        public async Task<IActionResult> Index(int idKategorije)
        {
            //var apoteka_dbContext = _context.Products.Include(p => p.Category).Include(p => p.Recipetype);
            //return View(await apoteka_dbContext.ToListAsync());
            if (idKategorije != 0)
            {
                var proizvodi = await _context.Products.Include(p => p.Category).Include(p => p.Recipetype).Where(p => p.Categoryid == idKategorije).ToListAsync();
                ViewData["Categories"] = _context.Categories.ToList();
                return View(proizvodi);
            }
            else
            {
                var proizvodi = await _context.Products.Include(p => p.Category).Include(p => p.Recipetype).ToListAsync();
                ViewData["Categories"] = _context.Categories.ToList();
                return View(proizvodi);
            }

        }

        // GET: ProductForOrderItems/Details/5
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

        // GET: ProductForOrderItems/Create
        public IActionResult Create()
        {
            ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "Categoryid");
            ViewData["Recipetypeid"] = new SelectList(_context.Recipetypes, "Recipetypeid", "Recipetypeid");
            return View();
        }

        // POST: ProductForOrderItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: ProductForOrderItems/Edit/5
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

        // POST: ProductForOrderItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Productid,Name,Description,Baseprice,Quantity,Categoryid,Recipetypeid")] Product product)
        {
            if (id != product.Productid)
            {
                return NotFound();
            }

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

        // GET: ProductForOrderItems/Delete/5
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

        // POST: ProductForOrderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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

        public List<Product> SearchByName(string searchQuery, int idKategorije)
        {
            if (string.IsNullOrEmpty(searchQuery) && idKategorije == 0)
            {
                return _context.Products.OrderBy(p => p.Categoryid).ToList();
            }
            else if (string.IsNullOrEmpty(searchQuery) && idKategorije != 0)
            {
                return _context.Products.Where(p => p.Categoryid == idKategorije).ToList();
            }
            else if (!string.IsNullOrEmpty(searchQuery) && idKategorije != 0)
            {
                searchQuery = searchQuery == null ? string.Empty : searchQuery;
                return _context.Products.Where(p => p.Name.ToLower().Contains(searchQuery.ToLower()) && p.Categoryid == idKategorije).OrderBy(p => p.Categoryid).ToList();

            }
            else
            {
                return _context.Products.Where(p => p.Name.ToLower().Contains(searchQuery.ToLower())).ToList();
            }
        }

        //[HttpGet]
        //public List<Category> AllCategories()
        //{
        //    return _context.Categories.ToList();
        //}

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.Productid == id)).GetValueOrDefault();
        }
    }
}
