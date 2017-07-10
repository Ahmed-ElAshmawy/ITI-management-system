using ITI_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ITI_System.Models
{
    public class Department_Instructor_Course
    {

        [Key, Column(Order = 0), ForeignKey("Department")]
        public int Dept_Id { get; set; }

        [Key, Column(Order = 1), ForeignKey("Instructor")]
        public string Ins_Id { get; set; }

        [Key, Column(Order = 2), ForeignKey("Course")]
        public int Crs_Id { get; set; }

        public string Crs_Status { get; set; }

        public virtual Department Department { get; set; }
        public virtual Instructor Instructor { get; set; }
        public virtual Course Course { get; set; }
    }
}