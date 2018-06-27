using SepehrsBlog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SepehrsBlog.DAL
{
    public class BlogContext : DbContext
    {
        public BlogContext() : base("MyConnectionString")
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
    }
}