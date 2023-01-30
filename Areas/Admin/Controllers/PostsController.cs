using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PostsController(ApplicationDbContext context, IHostingEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Posts
        public async Task<IActionResult> Index(string postTitle)
        {
            if (CheckFunctionAuthority(1, _context, _httpContextAccessor))
            {
                ViewBag.Types = await _context.PostTypes.ToListAsync();
                List<Posts> PostList = await _context.Posts.Where(
                                        post => string.IsNullOrEmpty(postTitle) || post.Title.ToLower().Contains(postTitle.ToLower()) == true)
                                        .ToListAsync();
                return View(PostList);
            }
            else
            {
                return Redirect("/Admin/NotAuthorized/Index");
            }
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (CheckFunctionAuthority(5, _context, _httpContextAccessor))
            {
                if (id == null || _context.Posts == null)
                {
                    return NotFound();
                }

                var posts = await _context.Posts
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (posts == null)
                {
                    return NotFound();
                }

                return View(posts);
            }
            else
            {
                return Redirect("/Admin/NotAuthorized/Index");
            }
        }

        // GET: Posts/Create
        public async Task<IActionResult> Create()
        {
            if (CheckFunctionAuthority(6, _context, _httpContextAccessor))
            {
                ViewBag.Types = await _context.PostTypes.ToListAsync();
                return View();
            }
            else
            {
                return Redirect("/Admin/NotAuthorized/Index");
            }
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Posts posts, IFormFile FileAnh)
        {
            if (CheckFunctionAuthority(6, _context, _httpContextAccessor))
            {
                if (FileAnh.Length > 0)
                {
                    var file = Path.Combine(_environment.ContentRootPath, "wwwroot/assets/img/posts/", FileAnh.FileName);
                    using (var fileStream = new FileStream(file, FileMode.Create))
                    {
                        await FileAnh.CopyToAsync(fileStream);
                    }
                    posts.Image = Path.Combine("/assets/img/posts/", FileAnh.FileName); ;
                    _context.Add(posts);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(posts);
            }
            else
            {
                return Redirect("/Admin/NotAuthorized/Index");
            }
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (CheckFunctionAuthority(8, _context, _httpContextAccessor))
            {
                if (id == null || _context.Posts == null)
                {
                    return NotFound();
                }

                var posts = await _context.Posts.FindAsync(id);
                if (posts == null)
                {
                    return NotFound();
                }
                ViewBag.Types = await _context.PostTypes.ToListAsync();
                ViewBag.ImgLink = Path.Combine(_environment.ContentRootPath, "wwwroot/assets/img/posts/", posts.Image);
                return View(posts);
            }
            else
            {
                return Redirect("/Admin/NotAuthorized/Index");
            }
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Date,CreatedBy,PostBodyContent,Image,Show,OrderNumber,idPost")] Posts posts, IFormFile? FileAnh)
        {
            if (CheckFunctionAuthority(8, _context, _httpContextAccessor))
            {
                if (id != posts.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        if (FileAnh != null)
                        {
                            var file = Path.Combine(_environment.ContentRootPath, "wwwroot/assets/img/posts/", FileAnh.FileName);
                            using (var fileStream = new FileStream(file, FileMode.Create))
                            {
                                await FileAnh.CopyToAsync(fileStream);
                            }
                            posts.Image = Path.Combine("/assets/img/posts/", FileAnh.FileName); ;
                        }
                        _context.Update(posts);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PostsExists(posts.Id))
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
                ViewBag.Types = await _context.PostTypes.ToListAsync();
                return View(posts);
            }
            else
            {
                return Redirect("/Admin/NotAuthorized/Index");
            }

        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (CheckFunctionAuthority(9, _context, _httpContextAccessor))
            {

                if (id == null || _context.Posts == null)
                {
                    return NotFound();
                }

                var posts = await _context.Posts
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (posts == null)
                {
                    return NotFound();
                }

                return View(posts);
            }
            else
            {
                return Redirect("/Admin/NotAuthorized/Index");
            }
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (CheckFunctionAuthority(9, _context, _httpContextAccessor))
            {
                if (_context.Posts == null)
                {
                    return Problem("Entity set 'ApplicationDbContext.Posts'  is null.");
                }
                var posts = await _context.Posts.FindAsync(id);
                if (posts != null)
                {
                    _context.Posts.Remove(posts);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return Redirect("/Admin/NotAuthorized/Index");
            }
        }

        private bool PostsExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
