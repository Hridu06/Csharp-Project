using System;
using BMS.Data;
using BMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BMS.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ShopController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index() => View(await _context.Shops.ToListAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var shop = await _context.Shops
                .Include(s => s.BookShops)
                    .ThenInclude(bs => bs.Book)
                .FirstOrDefaultAsync(s => s.ShopId == id);
            if (shop == null) return NotFound();
            return View(shop);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Location,Phone")] Shop shop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shop);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var shop = await _context.Shops.FindAsync(id);
            if (shop == null) return NotFound();
            return View(shop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShopId,Name,Location,Phone")] Shop shop)
        {
            if (id != shop.ShopId) return NotFound();
            if (ModelState.IsValid)
            {
                try { _context.Update(shop); await _context.SaveChangesAsync(); }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Shops.Any(e => e.ShopId == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(shop);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var shop = await _context.Shops.FirstOrDefaultAsync(s => s.ShopId == id);
            if (shop == null) return NotFound();
            return View(shop);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shop = await _context.Shops.FindAsync(id);
            if (shop != null) _context.Shops.Remove(shop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
