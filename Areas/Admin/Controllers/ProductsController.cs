using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductsController(ApplicationDbContext context, IHostingEnvironment environment,IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            if (CheckFunctionAuthority(1, _context, _httpContextAccessor))
            {
                ViewBag.Types = await _context.ProductTypes.ToListAsync();
                return View(await _context.Products.ToListAsync());
            }
            else
            {
                return Redirect("/Admin/NotAuthorized/Index");
            }
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (CheckFunctionAuthority(5, _context, _httpContextAccessor))
            {
                if (id == null || _context.Products == null)
                {
                    return NotFound();
                }

                var products = await _context.Products
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (products == null)
                {
                    return NotFound();
                }

                return View(products);
            }
            else
            {
                return Redirect("/Admin/NotAuthorized/Index");
            }
        }

        // GET: Admin/Products/Create
        public async Task<IActionResult> Create()
        {
            if (CheckFunctionAuthority(6, _context, _httpContextAccessor))
            {
                ViewBag.Types = await _context.ProductTypes.ToListAsync();
                return View();
            }
            else
            {
                return Redirect("/Admin/NotAuthorized/Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,UrlHinhAnh,idProduct")] Products products, IFormFile FileAnh)
        {
            if (CheckFunctionAuthority(6, _context, _httpContextAccessor))
            {
                if (FileAnh.Length > 0)
                {
                    var file = Path.Combine(_environment.ContentRootPath, "wwwroot/assets/img", FileAnh.FileName);
                    using (var fileStream = new FileStream(file, FileMode.Create))
                    {
                        await FileAnh.CopyToAsync(fileStream);
                    }
                    products.UrlHinhAnh = Path.Combine("/assets/img/", FileAnh.FileName); ;
                    _context.Add(products);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                return View(products);
            }
            else
            {
                return Redirect("/Admin/NotAuthorized/Index");
            }
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (CheckFunctionAuthority(8, _context, _httpContextAccessor))
            {
                if (id == null || _context.Products == null)
                {
                    return NotFound();
                }

                var products = await _context.Products.FindAsync(id);
                if (products == null)
                {
                    return NotFound();
                }
                ViewBag.Types = await _context.ProductTypes.ToListAsync();
                return View(products);
            }
            else
            {
                return Redirect("/Admin/NotAuthorized/Index");
            }
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,UrlHinhAnh,idProduct")] Products products, IFormFile? FileAnh)
        {
            if (CheckFunctionAuthority(8, _context, _httpContextAccessor))
            {
                if (id != products.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        if (FileAnh != null)
                        {
                            var file = Path.Combine(_environment.ContentRootPath, "wwwroot/assets/img", FileAnh.FileName);
                            using (var fileStream = new FileStream(file, FileMode.Create))
                            {
                                await FileAnh.CopyToAsync(fileStream);
                            }
                            products.UrlHinhAnh = Path.Combine("/assets/img/", FileAnh.FileName); ;
                        }
                        _context.Update(products);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProductsExists(products.Id))
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

                return View(products);
            }
            else
            {
                return Redirect("/Admin/NotAuthorized/Index");
            }

        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (CheckFunctionAuthority(9, _context, _httpContextAccessor))
            {
                if (id == null || _context.Products == null)
                {
                    return NotFound();
                }

                var products = await _context.Products
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (products == null)
                {
                    return NotFound();
                }

                return View(products);
            }
            else
            {
                return Redirect("/Admin/NotAuthorized/Index");
            }
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (CheckFunctionAuthority(9, _context, _httpContextAccessor))
            {
                if (_context.Products == null)
                {
                    return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
                }
                var products = await _context.Products.FindAsync(id);
                if (products != null)
                {
                    _context.Products.Remove(products);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return Redirect("/Admin/NotAuthorized/Index");
            }
        }

        private bool ProductsExists(int id)
        {
          return _context.Products.Any(e => e.Id == id);
        }
    }
}
