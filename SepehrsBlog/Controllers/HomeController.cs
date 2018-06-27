using SepehrsBlog.DAL;
using SepehrsBlog.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SepehrsBlog.Controllers
{
    [Authenticated]
    public class HomeController : Controller
    {
        //returnerar framsidan med posterna
        public ActionResult Index()
        {
            var context = new BlogContext();
            var posts = context.Posts.OrderByDescending(x => x.PostId);
            
            return View(posts);
        }
        

    }

    
}