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

namespace WebApplication4.Models
{
    public class Item_tableController : Controller
    {
        private AuctionDBEntities db = new AuctionDBEntities();

        // GET: Item_table
        public ActionResult Index()
        {
            return View(db.Item_table.ToList());
        }

        public ActionResult Edit()
        {
            return View();
        }

        // GET: Item_table/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item_table item_table = db.Item_table.Find(id);
            if (item_table == null)
            {
                return HttpNotFound();
            }
            return View(item_table);
        }

        // GET: Item_table/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: Item_table/Auction
        /*public ActionResult Auction()
        {
            return View();
        }*/

        // POST: Item_table/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,prod_name,prod_des,prod_sbid,prod_cbid,prod_pic")] Item_table item_table, HttpPostedFileBase file)
        {
            
            if (ModelState.IsValid)
            {
                string path = "" ;
                if (file != null && file.ContentLength > 0)
                        try
                        {
                            path = "~/Images/" + Path.GetFileName(file.FileName);
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
                return RedirectToAction("Index","Item_table");

            }
            return View(item_table);
        }

        // POST: Item_table/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        


        // GET: Item_table/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item_table item_table = db.Item_table.Find(id);
            if (item_table == null)
            {
                return HttpNotFound();
            }
            return View(item_table);
        }

        // POST: Item_table/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item_table item_table = db.Item_table.Find(id);
            db.Item_table.Remove(item_table);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Auction([Bind(Include = "Id,prod_name,prod_des,prod_sbid,prod_cbid")] Item_table item_table)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(item_table).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Auction");
                }
                return View(item_table);
            }
            catch (DbEntityValidationException e)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", e.ToString() + "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                return View();
            }

        }

        public ActionResult Auction(int? id)
        {
            var price = from m in db.Item_table
                        select m;

            return View();

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
