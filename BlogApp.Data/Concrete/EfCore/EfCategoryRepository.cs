using BlogApp.Data.Abstract;
using BlogApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfCategoryRepository : ICategoryRepository
    {
        private BlogContext _context;
        public EfCategoryRepository(BlogContext context)
        {
            _context = context;
        }
        public void AddCategory(Category entity)
        {
            _context.Categories.Add(entity);
            _context.SaveChanges();
        }

        public void AddOrEditCategory(Category entity)
        {
            if(entity.CategoryId==0)
            {
                _context.Categories.Add(entity);
            }
            else
            {
                var category = GetById(entity.CategoryId);
                if (category != null)
                {
                    category.Name = entity.Name;
                }
            }
            _context.SaveChanges();
        }

        public string DeleteCategory(int categoryId)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == categoryId);
            string sonuc = "";
            if(category!=null)
            {
                
                IQueryable<Blog> blogsForDelete = _context.Blogs.Where(b => b.CategoryId == categoryId);
                
                foreach (var item in blogsForDelete)
                {
                    sonuc = sonuc + item.Title + " named blog deleted\n";
                    _context.Blogs.Remove(item);
                }
                _context.Categories.Remove(category);
                _context.SaveChanges();
                
            }
            return sonuc;
        }

        public IQueryable<Category> GetAll()
        {
            return _context.Categories;
        }

        public Category GetById(int categoryId)
        {
            return _context.Categories.FirstOrDefault(c => c.CategoryId == categoryId);
        }

        public void UpdateCategory(Category entity)
        {
            var category = GetById(entity.CategoryId);
            if(category!=null)
            {
                category.Name = entity.Name;
                _context.SaveChanges();
            }   
        }
    }
}
