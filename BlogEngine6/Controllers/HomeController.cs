﻿using BlogEngine6.Models;
using BlogEngine6.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogEngine6.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {

            var blogs = db.FeaturedBlogs
                                .OrderBy(b => b.FeatureDate).Take(10);

            List<ViewBlogViewModel> blogList = blogs.AsEnumerable()
                          .Select(b => new ViewBlogViewModel
                          {
                              BlogID = b.BlogID,
                              UserName = b.Blog.User.UserName,
                              PostDate = b.Blog.PostDate,
                              Title = b.Blog.Title,
                              Content = b.Blog.Content
                          }).ToList();

            return View(blogList);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult WelcomeMessage()
        {
            return PartialView("SidebarWelcomePartial");
        }

    }
}