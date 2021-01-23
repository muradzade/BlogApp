using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IBlogRepository _blogRepository;
        public HomeController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public IActionResult Index()
        {
            var query = _blogRepository.GetAll().Where(blog => blog.isApproved == true);
            HomeSliderModel blogs = new HomeSliderModel();
            blogs.Home = query.Where(blog => blog.isHome == true).ToList();
            blogs.Slider = query.Where(blog => blog.isSlider == true).ToList();
            return View(blogs);
        }

        public IActionResult List()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }
    }
}