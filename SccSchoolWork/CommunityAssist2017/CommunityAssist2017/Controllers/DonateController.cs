using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommunityAssist2017.Models;

namespace CommunityAssist2017.Controllers
{
    public class DonateController : Controller
    {
        // GET: Donation
        public ActionResult Index()
        {

            if (Session["PersonKey"] == null)
            {
                Message m = new Message("You must be logged First");
                return RedirectToAction("Result", m);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Index([Bind(Include = "PersonKey, DonationAmount, DonationDate")]Donate d)
        {
            CommunityAssist2017Entities db = new CommunityAssist2017Entities();
            Donation dt = new Donation();
            dt.PersonKey = d.PersonKey;
            dt.DonationAmount = d.DonationAmount;
            dt.DonationDate = d.DonationDate;
            dt.DonationConfirmationCode = Guid.NewGuid();
            db.Donations.Add(dt);
            db.SaveChanges();

            Message msg = new Message();
            msg.MessageText = "Thank you for your donation.";


            return View("Result", msg);
        }

        public ActionResult Result(Message m)
        {
            return View(m);
        }
    }
}