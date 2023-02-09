using FinalBill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FinalBill.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        projectEntities4 db = new projectEntities4();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FinalBill.Models.Admin userModel)
        {
            using (projectEntities4 db = new projectEntities4())
            {
                var userDetails = db.Admins.Where(x => x.UserId == userModel.UserId && x.Password == userModel.Password).FirstOrDefault();
                if (userDetails == null)
                {
                    userModel.LoginErrorMessage = "Wrong username or password.";
                    return View("Index", userModel);
                }
                else
                {
                    Session["userId"] = userDetails.UserId;
                    Session["Password"] = userDetails.Password;
                    return RedirectToAction("Index1", "Admin");
                }
            }
        }
        public ActionResult Index1()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult LogOut()
        {
            int userId = (int)Session["userId"];
            Session.Abandon();
            return RedirectToAction("Index", "Admin");
        }
        public ActionResult Index2()
        {
            return View(db.Staffs.ToList());
        }
        public ActionResult grant(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff m = db.Staffs.Find(id);
            if (m == null)
            {
                return HttpNotFound();
            }
            return View(m);
        }
        [HttpPost, ActionName("grant")]
        [ValidateAntiForgeryToken]
        public ActionResult grantTo(int id)
        {
            Staff m = db.Staffs.Find(id);
            m.status = "Approved";

            db.SaveChanges();
            return RedirectToAction("Index2");
        }
        public ActionResult Reject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff cart = db.Staffs.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }


        [HttpPost, ActionName("Reject")]
        [ValidateAntiForgeryToken]
        public ActionResult RejectConfirmed(int id)
        {
            Staff c = db.Staffs.Find(id);
            db.Staffs.Remove(c);
            db.SaveChanges();
            return RedirectToAction("Index2");
        }
        public ActionResult Bill()
        {
            return View(db.TotBills.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TotBill cart = db.TotBills.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }
    }
}