using IMS.Data;
using IMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IMS.Controllers
{
    public class ShipmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShipmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Shipments
        public async Task<IActionResult> Index()
        {
            var shipments = _context.Shipments
                .Include(s => s.Supplier)
                .Include(s => s.Product)
                .Include(s => s.Warehouse);
            return View(await shipments.ToListAsync());
        }

        // GET: Shipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var shipment = await _context.Shipments
                .Include(s => s.Supplier)
                .Include(s => s.Product)
                .Include(s => s.Warehouse)
                .FirstOrDefaultAsync(m => m.ShipmentId == id);

            if (shipment == null) return NotFound();

            return View(shipment);
        }

        // GET: Shipments/Create
        public IActionResult Create()
        {
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "CompanyName");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "Name");
            return View();
        }

        // POST: Shipments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Shipment shipment)
        {
            if (ModelState.IsValid)
            {
                // Compute total cost dynamically from Product price
                var product = await _context.Products.FindAsync(shipment.ProductId);
                if (product != null)
                {
                    shipment.TotalCost = shipment.Quantity * product.UnitPrice;
                }

                _context.Add(shipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "CompanyName", shipment.SupplierId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", shipment.ProductId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "Name", shipment.WarehouseId);
            return View(shipment);
        }

        // GET: Shipments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment == null) return NotFound();

            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "CompanyName", shipment.SupplierId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", shipment.ProductId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "Name", shipment.WarehouseId);
            return View(shipment);
        }

        // POST: Shipments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Shipment shipment)
        {
            if (id != shipment.ShipmentId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var product = await _context.Products.FindAsync(shipment.ProductId);
                    if (product != null)
                    {
                        shipment.TotalCost = shipment.Quantity * product.UnitPrice;
                    }

                    _context.Update(shipment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Shipments.Any(e => e.ShipmentId == shipment.ShipmentId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "CompanyName", shipment.SupplierId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", shipment.ProductId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "Name", shipment.WarehouseId);
            return View(shipment);
        }

        // GET: Shipments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var shipment = await _context.Shipments
                .Include(s => s.Supplier)
                .Include(s => s.Product)
                .Include(s => s.Warehouse)
                .FirstOrDefaultAsync(m => m.ShipmentId == id);

            if (shipment == null) return NotFound();

            return View(shipment);
        }

        // POST: Shipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment != null)
            {
                _context.Shipments.Remove(shipment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

