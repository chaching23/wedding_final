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
    [Route("wedding")]
    // localhost:5000/wedding
    public class WeddingController : Controller
    {
        private int? SessionUser
        {
            get { return HttpContext.Session.GetInt32("UserId"); }
            set { HttpContext.Session.SetInt32("UserId", (int)value); }
        }
        private MyContext dbContext;
        public WeddingController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var x = dbContext.Weddings
                .Include(p => p.Creator)
                .Include(p => p.Answer)
                // ThenInclude() to get user name from user_id
                    .ThenInclude(v => v.yes)
                .ToList();
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            return View(x);

        }



        [HttpGet("new")]
        public IActionResult New()
        {

            if(HttpContext.Session.GetInt32("UserId") == null)
                {
                    return RedirectToAction("Index", "Home");
                }
            var users = dbContext.Users.ToList();
            ViewBag.Users = users;
            return View();
            }






        [HttpPost("create")]
        public IActionResult CreatePost(Wedding newWed)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if(ModelState.IsValid)
            {
                newWed.UserId = (int)HttpContext.Session.GetInt32("UserId");
                dbContext.Weddings.Add(newWed);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            return View("New");
        }





        [HttpGet("show/{ActId}")]
        public IActionResult Show(int ActId)
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var x = dbContext.Weddings
                .Include(p => p.Creator)
                .Include(p => p.Answer)
                // ThenInclude() to get user name from user_id
                    .ThenInclude(v => v.yes)
                .FirstOrDefault(u => u.WedId == ActId);


            return View(x);
            
            // ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
         
            // return View();
        }




        [HttpGet("edit/{ActId}")]

        public IActionResult Edit(int ActId)
        {
            Wedding somebody = dbContext.Weddings.FirstOrDefault(u => u.WedId == ActId);

            if(somebody == null)

                return RedirectToAction("Index");

            return View(somebody);
        }



        [HttpPost("update/{ActId}")]
        public IActionResult Update(Wedding user, int ActId)
        {


                Wedding toUpdate = dbContext.Weddings.FirstOrDefault(u => u.WedId == ActId);

                if(toUpdate == null)
                    return RedirectToAction("Dashboard");

                toUpdate.Wed1 = user.Wed1;
                toUpdate.Wed2 = user.Wed2;
                toUpdate.Date = user.Date;
                toUpdate.Address = user.Address;


                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
 






        [HttpGet("delete/{ActId}")]
        public IActionResult Delete(int ActId)
        {

            // query for thing
            Wedding toDel = dbContext.Weddings.FirstOrDefault(v => v.WedId == ActId && v.UserId == (int)SessionUser);
            dbContext.Weddings.Remove(toDel);
            dbContext.SaveChanges();
            return RedirectToAction("dashboard", "wedding");
        }







    }
}