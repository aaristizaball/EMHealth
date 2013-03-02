using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcEMasterTest.Models
{
    public class Exam
    {
        [Required]
        [Key]
        public string ID { get; set; }

        [Required]
        public string Doctor { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Notes { get; set; }

        [Required]
        public string Report { get; set; }

        public string UrlUploadedFile { get; set; }

        [DataType(DataType.Date)]
        public DateTime IssueDate { get; set; }

        [Required]
        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
    }
}