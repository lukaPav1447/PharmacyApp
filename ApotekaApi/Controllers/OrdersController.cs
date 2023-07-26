using ApotekaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApotekaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly apoteka_dbContext _context;

        public OrdersController(apoteka_dbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            return await _context.Orders.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        [HttpGet("GetAllOrdersWithProducts")]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrdersWithProducts()
        {
            var ordersWithProducts = await _context.Orders.AsNoTracking()
                .Include(o => o.Orderitems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();

            if (ordersWithProducts.Count == 0)
            {
                return NotFound();
            }

            return ordersWithProducts;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Orderid)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        //// POST: api/Orders
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Order>> PostOrder(Order order)
        //{
        //    if (_context.Orders == null)
        //    {
        //        return Problem("Entity set 'apoteka_dbContext.Orders'  is null.");
        //    }
        //    _context.Orders.Add(order);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetOrder", new { id = order.Orderid }, order);
        //}

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderRequest orderRequest)
        {
            try
            {
                if (orderRequest == null || orderRequest.Orderitems == null || orderRequest.Orderitems.Count == 0)
                {
                    return BadRequest("Narudžba mora sadržavati proizvode i količine.");
                }

                var order = new Order
                {
                    Orderdate = orderRequest.Orderdate,
                    Totalamount = orderRequest.Totalamount
                };

                foreach (var orderItemRequest in orderRequest.Orderitems)
                {
                    var product = await _context.Products.FindAsync(orderItemRequest.Productid);
                    if (product == null)
                    {
                        return NotFound($"Proizvod s ID-em {orderItemRequest.Productid} nije pronađen.");
                    }

                    var orderItem = new Orderitem
                    {
                        Order = order,
                        Product = product,
                        Quantity = orderItemRequest.Quantity,
                        Price = orderItemRequest.Price
                    };

                    _context.Orderitems.Add(orderItem);
                }

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetOrder", new { id = order.Orderid }, order);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while creating the order: {ex.Message}");
                return StatusCode(500, "An error occurred while processing the request.");
            }

        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.Orderid == id)).GetValueOrDefault();
        }
    }
    public class OrderRequest
    {
        public DateTime Orderdate { get; set; }
        public decimal Totalamount { get; set; }
        public List<OrderitemRequest> Orderitems { get; set; }
    }

    public class OrderitemRequest
    {
        public int Productid { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
