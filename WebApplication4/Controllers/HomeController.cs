using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
        public ActionResult Create([Bind(Include = "Id,prod_name,prod_des,prod_sbid,prod_cbid,prod_pic")] Item_table item_table, HttpPostedFileBase file)
        {

            if (ModelState.IsValid)
            {
                string path = "";
                if (file != null && file.ContentLength > 0)
                    try
                    {
                        path = "\\Images\\" + Path.GetFileName(file.FileName);
                        file.SaveAs(path);
                        ViewBag.Message = "File uploaded successfully";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                else
                {
                    ViewBag.Message = "You have not specified a file.";
                }
                item_table.prod_pic = path;
                db.Item_table.Add(item_table);
                db.SaveChanges();
                return RedirectToAction("Auction", "Home");

            }
            return View(item_table);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,prod_name,prod_des,prod_sbid,prod_cbid")] Item_table item_table)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item_table).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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