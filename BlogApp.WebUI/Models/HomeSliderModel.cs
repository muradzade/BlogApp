using BlogApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.WebUI.Models
{
    public class HomeSliderModel
    {
        public List<Blog> Home { get; set; }
        public List<Blog> Slider { get; set; }
    }
}
