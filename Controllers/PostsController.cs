using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PagedList;
using Shop.Data;
using Shop.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Shop.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _environment;

        public PostsController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Posts
        public async Task<IActionResult> Index(string postTitle, int pageNumber = 1)
        {
            int pageSize = 9;
            ViewBag.PostTitle = postTitle;
            ViewBag.Types = await _context.PostTypes.ToListAsync();
            var PostList = _context.Posts.Where(
                                post => string.IsNullOrEmpty(postTitle) || post.Title.ToLower().Contains(postTitle.ToLower()) == true);
            ViewBag.NumberOfPage = (int)((PostList.Count() - 1) / pageSize) + 1;
            return View(PostList.ToPagedList(pageNumber, pageSize));
        }

        public async Task<IActionResult> Type(int? id, int pageNumber = 1)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }
            int pageSize = 9;
            ViewBag.Types = await _context.PostTypes.ToListAsync();
            var PostList = _context.Posts.Where(
                                    post => string.IsNullOrEmpty(id.ToString()) || post.idPost == id);
            ViewBag.NumberOfPage = (int)((PostList.Count() - 1) / pageSize) + 1;
            return View(PostList.ToPagedList(pageNumber, pageSize));
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
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

        /*// GET: Posts/Create
       public async Task<IActionResult> Create()
       {
           ViewBag.Types = await _context.PostTypes.ToListAsync();
           return View();
       }

       // POST: Posts/Create
       // To protect from overposting attacks, enable the specific properties you want to bind to.
       // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       [HttpPost]
       [ValidateAntiForgeryToken]

       public async Task<IActionResult> Create(Posts posts, IFormFile FileAnh)
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

       // GET: Posts/Edit/5
       public async Task<IActionResult> Edit(int? id)
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
           return View(posts);
       }

       // POST: Posts/Edit/5
       // To protect from overposting attacks, enable the specific properties you want to bind to.
       // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
       [HttpPost]
       [ValidateAntiForgeryToken]
       public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Date,CreatedBy,PostBodyContent,Image,Show,OrderNumber,idPost")] Posts posts)
       {
           if (id != posts.Id)
           {
               return NotFound();
           }

           if (ModelState.IsValid)
           {
               try
               {
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
           return View(posts);
       }

       // GET: Posts/Delete/5
       public async Task<IActionResult> Delete(int? id)
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

       // POST: Posts/Delete/5
       [HttpPost, ActionName("Delete")]
       [ValidateAntiForgeryToken]
       public async Task<IActionResult> DeleteConfirmed(int id)
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

       private bool PostsExists(int id)
       {
         return _context.Posts.Any(e => e.Id == id);
       }*/
    }
}
