using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MvcEMasterTest.Models
{
    public class DbManager : DbContext
    {
        public DbSet<Exam> Exams { get; set; }

        public DbSet<Patient> Patients { get; set; }
    }
}