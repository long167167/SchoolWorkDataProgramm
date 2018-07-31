using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommunityAssist2017.Models;

namespace CommunityAsset2017.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "Email, Password")]Login lc)
        { //make connection with database
            CommunityAssist2017Entities db = new CommunityAssist2017Entities();
            //use stored procedure in database
            int result = db.usp_Login(lc.Email, lc.Password);
            if (result != -1)
            {
                var uID = (from p in db.People
                           where p.PersonEmail.Equals(lc.Email)
                           select p.PersonKey).FirstOrDefault();
                //cast ID into a int
                int key = (int)uID;
                //store it in a session key
                Session["PersonKey"] = key;
                var pName = (from p in db.People
                             where p.PersonEmail.Equals(lc.Email)
                             select p.PersonFirstName).FirstOrDefault();
                //create a message class and pass the string to the result view

                Message msg = new Message("Welcome, " + pName + ". You can now make donation or request for grant.");
                return RedirectToAction("Result", msg);
            }
            //if it fails pass this message to the result view
            Message message = new Message("Invalid Login");
            return View("Result", message);
        }

        //you must have an ActionResult method for the Result
        //that takes the message as a parameter
        public ActionResult Result(Message msg)
        {
            return View(msg);
        }
    }
}