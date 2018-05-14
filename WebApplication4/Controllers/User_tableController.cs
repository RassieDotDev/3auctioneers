using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class User_tableController : Controller
    {
        private AuctionDBEntities db = new AuctionDBEntities();

        // GET: User_table
        //public ActionResult Index()
        //{
         //   return View();//(db.User_table.ToList());
        //}

        public ActionResult Index(int? id)
        {

            if (ModelState.IsValid)
            {
                var price = from m in db.User_table
                            select m;

                return View(price.ToList());
            }

            return View();

        }

        // GET: User_table/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_table user_table = db.User_table.Find(id);
            if (user_table == null)
            {
                return HttpNotFound();
            }
            return View(user_table);
        }

        // GET: User_table/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User_table/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,user_name,user_email,user_password")] User_table user_table)
        {
            if (ModelState.IsValid)
            {
                db.User_table.Add(user_table);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user_table);
        }

        // GET: User_table/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_table user_table = db.User_table.Find(id);
            if (user_table == null)
            {
                return HttpNotFound();
            }
            return View(user_table);
        }

        // POST: User_table/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,user_name,user_email,user_password")] User_table user_table)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user_table).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user_table);
        }

        // GET: User_table/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_table user_table = db.User_table.Find(id);
            if (user_table == null)
            {
                return HttpNotFound();
            }
            return View(user_table);
        }

        // POST: User_table/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User_table user_table = db.User_table.Find(id);
            db.User_table.Remove(user_table);
            db.SaveChanges();
            return RedirectToAction("Index");
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
