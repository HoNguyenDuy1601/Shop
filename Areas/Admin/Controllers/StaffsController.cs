using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Shop.Areas.Admin.Controllers;

namespace Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class StaffsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StaffsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }


        // GET: Admin/Staffs
        public async Task<IActionResult> Index()
        {
            if(CheckFunctionAuthority(1, _context, _httpContextAccessor))
            {
                return View(await _context.Staffs.ToListAsync());
            }
            else
            {
                return Redirect("/Admin/NotAuthorized/Index");
            }
        }


        // GET: Admin/Staffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (CheckFunctionAuthority(5, _context, _httpContextAccessor))
            {
                if (id == null || _context.Staffs == null)
                {
                    return NotFound();
                }

                var staffs = await _context.Staffs
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (staffs == null)
                {
                    return NotFound();
                }

                return View(staffs);
            }
            else
            {
                return Redirect("/Admin/NotAuthorized/Index");
            }

        }

        // GET: Admin/Staffs/Create
        public IActionResult Create()
        {
            if (CheckFunctionAuthority(6, _context, _httpContextAccessor))
            {
                return View();
            }
            else
            {
                return Redirect("/Admin/NotAuthorized/Index");
            }
        }

        // POST: Admin/Staffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Password,Name,PhoneNumber,DateOfBirth,Address,idType,Position")] Staffs staffs)
        {
            if (CheckFunctionAuthority(6, _context, _httpContextAccessor))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(staffs);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(staffs);
            }
            else
            {
                return Redirect("/Admin/NotAuthorized/Index");
            }
        }

        // GET: Admin/Staffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (CheckFunctionAuthority(8, _context, _httpContextAccessor))
            {
                if (id == null || _context.Staffs == null)
                {
                    return NotFound();
                }

                var staffs = await _context.Staffs.FindAsync(id);
                if (staffs == null)
                {
                    return NotFound();
                }
                return View(staffs);
            }
            else
            {
                return Redirect("/Admin/NotAuthorized/Index");
            }
        }

        // POST: Admin/Staffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password,Name,PhoneNumber,DateOfBirth,Address,idType,Position")] Staffs staffs)
        {
            if (CheckFunctionAuthority(8, _context, _httpContextAccessor))
            {
                if (id != staffs.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(staffs);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!StaffsExists(staffs.Id))
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
                return View(staffs);
            }
            else
            {
                return Redirect("/Admin/NotAuthorized/Index");
            }

        }

        // GET: Admin/Staffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (CheckFunctionAuthority(9, _context, _httpContextAccessor))
            {
                if (id == null || _context.Staffs == null)
                {
                    return NotFound();
                }

                var staffs = await _context.Staffs
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (staffs == null)
                {
                    return NotFound();
                }
                return View(staffs);
            }
            else
            {
                return Redirect("/Admin/NotAuthorized/Index");
            }
        }

        // POST: Admin/Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (CheckFunctionAuthority(9, _context, _httpContextAccessor))
            {
                if (_context.Staffs == null)
                {
                    return Problem("Entity set 'ApplicationDbContext.Staffs'  is null.");
                }
                var staffs = await _context.Staffs.FindAsync(id);
                if (staffs != null)
                {
                    _context.Staffs.Remove(staffs);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return Redirect("/Admin/NotAuthorized/Index");
            }
        }   

        private bool StaffsExists(int id)
        {
          return _context.Staffs.Any(e => e.Id == id);
        }
    }
}
