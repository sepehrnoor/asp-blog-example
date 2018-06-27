using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SepehrsBlog.DAL;

namespace SepehrsBlog.Helpers
{
    public class AdminAttribute : ActionFilterAttribute //checks if the user is an admin
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);  //the base stuff, do not touch

            var myRequest = HttpContext.Current.Request; //c# object used for sending requests

            var token = myRequest.Cookies["LoginData"]["Token"];    //request the token

            var context = new BlogContext();    //for connecting to database
            var user = context.Users.SingleOrDefault(x => x.Token == token);    //get user with specified token

            if (user != null)   //if user is found
            {
                if (user.IsAdmin)   //if user is admin
                {
                    return; //proceed with method
                }
            }

            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new //runs if authentication failed (either user doesn't exist or isn't admin)
            {
                controller = "User",
                action = "Login"    //redirect to login page
            }));
        }

    }
}