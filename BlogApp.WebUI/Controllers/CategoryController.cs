using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository repository)
        {
            _categoryRepository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            return View(_categoryRepository.GetAll());
        }

        [HttpGet]
        public IActionResult AddOrEditCategory(int? id)
        {
            if(id == null)
            {
                return View(new Category());
            }
            else
            {
                return View(_categoryRepository.GetById((int)id));
            }

        }

        [HttpPost]
        public IActionResult AddOrEditCategory(Category entity)
        {
            if(ModelState.IsValid)
            {
                _categoryRepository.AddOrEditCategory(entity);
                TempData["message"] = $"{entity.Name} eklendi";
                return RedirectToAction("List");
            }

            return View(entity);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(_categoryRepository.GetById(id));
        }

        [HttpPost,ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {

            TempData["message"] = _categoryRepository.GetById(id).Name + " named category deleted\n";
            string blogs=_categoryRepository.DeleteCategory(id);
            TempData["message"] += blogs;
            return RedirectToAction("List");
        }
    }
}