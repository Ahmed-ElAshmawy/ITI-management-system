using ITI_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ITI_System.Models
{
    [Table("Instructor")]
    public class Instructor:ApplicationUser
    {
        
        public string Ins_Qualifications { get; set; }
        public string Ins_Status { get; set; }
        public DateTime Ins_GraduationYear { get; set; }

        [ForeignKey("Department")]
        public int? Dept_Id { get; set; }
        public virtual List<Course> Crs_List { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<Student_Course_Instructor> Student_Course_Instructor { get; set; }
        public virtual ICollection<Department_Instructor_Course> Department_Instructor_Course { get; set; }
    }
}