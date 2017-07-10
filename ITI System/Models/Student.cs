using ITI_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ITI_System.Models
{
    [Table("Students")]
    public class Student:ApplicationUser
    {


        //  [Required(ErrorMessage ="plz enter student Attendence Grade")]
        [Display(Name = "Attendance Grade")]
        public int? Std_Attendence_Grade { get; set; }
        [Display(Name = "Department")]

        [ForeignKey("Department")]
        public int? Dept_Id { get; set; }


        public virtual ICollection<StudentExam> Std_Exam { get; set; }
        public virtual ICollection<Student_Course_Instructor> Student_Course_Instructor { get; set; }
        public virtual List<Permission> Permissions_List { get; set; }

        public virtual Department Department { get; set; }


    }
}