using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommunityAssist2017.Models;

namespace CommunityAssist2017.Controllers
{
    public class GrantApplicationController : Controller
    {
        CommunityAssist2017Entities db = new CommunityAssist2017Entities();
        // GET: GrantApplication
        public ActionResult Index()
        {
            if (Session["PersonKey"] == null)
            {
                Message m = new Message();
                m.MessageText = "You must be logged in to add a review";
                return RedirectToAction("Result", m);
            }
            ViewBag.GrantTypeKey = new SelectList(db.GrantTypes, "GrantTypeKey", "GrantTypeName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "PersonKey, GrantAppicationDate, GrantTypeKey,GrantApplicationRequestAmount," +
            "GrantApplicationReason,GrantApplicationStatusKey,GrantApplicationAllocationAmount")]GrantApplication ga)
        {   
          
          
            ga.PersonKey = (int)Session["PersonKey"];
            ga.GrantAppicationDate = DateTime.Now;

            ga.GrantApplicationStatusKey = 1;

            db.GrantApplications.Add(ga);
            db.SaveChanges();
            

            Message m = new Message();
            m.MessageText = "Grant Application is sent";
            return View("Result", m);

        }

        public ActionResult Result(Message m)
        {
            return View(m);
        }
    }
}