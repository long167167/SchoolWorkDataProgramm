using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bakery.Models;

namespace Bakery.Controllers
{
    public class CustomerRegisterController : Controller
    {
        // GET: CustomerRegister
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = " PersonKey, PersonLastName, PersonFirstName,PersonEmail, PersonPhone,PersonDateAdded")]CustomerRegister cr)
        {   //connecting database of bakery
            BakeryEntities db = new BakeryEntities();
            Person p = new Person();
            p.PersonLastName = cr.PersonLastName;
            p.PersonFirstName = cr.PersonFirstName;
            var email = (from pl in db.People
                         where pl.PersonLastName.Equals(cr.PersonLastName) || pl.PersonFirstName.Equals(cr.PersonFirstName)||pl.PersonEmail.Equals(cr.PersonEmail)
                         select pl.PersonEmail).FirstOrDefault();
            var phone = (from pl in db.People
                         where pl.PersonLastName.Equals(cr.PersonLastName) || pl.PersonFirstName.Equals(cr.PersonFirstName)||pl.PersonPhone.Equals(cr.PersonPhone)
                         select pl.PersonPhone).FirstOrDefault();


            //checking if the personkey existed by comparing the input email and phone 
            
             if (email == cr.PersonEmail || phone==cr.PersonPhone)
             {
                Message msg = new Message("Account Existed");
                return RedirectToAction("Result", msg);

            }
            p.PersonEmail=cr.PersonEmail;
            p.PersonPhone = cr.PersonPhone;
            p.PersonDateAdded = DateTime.Now;
            db.People.Add(p);
            db.SaveChanges();
            Message m = new Message();
            m.MessageText = "Thank you, the book has been added";


            return View("Result", m);
        }


        public ActionResult Result(Message m)
        {
            return View(m);
        }
    }
}