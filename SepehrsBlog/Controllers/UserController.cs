using SepehrsBlog.DAL;
using SepehrsBlog.Helpers;
using SepehrsBlog.Models;
using SepehrsBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SepehrsBlog.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [NotAuthenticated]
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            var context = new BlogContext(); //för att komma åt databasen

            string username = model.Username;   //Hämta username
            string hashedPW = toHashString(model.Password); //Hämta password som en hashstring

            //se om det går att hitta en user med den username och password som vi har angett
            var dbUser = context.Users.SingleOrDefault(x => x.Username.ToLower() == username.ToLower() && x.Password == hashedPW);

            if (dbUser != null) //Om vi har hittat någon, vi ger dem en ny token och släpper in dem
            {
                var chars = "ABCDEFGHIJKLMNOPQRSTUVVWXYZ1234567890";    //bostäver som man kan välja mellan
                var random = new Random();  //för att slumpa
                var result = new string(
                    Enumerable.Repeat(chars, 16)    //16 tecken
                    .Select(s => s[random.Next(s.Length)])  //slumpa en av bokstäverna i chars
                    .ToArray());    //Vi gör en char array
                dbUser.Token = result;  //tilldela user token den token som vi nyss genererade
                context.SaveChanges();  //spara det vi har gjort till usern

                //gör en ny session med hela användaren sparad i LoginData
                Session["LoginData"] = dbUser;
                Response.Cookies["LoginData"]["Token"] = dbUser.Token; 
                Response.Cookies["LoginData"].Expires = DateTime.Now.AddMinutes(10); //token går ut om 10 min
                return RedirectToAction("Index", "Home");
            }

            //Om man inte hittar användaren
            else
            {
                ModelState.AddModelError("Error", "Invalid username or password");
                return View(model);
            }
            
        }
        
        public ActionResult Logout()
        {
            Session["LoginData"] = null; //ta bort sessionen
            Response.Cookies["LoginData"].Expires = DateTime.Now.AddDays(-1); //tvingar cookies att expira
            return RedirectToAction("Login", "User");
        }
        
        [HttpPost]
        public ActionResult Register(LoginViewModel model)
        {
            var context = new BlogContext();

            //hämta username från sidan
            var myUser = new User();
            myUser.Username = model.Username;
            
            //hämta pass från sidan
            string hashedPassword = toHashString(model.Password);
            myUser.Password = hashedPassword;

            //ingen ska vara admin!
            myUser.IsAdmin = false;

            //om det inte finns någon med samma username
            if (!context.Users.Any(x => x.Username == myUser.Username))
            {
                context.Users.Add(myUser); //lägg till user
                context.SaveChanges();  //uppdatera
            }

            else
            {
                ModelState.AddModelError("Error","Username already in use");
                return View(model);
            }

            return RedirectToAction("Login", "User");
        }

        [NotAuthenticated]
        public ActionResult Register()
        {
            return View();
        }

        string toHashString(string str)
        {
            //Calculate hash for hardcoded password
            if (str == null) return "";
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(str)); //get hash of "password"
                string hashedString = Encoding.Unicode.GetString(hash);  //get password hash bytes as string
                return hashedString;
            }
        }
        
    }
}