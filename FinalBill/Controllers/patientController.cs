using FinalBill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FinalBill.Controllers
{
    public class patientController : Controller
    {
        // GET: patient
        projectEntities4 db = new projectEntities4();
        public ActionResult PatientReg()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PatientReg([Bind(Include = "Id,First_Name,Last_Name,DoB,Gender,Contact_Number,Email,User_Id,Password")] Patient registration)
        {
            if (ModelState.IsValid)
            {
                db.Patients.Add(registration);
                db.SaveChanges();
                return RedirectToAction("Success");
            }



            return View(registration);
        }



        public ActionResult Success()
        {
            return View();
        }
        public ActionResult PatientReg1()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PatientReg1([Bind(Include = "Id,First_Name,Last_Name,DoB,Gender,Contact_Number,Email,User_Id,Password")] Patient registration)
        {
            if (ModelState.IsValid)
            {
                db.Patients.Add(registration);
                db.SaveChanges();
                return RedirectToAction("Success1");
            }



            return View(registration);
        }



        public ActionResult Success1()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FinalBill.Models.Patient userModel)
        {
            using (projectEntities4 db = new projectEntities4())
            {
                var userDetails = db.Patients.Where(x => x.User_Id == userModel.User_Id && x.Password == userModel.Password).FirstOrDefault();
                if (userDetails == null)
                {
                    userModel.LoginErrorMessage = "Wrong username or password.";
                    return View("Index", userModel);
                }
                
                else
                {
                    Session["userId"] = userDetails.User_Id;
                    Session["Password"] = userDetails.Password;
                    return RedirectToAction("Index1", "Patient");
                }
            }
        }
        public ActionResult Index11()
        {
            return View();
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
            return RedirectToAction("Index", "Patient");
        }
        public ActionResult Bill()
        {
            return View(db.TotBills.ToList());
        }
        [HttpPost]
        public ActionResult Details(TotBill d)
        {
            if (d.PatientId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TotBill cart = db.TotBills.Find(d.PatientId);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }
         
    }
}