using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IMS.Data;
using IMS.Models;

namespace IMS.Controllers
{
    public class ProductWarehousesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductWarehousesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductWarehouses
        public async Task<IActionResult> Index()
        {
            var productWarehouses = _context.ProductWarehouses
                .Include(pw => pw.Product)
                .Include(pw => pw.Warehouse);

            return View(await productWarehouses.ToListAsync());
        }

        // GET: ProductWarehouses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var productWarehouse = await _context.ProductWarehouses
                .Include(pw => pw.Product)
                .Include(pw => pw.Warehouse)
                .FirstOrDefaultAsync(m => m.ProductWarehouseId == id);

            if (productWarehouse == null) return NotFound();

            return View(productWarehouse);
        }

        // GET: ProductWarehouses/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "Name");
            return View();
        }

        // POST: ProductWarehouses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductWarehouse productWarehouse)
        {
            if (ModelState.IsValid)
            {
                productWarehouse.LastUpdated = DateTime.Now;
                _context.Add(productWarehouse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", productWarehouse.ProductId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "Name", productWarehouse.WarehouseId);
            return View(productWarehouse);
        }

        // GET: ProductWarehouses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var productWarehouse = await _context.ProductWarehouses.FindAsync(id);
            if (productWarehouse == null) return NotFound();

            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", productWarehouse.ProductId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "Name", productWarehouse.WarehouseId);
            return View(productWarehouse);
        }

        // POST: ProductWarehouses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductWarehouse productWarehouse)
        {
            if (id != productWarehouse.ProductWarehouseId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    productWarehouse.LastUpdated = DateTime.Now;
                    _context.Update(productWarehouse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ProductWarehouses.Any(e => e.ProductWarehouseId == productWarehouse.ProductWarehouseId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", productWarehouse.ProductId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "Name", productWarehouse.WarehouseId);
            return View(productWarehouse);
        }

        // GET: ProductWarehouses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var productWarehouse = await _context.ProductWarehouses
                .Include(pw => pw.Product)
                .Include(pw => pw.Warehouse)
                .FirstOrDefaultAsync(m => m.ProductWarehouseId == id);

            if (productWarehouse == null) return NotFound();

            return View(productWarehouse);
        }

        // POST: ProductWarehouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productWarehouse = await _context.ProductWarehouses.FindAsync(id);
            if (productWarehouse != null)
            {
                _context.ProductWarehouses.Remove(productWarehouse);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
