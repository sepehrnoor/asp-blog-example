using SepehrsBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SepehrsBlog.ViewModels
{
    public class PostViewModel
    {
        public Post Post { get; set; }
        public string Poster { get; set; }
        public Comment NewComment { get; set; }
        public List<Comment> Comments { get; set; }
    }
}