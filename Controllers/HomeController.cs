using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using wedding.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http; 



namespace wedding.Controllers
{
    public class HomeController : Controller
    {

        private MyContext dbContext;

        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid)
            {
                bool notUnique = dbContext.Users.Any(a => a.Email == newUser.Email);

                if(notUnique)
                {
                    ModelState.AddModelError("Email", "Email is in use");
                    return View("Index");
                }

                PasswordHasher<User> hasher = new PasswordHasher<User>();
                string hash = hasher.HashPassword(newUser, newUser.Password);
                newUser.Password = hash;


                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("UserId", newUser.UserId);
                return RedirectToAction("dashboard", "wedding");


            }
            return View("Index");
        }

        [HttpPost("login")]
        public IActionResult Login(LogUser user)
        {
            if(ModelState.IsValid)
            {
                User check = dbContext.Users.FirstOrDefault(u => u.Email == user.LogEmail);

                if(check == null)
                {
                    ModelState.AddModelError("LogEmail", "Invalid Email/Password");
                    return View("Index");
                }

                PasswordHasher<LogUser> hasher = new PasswordHasher<LogUser>();
                var result = hasher.VerifyHashedPassword(user, check.Password, user.LogPassword);

                if (result==0)
                {
                    ModelState.AddModelError("LogEmail", "Invalid Email/Password");
                    return View("Index");
                }

                HttpContext.Session.SetInt32("UserId", check.UserId);
                return RedirectToAction("dashboard", "wedding");

            }
            return View("Index");
        }


        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }


    }
}
