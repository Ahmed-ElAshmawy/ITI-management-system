using ITI_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ITI_System.Models
{
    [Table("Course")]
    public class Course
    {
        [Key]
        [Required]
        public int Crs_Id { get; set; }
        [Required]
        public string Crs_name { get; set; }
        [Required]
        public int Crs_LabDuration { get; set; }
        [Required]
        public int Crs_LectDuration { get; set; }
        public virtual List<Instructor> Ins_List { get; set; }
        public virtual List<Exam> Exams_List { get; set; }
        public virtual List<Department> Dept_List { get; set; }
        public virtual ICollection<Student_Course_Instructor> Student_Course_Instructor { get; set; }
        public virtual ICollection<Department_Instructor_Course> Department_Instructor_Course { get; set; }

    }
}