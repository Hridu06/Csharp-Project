using System;
using BMS.Data;
using BMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BMS.Controllers
{
    public class BookShopsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookShopsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BookShops
        public async Task<IActionResult> Index()
        {
            var bookShops = _context.BookShops
                .Include(bs => bs.Book)
                .Include(bs => bs.Shop);
            return View(await bookShops.ToListAsync());
        }

        // GET: BookShops/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var bookShop = await _context.BookShops
                .Include(bs => bs.Book)
                .Include(bs => bs.Shop)
                .FirstOrDefaultAsync(m => m.BookShopId == id);

            if (bookShop == null) return NotFound();

            return View(bookShop);
        }

        // GET: BookShops/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "Title");
            ViewData["ShopId"] = new SelectList(_context.Shops, "ShopId", "Name");
            return View();
        }

        // POST: BookShops/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookShopId,BookId,ShopId,Quantity")] BookShop bookShop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookShop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "Title", bookShop.BookId);
            ViewData["ShopId"] = new SelectList(_context.Shops, "ShopId", "Name", bookShop.ShopId);
            return View(bookShop);
        }

        // GET: BookShops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var bookShop = await _context.BookShops.FindAsync(id);
            if (bookShop == null) return NotFound();

            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "Title", bookShop.BookId);
            ViewData["ShopId"] = new SelectList(_context.Shops, "ShopId", "Name", bookShop.ShopId);
            return View(bookShop);
        }

        // POST: BookShops/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookShopId,BookId,ShopId,Quantity")] BookShop bookShop)
        {
            if (id != bookShop.BookShopId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookShop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.BookShops.Any(e => e.BookShopId == bookShop.BookShopId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "Title", bookShop.BookId);
            ViewData["ShopId"] = new SelectList(_context.Shops, "ShopId", "Name", bookShop.ShopId);
            return View(bookShop);
        }

        // GET: BookShops/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var bookShop = await _context.BookShops
                .Include(bs => bs.Book)
                .Include(bs => bs.Shop)
                .FirstOrDefaultAsync(m => m.BookShopId == id);

            if (bookShop == null) return NotFound();

            return View(bookShop);
        }

        // POST: BookShops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookShop = await _context.BookShops.FindAsync(id);
            if (bookShop != null)
            {
                _context.BookShops.Remove(bookShop);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

