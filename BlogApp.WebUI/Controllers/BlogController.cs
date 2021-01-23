using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.WebUI.Controllers
{
    public class BlogController : Controller
    {
        private IBlogRepository _blogRepository;
        private ICategoryRepository _categoryRepository;
        public BlogController(IBlogRepository repository,ICategoryRepository categoryRepository)
        {
            _blogRepository = repository;
            _categoryRepository = categoryRepository;
        }
        public IActionResult Index(int? id,string word)
        {
            if (!string.IsNullOrEmpty(word))
                return View(_blogRepository.GetAll().
                Where(blog => blog.isApproved == true && (EF.Functions.Like(blog.Title, "%%" + word + "%%") ||
                EF.Functions.Like(blog.Description,"%%"+word+"%%")||
                EF.Functions.Like(blog.Body, "%%" + word + "%%")
                ))
                );
            
            if (id ==null)
                return View(_blogRepository.GetAll().
                    Where(blog => blog.isApproved == true).
                    OrderByDescending(blog => blog.Date));

            else
                return View(_blogRepository.GetAll().
                    Where(blog => blog.isApproved == true && blog.CategoryId == id).
                    OrderByDescending(blog => blog.Date));
        }

        public IActionResult List()
        {
            return View(_blogRepository.GetAll());
        }

        [HttpGet]
        public IActionResult AddOrEdit(int? id)
        {
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
            if (id==null)
            {
                //yeni veri eklenecek
                return View(new Blog());
            }
            else
            {
                //guncelleme
                return View(_blogRepository.GetById((int)id));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEdit(Blog entity, IFormFile formFile)
        {
            if (ModelState.IsValid)
            {
                if(formFile!=null)
                {
                    var path = Path.Combine("wwwroot\\img", formFile.FileName);
                    using(var stream=new FileStream(path,FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    entity.Image = formFile.FileName;
                }

                _blogRepository.AddOrEditCategory(entity);
                TempData["message"] = $"{entity.Title} guncellendi";
                return RedirectToAction("List");
            }
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
            return View(entity);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(_blogRepository.GetById(id));
        }

        [HttpPost,ActionName("Delete")]
        public IActionResult DeleteConfirmed(int blogId)
        {
            var blogTitle = _blogRepository.GetById(blogId).Title;
            _blogRepository.DeleteBlog(blogId);
            TempData["message"] = $"{blogTitle} blog was deleted";
            return RedirectToAction("List");
        }

        public IActionResult Details(int id)
        {
            return View(_blogRepository.GetById(id));
        }


    }
}
