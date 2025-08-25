using BMS.Data;
using BMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BMS.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var orders = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.BookShop)
                    .ThenInclude(bs => bs.Book)
                .Include(o => o.BookShop)
                    .ThenInclude(bs => bs.Shop);

            return View("~/Views/Order/Index.cshtml", await orders.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.BookShop)
                    .ThenInclude(bs => bs.Book)
                .Include(o => o.BookShop)
                    .ThenInclude(bs => bs.Shop)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return NotFound();

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName");
            ViewData["BookShopId"] = new SelectList(
                _context.BookShops.Include(bs => bs.Book).Include(bs => bs.Shop),
                "BookShopId",
                "Display"
            );
            return View("~/Views/Order/Create.cshtml");
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,CustomerId,BookShopId,Quantity")] Order order)
        {
            if (ModelState.IsValid)
            {
                // calculate total price
                var bookShop = await _context.BookShops
                    .Include(bs => bs.Book)
                    .FirstOrDefaultAsync(bs => bs.BookShopId == order.BookShopId);

                if (bookShop == null)
                {
                    ModelState.AddModelError("", "Invalid BookShop selection.");
                    ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName", order.CustomerId);
                    ViewData["BookShopId"] = new SelectList(_context.BookShops.Include(bs => bs.Book).Include(bs => bs.Shop), "BookShopId", "Display", order.BookShopId);
                    return View(order);
                }

                order.TotalPrice = order.Quantity * bookShop.Book.Price;
                order.OrderDate = DateTime.UtcNow;

                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName", order.CustomerId);
            ViewData["BookShopId"] = new SelectList(_context.BookShops.Include(bs => bs.Book).Include(bs => bs.Shop), "BookShopId", "Display", order.BookShopId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName", order.CustomerId);
            ViewData["BookShopId"] = new SelectList(_context.BookShops.Include(bs => bs.Book).Include(bs => bs.Shop), "BookShopId", "Display", order.BookShopId);

            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerId,BookShopId,Quantity")] Order order)
        {
            if (id != order.OrderId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var bookShop = await _context.BookShops.Include(bs => bs.Book).FirstOrDefaultAsync(bs => bs.BookShopId == order.BookShopId);
                    order.TotalPrice = order.Quantity * bookShop.Book.Price;
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Orders.Any(e => e.OrderId == order.OrderId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "FullName", order.CustomerId);
            ViewData["BookShopId"] = new SelectList(_context.BookShops.Include(bs => bs.Book).Include(bs => bs.Shop), "BookShopId", "Display", order.BookShopId);

            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.BookShop)
                    .ThenInclude(bs => bs.Book)
                .Include(o => o.BookShop)
                    .ThenInclude(bs => bs.Shop)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return NotFound();

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

