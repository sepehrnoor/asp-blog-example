using SepehrsBlog.DAL;
using SepehrsBlog.Helpers;
using SepehrsBlog.Models;
using SepehrsBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SepehrsBlog.Controllers
{
    [Authenticated]
    public class PostController : Controller
    {

        // GET: Post
        public ActionResult Index(int postId) // för enskilda poster
        {
            var context = new BlogContext();
            if (context.Posts != null)
            {
                if (context.Posts.Count() > 0)
                {
                    var dbPost = context.Posts.Single(x => x.PostId == postId);

                    var posterName = context.Users.Single(x => x.UserId == dbPost.UserId).Username;

                    var viewModel = new PostViewModel();

                    viewModel.Post = dbPost;
                    viewModel.Poster = posterName;

                    IEnumerable<Comment> queryComments =
                        from comment in context.Comments
                        where comment.PostId == postId
                        select comment;

                    viewModel.Comments = new List<Comment>();
                    foreach (Comment c in queryComments)
                    {
                        viewModel.Comments.Add(c);
                    }

                    return View(viewModel);

                }
            }

            return RedirectToAction("Index", "Home");
        }

        //tar bort en post
        public ActionResult RemovePost(int postId)
        {
            User loggedUser = Session["LoginData"] as User;

            var context = new BlogContext();

            var postUserId = context.Posts.Single(x => x.PostId == postId).UserId;

            if (loggedUser.UserId == postUserId || loggedUser.IsAdmin)
            {
                if (context.Comments != null)
                {
                    foreach (Comment c in context.Comments.ToList())
                    {
                        if (c.PostId == postId) context.Comments.Remove(c);
                    }
                }

                var postToDelete = context.Posts.Single(x => x.PostId == postId);
                context.Posts.Remove(postToDelete);
                context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", new { postId = postId });
        }

        //tar bort en kommentar, bara admins
        [Admin]
        public ActionResult RemoveComment(int commentId)
        {
            var context = new BlogContext();
            var commentToDelete = context.Comments.Single(x => x.CommentId == commentId);
            int redirectId = commentToDelete.PostId;
            context.Comments.Remove(commentToDelete);
            context.SaveChanges();

            return RedirectToAction("Index", "Post", new { postId = redirectId });
        }

        //sparar en kommentar
        public ActionResult SaveComment(PostViewModel viewModel)
        {
            var context = new BlogContext();

            int postId = viewModel.Post.PostId;

            viewModel.NewComment.PostId = postId;

            var user = Session["LoginData"] as User;

            string posterName = user.Username;

            viewModel.NewComment.Name = posterName;
            viewModel.NewComment.Date = DateTime.Now.ToUniversalTime();

            context.Comments.Add(viewModel.NewComment);

            context.SaveChanges();

            return RedirectToAction("Index", "Post", new { postId = viewModel.Post.PostId });
        }

        //returnerar vyn för att göra en ny post
        public ActionResult NewPost()
        {
            return View();
        }

        //sparar en post
        public ActionResult SavePost(Post post)
        {
            var context = new BlogContext();

            var user = Session["LoginData"] as User;

            post.UserId = user.UserId;
            post.Date = DateTime.Now.ToUniversalTime();

            context.Posts.Add(post);
            context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}