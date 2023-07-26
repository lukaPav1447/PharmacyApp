using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Apoteka.Data;
using Apoteka.Models;

namespace Apoteka.Controllers
{
    public class OrderController : Controller
    {
        private readonly apoteka_dbContext _context;

        public OrderController(apoteka_dbContext context)
        {
            _context = context;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
              return _context.Orders != null ? 
                          View(await _context.Orders.Include(f => f.Orderitems).ToListAsync()) :
                          Problem("Entity set 'apoteka_dbContext.Orders'  is null.");
        }

        [HttpPost]
        public JsonResult SnimiNarudzbu([FromBody] Order narudzba)
        {
            try
            {
                // Kreiranje nove narudžbe
                var novaNarudzba = new Order
                {
                    Orderdate = narudzba.Orderdate,
                    Totalamount = narudzba.Totalamount
                };

                // Spremanje nove narudžbe u bazu
                _context.Orders.Add(novaNarudzba);
                _context.SaveChanges();

                // Spremanje stavki narudžbe
                foreach (var stavka in narudzba.Orderitems)
                {
                    stavka.Orderid = novaNarudzba.Orderid;
                    _context.Orderitems.Add(stavka);

                    var proizvod = _context.Products.FirstOrDefault(p => p.Productid == stavka.Productid);
                    if (proizvod != null)
                    {
                        proizvod.Quantity -= stavka.Quantity;
                        _context.Products.Update(proizvod);
                    }
                }

                _context.SaveChanges();

                return Json(new { message = "Narudžba je uspješno spremljena." });
            }
            catch (Exception ex)
            {
                return Json(new { error = "Greška prilikom spremanja narudžbe: " + ex.Message });
            }
        }


        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Orderid == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Orderid,Orderdate,Totalamount")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Orderid,Orderdate,Totalamount")] Order order)
        {
            if (id != order.Orderid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Orderid))
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
            return View(order);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Orderid == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'apoteka_dbContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return (_context.Orders?.Any(e => e.Orderid == id)).GetValueOrDefault();
        }
    }
}
