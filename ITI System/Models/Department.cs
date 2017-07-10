using ITI_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ITI_System.Models
{
    public class Department
    {
        [Key]
  
        public int Dept_Id { get; set; }

        [Required(ErrorMessage = "plz enter DepartmentName")]
        public string Dept_name { get; set; }
        public int Dept_Capacity { get; set; }

        [ForeignKey("manager")]
    
        public string Manager_Id { get; set; } //Manager

        public virtual Instructor manager { get; set; }

        [InverseProperty("Department")]
        public virtual List<Instructor> Ins_list { get; set; }

        public virtual List<Student> Std_List { get; set; }

        public virtual List<Course> Crs_List { get; set; }
        public virtual ICollection<Department_Instructor_Course> Department_Instructor_Course { get; set; }


    }
}