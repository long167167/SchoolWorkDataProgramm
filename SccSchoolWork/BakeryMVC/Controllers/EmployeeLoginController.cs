using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bakery.Models;

namespace Bakery.Controllers
{
    public class EmployeeLoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Index([Bind(Include = "PersonLastName, PersonEmail, PersonPhone")]LoginClass lc)
        {   //connecting database
            BakeryEntities db = new BakeryEntities();
            Person p = new Person();
            var email = (from pl in db.People
                         where pl.PersonLastName.Equals(lc.PersonLastName)
                         select pl.PersonEmail).FirstOrDefault();
            var phone = (from pl in db.People
                         where pl.PersonLastName.Equals(lc.PersonLastName)
                         select pl.PersonPhone).FirstOrDefault();
            if (lc.PersonEmail == email || lc.PersonPhone == phone)
            {   //lookup user key
                int uID = (from pl in db.People
                           where pl.PersonLastName.Equals(lc.PersonLastName) && (pl.PersonEmail.Equals(lc.PersonEmail) || pl.PersonPhone.Equals(lc.PersonPhone))
                           select pl.PersonKey).FirstOrDefault();
                var FName = (from pl in db.People
                             where pl.PersonLastName.Equals(lc.PersonLastName) && (pl.PersonEmail.Equals(lc.PersonEmail) || pl.PersonPhone.Equals(lc.PersonPhone))
                             select pl.PersonFirstName).FirstOrDefault();
                int eID=0;
                 eID = (from e in db.Employees
                           where e.PersonKey==(uID)
                           select e.EmployeeKey).FirstOrDefault();
                Session["EmployeeID"] = eID;
                if (Session["EmployeeID"] != null)
                {     //create a message class and pass the string to the result view
                    Message msg = new Message("Welcome " + FName + ", you can Operate the system now.");
                    return RedirectToAction("Result", msg);
                }
                Message message1 = new Message("Invalid Employee Login");
                return View("Result", message1);
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