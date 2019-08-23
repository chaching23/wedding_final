using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using wedding.Models;
using Microsoft.AspNetCore.Http;

namespace wedding.Controllers
{
    [Route("action")]
    public class ActionController : Controller
    {
        private int? SessionUser
        {
            get { return HttpContext.Session.GetInt32("UserId"); }
            set { HttpContext.Session.SetInt32("UserId", (int)value); }
        }
        MyContext dbContext;
        public ActionController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("{ActId}/Action/{z}")]
        public IActionResult Action(int ActId, bool z)
        {
            
            Action newGuy = new Action()
            {
                WedId = ActId,
                RSVP = z,
                UserId = (int)SessionUser
            };

            dbContext.Actions.Add(newGuy);
            dbContext.SaveChanges();
            return RedirectToAction("dashboard", "wedding");
        }

        [HttpGet("delete/{ActId}")]
        public IActionResult Remove(int ActId)
        {
            // query for thing
            Action toDel = dbContext.Actions.FirstOrDefault(v => v.WedId == ActId && v.UserId == (int)SessionUser);
            dbContext.Actions.Remove(toDel);
            dbContext.SaveChanges();
            return RedirectToAction("dashboard", "wedding");
        }
        






    }
}