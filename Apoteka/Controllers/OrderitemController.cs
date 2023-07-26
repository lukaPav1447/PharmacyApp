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
    public class OrderitemController : Controller
    {
        private readonly apoteka_dbContext _context;

        public OrderitemController(apoteka_dbContext context)
        {
            _context = context;
        }

        // GET: Orderitem
        public async Task<IActionResult> Index()
        {
            var apoteka_dbContext = _context.Orderitems.Include(o => o.Order).Include(o => o.Product);
            return View(await apoteka_dbContext.ToListAsync());
        }

        // GET: Orderitem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orderitems == null)
            {
                return NotFound();
            }

            var orderitem = await _context.Orderitems
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.Orderitemid == id);
            if (orderitem == null)
            {
                return NotFound();
            }

            return View(orderitem);
        }

        // GET: Orderitem/Create
        public IActionResult Create()
        {
            var Narudzba = _context.Orders.Select(x => new
            {
                Orderid = x.Orderid,
                Naziv = x.Orderid + " - Date: " + x.Orderdate + " Total: " + x.Totalamount
            });

            ViewData["Orderid"] = new SelectList(Narudzba, "Orderid", "Naziv");
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Name");
            return View();
        }

        // POST: Orderitem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Orderitemid,Orderid,Productid,Quantity,Price")] Orderitem orderitem)
        {
            //if (ModelState.IsValid)
            //{
                _context.Add(orderitem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            ViewData["Orderid"] = new SelectList(_context.Orders, "Orderid", "Orderid", orderitem.Orderid);
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Productid", orderitem.Productid);
            return View(orderitem);
        }

        // GET: Orderitem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orderitems == null)
            {
                return NotFound();
            }

            var orderitem = await _context.Orderitems.FindAsync(id);
            if (orderitem == null)
            {
                return NotFound();
            }
            ViewData["Orderid"] = new SelectList(_context.Orders, "Orderid", "Orderid", orderitem.Orderid);
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Description", orderitem.Productid);
            return View(orderitem);
        }

        // POST: Orderitem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Orderitemid,Orderid,Productid,Quantity,Price")] Orderitem orderitem)
        {
            if (id != orderitem.Orderitemid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderitem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderitemExists(orderitem.Orderitemid))
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
            ViewData["Orderid"] = new SelectList(_context.Orders, "Orderid", "Orderid", orderitem.Orderid);
            ViewData["Productid"] = new SelectList(_context.Products, "Productid", "Description", orderitem.Productid);
            return View(orderitem);
        }

        // GET: Orderitem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orderitems == null)
            {
                return NotFound();
            }

            var orderitem = await _context.Orderitems
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.Orderitemid == id);
            if (orderitem == null)
            {
                return NotFound();
            }

            return View(orderitem);
        }

        // POST: Orderitem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orderitems == null)
            {
                return Problem("Entity set 'apoteka_dbContext.Orderitems'  is null.");
            }
            var orderitem = await _context.Orderitems.FindAsync(id);
            if (orderitem != null)
            {
                _context.Orderitems.Remove(orderitem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderitemExists(int id)
        {
          return (_context.Orderitems?.Any(e => e.Orderitemid == id)).GetValueOrDefault();
        }
    }
}
