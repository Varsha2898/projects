using FinalBill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FinalBill.Controllers
{
    public class StaffController : Controller
    {
        projectEntities4 db = new projectEntities4();

        public ActionResult StaffReg()
        {
            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StaffReg([Bind(Include = "Id,FName,LName,Dob,Gender,ContactNumber,Email,User_Id,Password")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                db.Staffs.Add(staff);
                //db.SaveChanges();
                return RedirectToAction("Message");
            }



            return View(staff);
        }
        public ActionResult Message()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FinalBill.Models.Staff userModel)
        {
            using (projectEntities4 db = new projectEntities4())
            {
                var userDetails = db.Staffs.Where(x => x.User_Id == userModel.User_Id && x.Password == userModel.Password).FirstOrDefault();
                if (userDetails == null )
                {
                    userModel.LoginErrorMessage = "Wrong username or password.";
                    return View("Index", userModel);
                }
                if (userDetails.status =="Pending")
                {
                    userModel.LoginErrorMessage = "Your request has not been approved yet";
                    return View("Index", userModel);
                }
                else
                {
                    Session["userId"] = userDetails.User_Id;
                    Session["Password"] = userDetails.Password;
                    return RedirectToAction("Index1", "Staff");
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
            string userId = (string)Session["userId"];
            Session.Abandon();
            return RedirectToAction("Index", "Staff");
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