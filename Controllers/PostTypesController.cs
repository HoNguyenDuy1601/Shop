using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    public class PostTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PostTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.PostTypes.ToListAsync());
        }

        // GET: PostTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PostTypes == null)
            {
                return NotFound();
            }

            var postTypes = await _context.PostTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postTypes == null)
            {
                return NotFound();
            }

            return View(postTypes);
        }

        // GET: PostTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PostTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type")] PostTypes postTypes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(postTypes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(postTypes);
        }

        // GET: PostTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PostTypes == null)
            {
                return NotFound();
            }

            var postTypes = await _context.PostTypes.FindAsync(id);
            if (postTypes == null)
            {
                return NotFound();
            }
            return View(postTypes);
        }

        // POST: PostTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type")] PostTypes postTypes)
        {
            if (id != postTypes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postTypes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostTypesExists(postTypes.Id))
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
            return View(postTypes);
        }

        // GET: PostTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PostTypes == null)
            {
                return NotFound();
            }

            var postTypes = await _context.PostTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postTypes == null)
            {
                return NotFound();
            }

            return View(postTypes);
        }

        // POST: PostTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PostTypes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PostTypes'  is null.");
            }
            var postTypes = await _context.PostTypes.FindAsync(id);
            if (postTypes != null)
            {
                _context.PostTypes.Remove(postTypes);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostTypesExists(int id)
        {
          return _context.PostTypes.Any(e => e.Id == id);
        }
    }
}
