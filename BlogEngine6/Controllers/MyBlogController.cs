using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlogEngine6.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Infrastructure;
using BlogEngine6.Models.ViewModels;
using PagedList;

namespace BlogEngine6.Controllers
{
    public class MyBlogController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MyBlog
        [Authorize]
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {

            // Allow for paging and search filter
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = (String.IsNullOrEmpty(searchString) ? null : searchString);

            var userID = User.Identity.GetUserId();
            var blogs = db.Blogs.Include(b => b.User).Where(b => b.UserID == userID);

            // Add search string to query
            if (!String.IsNullOrEmpty(searchString))
            {
                blogs = blogs.Where(b => b.User.UserName.Contains(searchString) || b.Title.Contains(searchString));
            }


            blogs = blogs.OrderByDescending(b => b.PostDate);

            List<ViewBlogViewModel> blogList = blogs.AsEnumerable()
                          .Select(o => new ViewBlogViewModel
                          {
                              BlogID = o.BlogID,
                              UserName = o.User.UserName,
                              PostDate = o.PostDate,
                              Title = o.Title,
                              Content = o.Content
                          }).ToList();

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(blogList.ToPagedList(pageNumber, pageSize));
        }

        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Blog blog = db.Blogs.Find(id);
            var userID = User.Identity.GetUserId();

            if (blog == null)
            {
                return HttpNotFound();
            }
            else if (blog.UserID != userID)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            ViewBlogViewModel blogViewModel = new ViewBlogViewModel{
                                        BlogID = blog.BlogID,
                                        UserName = blog.User.UserName,
                                        PostDate = blog.PostDate,
                                        Title = blog.Title,
                                        Content = blog.Content
                                    };

            return View(blogViewModel);
        }


        // GET: MyBlog/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

       
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Title,Content")] CreateBlogViewModel blog)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Blog newBlog = new Blog();

                    newBlog.Title = blog.Title;
                    newBlog.Content = blog.Content;
                    newBlog.UserID = User.Identity.GetUserId();
                    newBlog.PostDate = DateTime.Now;
                    db.Blogs.Add(newBlog);

                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            return View(blog);
        }


        // GET: MyBlog/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Blog blog = db.Blogs.Find(id);
            var userID = User.Identity.GetUserId();

            if (blog == null)
            {
                return HttpNotFound();
            }
            else if (blog.UserID != userID)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBlogViewModel blogViewModel = new ViewBlogViewModel
            {
                BlogID = blog.BlogID,
                UserName = blog.User.UserName,
                PostDate = blog.PostDate,
                Title = blog.Title,
                Content = blog.Content
            };

            return View(blogViewModel);
        }
        
        [Authorize]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userID = User.Identity.GetUserId();
            var blogToUpdate = db.Blogs.Find(id);

            if (blogToUpdate.UserID != userID)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (TryUpdateModel(blogToUpdate, "",
               new string[] { "Title", "Content" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(blogToUpdate);
        }
    
   

        // POST: MyBlog/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var userID = User.Identity.GetUserId();
            Blog blog = await db.Blogs.FindAsync(id);

            if(blog.UserID == userID)
            {
                db.Blogs.Remove(blog);
                await db.SaveChangesAsync();

                return Json(new { success = true, blogId = id });
            }

            return Json(new { success = false });

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
