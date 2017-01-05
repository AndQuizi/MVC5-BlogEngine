using BlogEngine6.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using BlogEngine6.Models.ViewModels;
using AutoMapper;

namespace BlogEngine6.Controllers
{
    public class BlogsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Blogs
        public ActionResult Index(string author, string currentFilter, string searchString, int? page)
        {

            ViewBag.CurrentAuthor = author;

            // Allow for paging and search filter
            if (searchString != null){
                page = 1;
            }
            else{
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = (String.IsNullOrEmpty(searchString) ? null : searchString);

            var blogs = db.Blogs.Include(b => b.User);

            // Add search string to query
            if (!String.IsNullOrEmpty(searchString)){
                blogs = blogs.Where(b => b.User.UserName.Contains(searchString) || b.Title.Contains(searchString));
            }

            // Add author to query
            if (!String.IsNullOrEmpty(author)){
                blogs = blogs.Where(b => b.User.UserName == author);
            }

            blogs = blogs.OrderByDescending(b => b.PostDate);

            List<ViewBlogViewModel> blogList = blogs.AsEnumerable()
                          .Select(b => new ViewBlogViewModel
                          {
                              BlogID = b.BlogID,
                              UserName = b.User.UserName,
                              PostDate = b.PostDate,
                              Title = b.Title,
                              Content = b.Content
                          }).ToList();


            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(blogList.ToPagedList(pageNumber, pageSize));

        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Blog blog = db.Blogs.Find(id);

            if (blog == null)
            {
                return HttpNotFound();
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

        public async Task<ActionResult> DetailsJSON(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Blog blog = await db.Blogs.FindAsync(id);

            if (blog == null)
            {
                return HttpNotFound();
            }

            var blogJSON = new Dictionary<string, string>();

            blogJSON["Title"] = blog.Title;
            blogJSON["PostDate"] = blog.PostDate.ToString();

            return Json(blogJSON, JsonRequestBehavior.AllowGet);
        }


        [ChildActionOnly]
        public ActionResult FeaturedPosts()
        {
            var featuredBlogs = db.FeaturedBlogs
                                .OrderBy(b => b.FeatureDate).Take(5);

            List<FeaturedPostViewModel> blogList = featuredBlogs.AsEnumerable()
                          .Select(b => new FeaturedPostViewModel
                          {
                              BlogID = b.BlogID,
                              Title = b.Blog.Title,
                              Author = b.Blog.User.UserName
                          }).ToList();

            ViewBag.Title = "Editors’ picks";
            ViewBag.Description = "Stories that matter.";

            return PartialView("SidebarPostsPartial", blogList);
        }

        [ChildActionOnly]
        public ActionResult RandomPosts()
        {
            // Get random blog based on generated GUID
            var featuredBlogs = db.Blogs
                                .OrderBy(b => b.PostDate)
                                .OrderBy(r => Guid.NewGuid()).Take(5);

            List<FeaturedPostViewModel> blogList = featuredBlogs.AsEnumerable()
                          .Select(b => new FeaturedPostViewModel
                          {
                              BlogID = b.BlogID,
                              Title = b.Title,
                              Author = b.User.UserName
                          }).ToList();

            ViewBag.Title = "Reading roulette";
            ViewBag.Description = "The new variety hour.";

            return PartialView("SidebarPostsPartial", blogList);
        }

    }
}