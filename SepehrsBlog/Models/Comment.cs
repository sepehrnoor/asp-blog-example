using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SepehrsBlog.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Name { get; set; }
        public string Title { get; set;}
        public string Content { get; set; }
        public int PostId { get; set; }
        public DateTime Date { get; set; }
    }
}