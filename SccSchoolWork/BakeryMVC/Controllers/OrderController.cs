

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bakery.Models;
namespace Bakery.Controllers
{
    public class OrderController : Controller
    {//getting database
        BakeryEntities db = new BakeryEntities();
        Product p = new Product();
        Sale s = new Sale();
        SaleDetail sd = new SaleDetail();
        Person pr = new Person();
        List<Order> orders = new List<Order>();
        List<Receipt> receipts = new List<Receipt>();



        //View the Menu list
        public ActionResult Index([Bind(Include = "Quantity")]Order o)
        {

            int IDNUM = 1;
            while (IDNUM < 10)
            {
                Order order = new Order();
                order.ProductKey = (from pd in db.Products
                                    where pd.ProductKey==(IDNUM)
                                    select pd.ProductKey).FirstOrDefault();
                order.ProductName = (from pd in db.Products
                                     where pd.ProductKey==(IDNUM)
                                     select pd.ProductName).FirstOrDefault();
                order.Price =  (from pd in db.Products
                               where pd.ProductKey==(IDNUM)
                               select pd.ProductPrice).FirstOrDefault();


                orders.Add(order);
                IDNUM++;
            }
            return View(orders);

        }

        //CustomerID and EmployeeID check
     

//Order Page in put product Quantity by assigned Product ID
public ActionResult Purchase(int? id)
        {   

            Session["ProductOrderID"] = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product p = db.Products.Find(id);
            ViewBag.ProductOrder = p;
            // Order order = new Order();
            if (p == null)
            {
                return HttpNotFound();
            }

            if (Session["CustomerID"] == null)
            {
                Message m = new Message("Customer must be logged in to purchase");
                return View("Result", m);
            }

            if (Session["EmployeeID"] == null)
            {
                Message m1 = new Message("Employee must be logged in to operate the system");
                return View("Result", m1);
            }
            s.EmployeeKey =(int)Session["EmployeeID"];
            s.CustomerKey = (int)Session["CustomerID"];
            s.SaleDate = DateTime.Now;
            db.Sales.Add(s);
            db.SaveChanges();
            int saleID = db.Sales.Max(s => s.SaleKey);
            Session["salekey"] = saleID;

            return View();

        }
       


        [HttpPost, ActionName("Purchase")]
        [ValidateAntiForgeryToken]
        public ActionResult Purchase([Bind(Include = "Quantity, Discount")]Order o)
        {

            sd.ProductKey = (int)Session["ProductOrderID"];
            sd.SaleDetailQuantity = o.Quantity;
            sd.SaleDetailDiscount = o.Discount;
            sd.SaleKey = (int)Session["salekey"];
            sd.SaleDetailPriceCharged = (from pr in db.Products
                                         where pr.ProductKey==(sd.ProductKey)
                                         select pr.ProductPrice).FirstOrDefault();
            sd.SaleDetailSaleTaxPercent = (decimal)0.09;
            sd.SaleDetailEatInTax = (decimal)0;
            
            db.SaleDetails.Add(sd);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult Receipt()
        {
            if (Session["CustomerID"] != null)
            {
                Nullable<System.DateTime> SaleDate = (from s in db.Sales
                                                      where s.SaleKey == 1
                                                      select s.SaleDate).FirstOrDefault();
                Nullable<System.DateTime> lotime = DateTime.Now; 
                int rskey = 1;
               
                Receipt rc = new Receipt();
                Nullable<System.DateTime> litime;
                litime = (System.DateTime)Session["LoginTime"];
                decimal subtotal = 0;
                rc.Total = 0;
                while (SaleDate<lotime)
                {

                    rskey++;

                    SaleDate = (from s in db.Sales
                                where s.SaleKey == rskey
                                select s.SaleDate).FirstOrDefault();
                    while (SaleDate > litime)
                    {
                     int rpkey = (int)(from sd in db.SaleDetails
                              where sd.SaleKey==rskey
                              select sd.ProductKey).FirstOrDefault();
                        rc.ProductName = (from pr in db.Products
                                          where pr.ProductKey == rpkey
                                          select pr.ProductName).FirstOrDefault();
                        rc.Unitprice = (from pr in db.Products
                                        where pr.ProductKey == rpkey
                                        select pr.ProductPrice).FirstOrDefault();
                        rc.Quantity = (from sd in db.SaleDetails
                                       where sd.SaleKey == rskey
                                       select sd.SaleDetailQuantity).FirstOrDefault();
                        rc.Discount = (Decimal)(from sd in db.SaleDetails
                                       where sd.SaleKey == rskey
                                       select sd.SaleDetailDiscount).FirstOrDefault();
                        rc.SaleTax = (Decimal)(from sd in db.SaleDetails
                                                where sd.SaleKey == rskey
                                                select sd.SaleDetailSaleTaxPercent).FirstOrDefault();
                        rc.EmployeeKey =(int) (from s in db.Sales
                                          where s.SaleKey == rskey
                                          select s.EmployeeKey).FirstOrDefault();
                        subtotal = rc.Unitprice * rc.Quantity * (1 + rc.SaleTax - rc.Discount);

                        rc.Total = subtotal + rc.Total;



                        receipts.Add(rc);



                        return View(receipts);

                    }

                }
                        
            
                Session["CustomerID"] = null;
                Session["EmployeeID"] = null;
                Session["salekey"] = null;

                Session["LogOutTime"] = DateTime.Now;
                return View();
            }
            Message message = new Message("You didn't log in yet.");
            return View("Result", message);
        }

}
    
}