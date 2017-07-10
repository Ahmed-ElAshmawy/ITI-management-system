using ITI_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ITI_System.Models
{
    [Table("Permissions")]
    public class Permission
    {
        [Key]
        public int Perm_Id { get; set; }

        [Display(Name = "Permission Date")]
        [DataType(DataType.Date, ErrorMessage = "Invalid Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Perm_Date { get; set; }

        [ForeignKey("Student")]
        public string Stud_Id { get; set; }

        public virtual Student Student { get; set; }
    }
}