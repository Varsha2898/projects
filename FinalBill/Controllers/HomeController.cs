using FinalBill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalBill.Controllers
{
    public class HomeController : Controller
    {
        projectEntities4 db = new projectEntities4();
        
        public ActionResult HomeView1()
        {
            return View();
        }
        public ActionResult Index()
        {
            var items = db.Docs.ToList();
           if (items != null)
            {
               ViewBag.data = items;
            }
            return View();
        }

        [HttpPost]

        public ActionResult Index([Bind(Include = "PatientId,Doctors_Name,Consult_Fees")] Doc d)
        {
            TotBill b = new TotBill();
            if (ModelState.IsValid)
            {
                //Bill b = new Bill();

                b.PatientId = (int)d.PatientId;
                b.ConsFees = d.Consult_Fees;
                b.DoctorsName = d.Doctors_Name;
                db.TotBills.Add(b);
                db.SaveChanges();
                return RedirectToAction("Index1");
            }

            return View(b);
        }
        public ActionResult Index1()
        {
            var items = db.Services.ToList();
            if (items != null)
            {
                ViewBag.data = items;
            }
            return View();
        }



        [HttpPost]



        public ActionResult Index1([Bind(Include = "PatientId,ServiceName,Cost")] Service d)
        {
            TotBill b = new TotBill();
            if (ModelState.IsValid)
            {
                /* //Bill b = new Bill();
                  b = db.TotBills.Find(d.PatientId);
                 //b.PatientId = (int)d.PatientId;
                 b.serviceName = d.ServiceName;
                 b.Servcost = d.Cost;
                 db.TotBills.Add(b);
                 db.SaveChanges();
                 return RedirectToAction("Index");*/
                var ent = db.Set<TotBill>().SingleOrDefault(o => o.PatientId == d.PatientId);
               ent.serviceName = d.ServiceName;
                ent.Servcost= d.Cost;
               // ent.totalCost = ent.Servcost + ent.ConsFees;
                db.SaveChanges();
                return RedirectToAction("Index2");
            }





            return View(b);
        }
        public ActionResult Index2()
        {
            var items = db.Inpatients.ToList();
            if (items != null)
            {
                ViewBag.data = items;
            }
            return View();
        }



        [HttpPost]



        public ActionResult Index2([Bind(Include = "PatientId,Rcharges")] Inpatient d)
        {
            TotBill b = new TotBill();
            if (ModelState.IsValid)
            {
                
                var ent = db.Set<TotBill>().SingleOrDefault(o => o.PatientId == d.PatientId);
                ent.RoomCharges = d.Rcharges;
                
                db.SaveChanges();
                return RedirectToAction("Index3");
            }
         return View(b);
        }
        public ActionResult Index3()
        {
            var items = db.Pharmacies.ToList();
            if (items != null)
            {
                ViewBag.data = items;
            }
            return View();
        }



        [HttpPost]



        public ActionResult Index3([Bind(Include = "PatientId,quantity,cost")] Pharmacy d)
        {
            TotBill b = new TotBill();
            if (ModelState.IsValid)
            {

               
                var ent = db.Set<TotBill>().SingleOrDefault(o => o.PatientId == d.patientId);
                d.totalCost = d.quantity * d.cost;
                ent.PharmacyCharges = d.totalCost;
                ent.totalCost = ent.ConsFees + ent.Servcost + ent.RoomCharges + ent.PharmacyCharges;

                db.SaveChanges();
                return RedirectToAction("Index4");
            }
            return View(b);
        }
        public ActionResult Index4()
        {
            
            return View();
        }
    }
}
