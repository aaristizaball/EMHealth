using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcEMasterTest.Models;

namespace MvcEMasterTest.Controllers
{
    public class ExamsController : Controller
    {
        private const string EXAMS_FOLDER = "ExamsFile";

        private DbManager db = new DbManager();

        //
        // GET: /Exams/

        public ActionResult Index( string sortBy, string patientsList, string filterByDoctor, string filterByType,
            string filterByNotes, string filterByReport)
        {
            var exams = db.Exams.Include(e => e.Patient);

            //Patients list for the filter
            var patients = new List<string>();
            var patientsQuery = from d in db.Patients
                                orderby d.Name
                                select d.Name;

            patients.AddRange(patientsQuery);
            ViewBag.patientsList = new SelectList(patients);

            // Filtering exams
            if (!String.IsNullOrEmpty(patientsList))
            {
                exams = exams.Where(s => s.Patient.Name.Contains(patientsList));
            }

            if (!String.IsNullOrEmpty(filterByDoctor))
            {
                exams = exams.Where(s => s.Doctor.Contains(filterByDoctor));
            }

            if (!String.IsNullOrEmpty(filterByType))
            {
                exams = exams.Where(s => s.Type.Contains(filterByType));
            }

            if (!String.IsNullOrEmpty(filterByNotes))
            {
                exams = exams.Where(s => s.Notes.Contains(filterByNotes));
            }

            if (!String.IsNullOrEmpty(filterByReport))
            {
                exams = exams.Where(s => s.Report.Contains(filterByReport));
            }

            //Ordering exams
            switch (sortBy)
            {
                case "patient":

                    // Order by name
                    exams = from d in exams
                            orderby d.Patient.Name
                            select d;
                    break;
                case "issueDate":

                    // Order by IssueDate
                    exams = from d in exams
                            orderby d.IssueDate
                            select d;
                    break;
                case "doctor":
                    // Order by doctor
                    exams = from d in exams
                            orderby d.Doctor
                            select d;
                    break;

                case "type":
                    // Order by type
                    exams = from d in exams
                            orderby d.Type
                            select d;
                    break;
                case "notes":

                    // Order by notes
                    exams = from d in exams
                            orderby d.Notes
                            select d;
                    break;
                case "report":
                    // Order by report
                    exams = from d in exams
                            orderby d.Report
                            select d;
                    break;
                default:
                    // Order by id
                    exams = from d in exams
                            orderby d.ID
                            select d;
                    break;


            }

            return View(exams);
        }

        //
        // GET: /Exams/Details/5

        public ActionResult Details(string id = null)
        {
            Exam exam = db.Exams.Find(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            return View(exam);
        }

        //
        // GET: /Exams/Create

        public ActionResult Create()
        {
            ViewBag.PatientId = new SelectList(db.Patients, "ID", "Name");
            return View();
        }

        //
        // POST: /Exams/Create

        [HttpPost]
        public ActionResult Create(Exam exam)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0 && !String.IsNullOrEmpty(Request.Files[0].FileName))
                {
                    string filename = System.IO.Path.GetFileName(Request.Files[0].FileName);
                    string filePath = HttpContext.Server.MapPath("../") + EXAMS_FOLDER;

                    if (saveFile(filename, filePath))
                    {
                        exam.UrlUploadedFile = EXAMS_FOLDER + "\\" + filename;
                    }
                }

                db.Exams.Add(exam);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PatientId = new SelectList(db.Patients, "ID", "Name", exam.PatientId);
            return View(exam);
        }

        //
        // GET: /Exams/Edit/5

        public ActionResult Edit(string id = null)
        {
            Exam exam = db.Exams.Find(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            ViewBag.PatientId = new SelectList(db.Patients, "ID", "Name", exam.PatientId);
            return View(exam);
        }

        //
        // POST: /Exams/Edit/5

        [HttpPost]
        public ActionResult Edit(Exam exam)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0 && !String.IsNullOrEmpty(Request.Files[0].FileName))
                {
                    string filename = System.IO.Path.GetFileName(Request.Files[0].FileName);
                    string projectPath = HttpContext.Server.MapPath("../../");

                    try
                    {
                        System.IO.File.Delete(projectPath + exam.UrlUploadedFile);
                    }
                    catch (Exception e)
                    {
                        //TODO: Manage error log
                    }
                    try
                    {
                        if (saveFile(filename, projectPath + EXAMS_FOLDER))
                        {
                            exam.UrlUploadedFile = EXAMS_FOLDER + "\\" + filename; ;
                        }
                    }
                    catch (Exception e)
                    {
                        //TODO: Manage error log
                    }
                }

                db.Entry(exam).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PatientId = new SelectList(db.Patients, "ID", "Name", exam.PatientId);
            return View(exam);
        }

        //
        // GET: /Exams/Delete/5

        public ActionResult Delete(string id = null)
        {
            Exam exam = db.Exams.Find(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            return View(exam);
        }

        //
        // POST: /Exams/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            Exam exam = db.Exams.Find(id);
            db.Exams.Remove(exam);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private bool saveFile(string name, string path)
        {
            try
            {
                if (Request.Files.Count == 1)
                {
                    bool IsExists = System.IO.Directory.Exists(path);

                    if (!IsExists)
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }

                    Request.Files[0].SaveAs(path + "\\" + name);
                }
                return true;
            }
            catch (IOException e)
            {
                //TODO: Manage error log
                return false;
            }
            catch (UnauthorizedAccessException e)
            {
                //TODO: Manage error log
                return false;
            }
        }
    }
}