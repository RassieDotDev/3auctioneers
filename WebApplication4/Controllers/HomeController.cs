using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;
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
        public ActionResult Auction(double? newbid, int? Id)
        {
            var price = from m in db.Item_table
                        select m;

            var model = new Models.AuctionModel
            {
                item_table = price
            };

            if (newbid == null)
            {
                return View(model);
            }
            else
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        Item_table f = (from data in db.Item_table where data.Id == Id select data).FirstOrDefault();
                        f.prod_cbid = (double)newbid;
                        db.SaveChanges();

                        dbContextTransaction.Commit();
                        var upInfo = from m in db.Item_table
                                    select m;
                        var models = new Models.AuctionModel
                        {
                            item_table = upInfo
                        };

                        return View(models);
                    }
                    catch (Exception /*ex*/)
                    {
                        return View(model);
                    }
                }
            }

        }

        public ActionResult Auction(int? id)
        {
             var price = from m in db.Item_table
                            select m;

            var model = new Models.AuctionModel
            {
                item_table = price
            };

            return View(model);
        }

        
    }
}