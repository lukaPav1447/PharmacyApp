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
    public class RecipetypeController : Controller
    {
        private readonly apoteka_dbContext _context;

        public RecipetypeController(apoteka_dbContext context)
        {
            _context = context;
        }

        // GET: Recipetype
        public async Task<IActionResult> Index()
        {
              return _context.Recipetypes != null ? 
                          View(await _context.Recipetypes.ToListAsync()) :
                          Problem("Entity set 'apoteka_dbContext.Recipetypes'  is null.");
        }

        public JsonResult getAllRecipes()
        {
            return Json(_context.Recipetypes.ToList());
        }

        //[HttpPost]
        //public JsonResult GetRecipeTypes()
        //{

        //    var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
        //    var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
        //    //var sortColumn = "Recipetypeid";
        //    var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
        //    var searchValue = Request.Form["search[value]"].FirstOrDefault();
        //    //int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
        //    //int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");

        //    var start = Request.Form["start"].FirstOrDefault();
        //    var length = Request.Form["length"].FirstOrDefault();

        //    int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //    int skip = start != null ? Convert.ToInt32(start) : 0;
        //    int recordsTotal = 0;
        //    //var data = _context.CheckIns.AsQueryable();
        //    var data = _context.Recipetypes.ToList();

        //    if (!string.IsNullOrEmpty(searchValue))
        //    {
        //        data = data.Where(m => m.Name.ToLower().Contains(searchValue.ToLower()) || m.Pricemodifier.ToString().Contains(searchValue)).ToList();
        //    }

        //    // get total count of records after search
        //    recordsTotal = data.Count();

        //    if (!string.IsNullOrEmpty(sortColumn))
        //    {
        //        if (!string.IsNullOrEmpty(sortColumnDirection))
        //        {

        //            //data = sortColumnDirection.Equals("asc") ? data.OrderBy(a => a.GetType().GetProperty(sortColumn).GetValue(a, null)).ToList() : data.OrderByDescending(a => a.GetType().GetProperty(sortColumn).GetValue(a, null)).ToList();
        //            data = sortColumnDirection.Equals("asc") ? data.OrderBy(a => a.GetType().GetProperty(sortColumn).GetValue(a, null)).ToList() : data.OrderByDescending(a => a.GetType().GetProperty(sortColumn).GetValue(a, null)).ToList();
        //        }
        //    }

        //    //pagination
        //    var empList = data.Skip(skip).Take(pageSize).ToList();
        //    var returnObj = new
        //    {
        //        draw = draw,
        //        recordsFiltered = recordsTotal,
        //        recordsTotal = recordsTotal,
        //        data = empList
        //    };

        //    return Json(returnObj);
        //}

        // GET: Recipetype/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Recipetypes == null)
            {
                return NotFound();
            }

            var recipetype = await _context.Recipetypes
                .FirstOrDefaultAsync(m => m.Recipetypeid == id);
            if (recipetype == null)
            {
                return NotFound();
            }

            return View(recipetype);
        }

        // GET: Recipetype/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recipetype/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Recipetypeid,Name,Pricemodifier")] Recipetype recipetype)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recipetype);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recipetype);
        }

        // GET: Recipetype/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Recipetypes == null)
            {
                return NotFound();
            }

            var recipetype = await _context.Recipetypes.FindAsync(id);
            if (recipetype == null)
            {
                return NotFound();
            }
            return View(recipetype);
        }

        // POST: Recipetype/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Recipetypeid,Name,Pricemodifier")] Recipetype recipetype)
        {
            if (id != recipetype.Recipetypeid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipetype);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipetypeExists(recipetype.Recipetypeid))
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
            return View(recipetype);
        }

        // GET: Recipetype/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Recipetypes == null)
            {
                return NotFound();
            }

            var recipetype = await _context.Recipetypes
                .FirstOrDefaultAsync(m => m.Recipetypeid == id);
            if (recipetype == null)
            {
                return NotFound();
            }

            return View(recipetype);
        }

        // POST: Recipetype/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Recipetypes == null)
            {
                return Problem("Entity set 'apoteka_dbContext.Recipetypes'  is null.");
            }
            var recipetype = await _context.Recipetypes.FindAsync(id);
            if (recipetype != null)
            {
                _context.Recipetypes.Remove(recipetype);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipetypeExists(int id)
        {
          return (_context.Recipetypes?.Any(e => e.Recipetypeid == id)).GetValueOrDefault();
        }
    }
}

