using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MvcEMasterTest.Models
{
    public class Patient
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Gender { get; set; }
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }
        [Required]
        public string Notes { get; set; }
        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; }
    }
}