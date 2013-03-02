using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcEMasterTest.Models;

namespace MvcEMasterTest.Controllers
{
    public class PatientsController : Controller
    {
        private DbManager db = new DbManager();

        //
        // GET: /Patients/

        public ActionResult Index(string sortBy, string filterByName = null,
           string filterByGender = null, string filterByNotes = null)
        {
                var patients = from m in db.Patients
                               select m;

                // Filtering patients
                if (!String.IsNullOrEmpty(filterByName))
                {
                    patients = patients.Where(s => s.Name.Contains(filterByName));
                }

                if (!String.IsNullOrEmpty(filterByGender))
                {
                    patients = patients.Where(s => s.Gender.Contains(filterByGender));
                }

                if (!String.IsNullOrEmpty(filterByNotes))
                {
                    patients = patients.Where(s => s.Notes.Contains(filterByNotes));
                }

                // Ordering patients by
                switch (sortBy)
                {
                    // Order by gender
                    case "gender":
                        patients = from d in patients
                                  orderby d.Gender
                                  select d;
                        break;
                   // Order by birthDate
                    case "bithDate":
                        patients = from d in patients
                                  orderby d.Birthdate
                                  select d;
                        break;
                    // Order by notes
                    case "notes":
                        patients = from d in patients
                                  orderby d.Notes
                                  select d;
                        break;
                    // Order by regristrationDate
                     case "registrationDate":
                        patients = from d in patients
                                  orderby d.RegistrationDate
                                  select d;
                        break;
                    //Order by name
                    default:
                        patients = from d in patients
                                  orderby d.Name
                                  select d;
                        break;
                }
                return View(patients);
         
        }

        //
        // GET: /Patients/Details/5

        public ActionResult Details(int id = 0)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        //
        // GET: /Patients/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Patients/Create

        [HttpPost]
        public ActionResult Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Patients.Add(patient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(patient);
        }

        //
        // GET: /Patients/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        //
        // POST: /Patients/Edit/5

        [HttpPost]
        public ActionResult Edit(Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(patient);
        }

        //
        // GET: /Patients/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        //
        // POST: /Patients/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient patient = db.Patients.Find(id);
            db.Patients.Remove(patient);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}