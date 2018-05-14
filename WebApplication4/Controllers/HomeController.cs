using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class HomeController : Controller
    {
        private AuctionDBEntities db = new AuctionDBEntities();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Auction(Item_table item_table, int newbid, int? Id)
        {

            if (ModelState.IsValid)
            {
                Item_table item = new Item_table();
                item = db.Item_table.Find(Id);
                item.prod_cbid = newbid;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Auction", "Home");

            }
            return View(item_table);
        }

        public ActionResult Auction(int? id)
        {

            if (Session["Id"] != null)
            {
                var price = from m in db.Item_table
                            select m;

                return View(price.ToList());
            }
            else
            {
                return RedirectToAction("Auction", "Home");
            }
            

        }

        
    }
}