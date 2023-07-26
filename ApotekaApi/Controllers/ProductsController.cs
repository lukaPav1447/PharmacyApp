using ApotekaApi.Models;
using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;

namespace ApotekaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly apoteka_dbContext _context;

        public ProductsController(apoteka_dbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // GET: api/Products/ByCategory/{categoryId}
        [HttpGet("ByCategory/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(int categoryId)
        {
            var productsByCategory = await _context.Products.Where(p => p.Categoryid == categoryId).ToListAsync();

            if (productsByCategory.Count == 0)
            {
                return NotFound();
            }

            return productsByCategory;
        }

        [HttpPost("import")]
        public IActionResult ImportProducts(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                var records = csv.GetRecords<ProductCsvModel>().ToList();

                // Konverzija ProductCsvModel u Product model
                var products = records.Select(r => new Product
                {
                    Name = r.Name,
                    Description = r.Description,
                    Baseprice = r.Baseprice,
                    Quantity = r.Quantity,
                    Categoryid = r.Categoryid,
                    Recipetypeid = r.Recipetypeid
                }).ToList();


                _context.Products.AddRange(products);
                _context.SaveChanges();
            }

            return Ok();
        }


        [HttpGet("ExportExcel")]
        public ActionResult ExportExcel()
        {
            var _productdata = GetProductData();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet(_productdata, "Products Records");
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Sample.xlsx");
                }
            }
        }

        [NonAction]
        private DataTable GetProductData()
        {
            DataTable dt = new DataTable();
            dt.TableName = "Products";
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Baseprice", typeof(decimal));
            dt.Columns.Add("Quantity", typeof(int));
            dt.Columns.Add("Categoryid", typeof(int));
            dt.Columns.Add("Recipetypeid", typeof(int));

            var _list = this._context.Products.ToList();
            if (_list.Count > 0)
            {
                _list.ForEach(item =>
                {
                    dt.Rows.Add(item.Name, item.Description, item.Baseprice, item.Quantity, item.Categoryid, item.Recipetypeid);
                });
            }

            return dt;
        }



        [HttpGet("SearchByName")]
        public ActionResult<List<Product>> SearchByName(string searchQuery, int idKategorije)
        {
            if (string.IsNullOrEmpty(searchQuery) && idKategorije == 0)
            {
                var products = _context.Products.OrderBy(p => p.Categoryid).ToList();
                if (products.Count == 0)
                {
                    return NotFound("Ne postoji artikal sa unesenim podacima");
                }
                return products;
            }
            else if (string.IsNullOrEmpty(searchQuery) && idKategorije != 0)
            {
                var products = _context.Products.Where(p => p.Categoryid == idKategorije).ToList();
                if (products.Count == 0)
                {
                    return NotFound("Ne postoji artikal sa unesenim podacima");
                }
                return products;
            }
            else if (!string.IsNullOrEmpty(searchQuery) && idKategorije != 0)
            {
                searchQuery = searchQuery == null ? string.Empty : searchQuery;

                var products = _context.Products
                    .Where(p => p.Name.ToLower().Contains(searchQuery.ToLower()) && p.Categoryid == idKategorije)
                    .OrderBy(p => p.Categoryid).ToList();

                if (products.Count == 0)
                {
                    return NotFound("Ne postoji artikal sa unesenim podacima");
                }
                return products;
            }
            else
            {

                var products = _context.Products
                    .Where(p => p.Name.ToLower().Contains(searchQuery.ToLower()))
                    .ToList();
                if (products.Count == 0)
                {
                    return NotFound("Ne postoji artikal sa unesenim podacima");
                }
                return products;
            }
        }


        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Productid)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'apoteka_dbContext.Products'  is null.");
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Productid }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.Productid == id)).GetValueOrDefault();
        }
    }
}
