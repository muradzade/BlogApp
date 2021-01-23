using BlogApp.Data.Abstract;
using BlogApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfBlogRepository : IBlogRepository
    {
        private BlogContext _context;
        public EfBlogRepository(BlogContext context)
        {
            _context = context;
        }
        public void AddBlog(Blog entity)
        {
            _context.Blogs.Add(entity);
            _context.SaveChanges();
        }

        public void AddOrEditCategory(Blog entity)
        {
            if (entity.BlogId == 0)
            {
                entity.Date = DateTime.Now;
                _context.Blogs.Add(entity);
                _context.SaveChanges();
            }
            else
            {
                var blog = GetById(entity.BlogId);
                if (blog != null)
                {
                    blog.Title = entity.Title;
                    blog.Description = entity.Description;
                    blog.Body = entity.Body;
                    blog.CategoryId = entity.CategoryId;
                    blog.isApproved = entity.isApproved;
                    blog.isHome = entity.isHome;
                    blog.isSlider = entity.isSlider;
                    if(entity.Image!=null)
                        blog.Image = entity.Image;
                    _context.SaveChanges();
                }
            }
        }

        public void DeleteBlog(int blogId)
        {

            var blog = _context.Blogs.FirstOrDefault(b => b.BlogId == blogId);
            if(blog!=null)
            {
                _context.Blogs.Remove(blog);
                _context.SaveChanges();
            }
           
        }

        public IQueryable<Blog> GetAll()
        {
            return _context.Blogs;
        }

        public Blog GetById(int blogId)
        {
            return _context.Blogs.FirstOrDefault(b => b.BlogId == blogId);
        }

        public void UpdateBlog(Blog entity)
        {
            var blog = GetById(entity.BlogId);
            if(blog!=null)
            {
                blog.Title = entity.Title;
                blog.Description = entity.Description;
                blog.Body = entity.Body;
                blog.Image = entity.Image;

                _context.SaveChanges();
            }
        }
    }
}
