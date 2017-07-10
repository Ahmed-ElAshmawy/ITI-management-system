using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ITI_System.Models
{
    [Table("Attendence")]
    public class Attendence
    {
        [Key, Column(Order = 0), ForeignKey("Student")]
        public string Id { get; set; }


        [Key, Column(Order = 1)]
        [Display(Name = "Absent Date")]
        [DataType(DataType.Date, ErrorMessage = "Invalid Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Absence_Date { get; set; }

        public virtual Student Student { get; set; }

    }
}