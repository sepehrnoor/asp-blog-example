using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SepehrsBlog.DAL;

namespace SepehrsBlog.Helpers
{
    public class AuthenticatedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);  //the base stuff, do not touch

            if (HttpContext.Current.Session["LoginData"] == null)   //if no login data is found for current session
            {

                var myRequest = HttpContext.Current.Request; //c# object used for sending requests

                if (myRequest.Cookies["LoginData"] != null && myRequest.Cookies["LoginData"]["Token"] != null) //if a cookie with a token is found
                {
                    var token = myRequest.Cookies["LoginData"]["Token"];    //get the token

                    var context = new BlogContext();
                    var user = context.Users.SingleOrDefault(x => x.Token == token);    //get user with specified token

                    if (user != null)   //if user is found
                    {
                        HttpContext.Current.Session["LoginData"] = user;    //log the user in
                        return; //then proceed to the requested page
                    }
                }



                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new //runs if authentication failed 
                {
                    controller = "User",
                    action = "Login"    //redirect to login page
                }));

            }


        }

    }
}