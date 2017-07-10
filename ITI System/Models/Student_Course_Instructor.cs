using ITI_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ITI_System.Models
{
    [Table("Student_Course_Instructor")]
    public class Student_Course_Instructor
    {
        [Key, Column(Order = 0), ForeignKey("Student")]
        public string Std_Id { get; set; }//fk-pk

        [Key, Column(Order = 1), ForeignKey("Instructor")]
        public string Ins_Id { get; set; }//fk-pk

        [Key, Column(Order = 2), ForeignKey("Course")]
        public int Crs_Id { get; set; }//fk-pk

        public int Ins_Eval { get; set; }
        public int Std_Lab_Eval { get; set; }


        public virtual Student Student { get; set; }
        public virtual Course Course { get; set; }
        public virtual Instructor Instructor { get; set; }
    }
}