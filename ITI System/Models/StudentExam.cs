using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ITI_System.Models
{
    public class StudentExam
    {
        [Key, Column(Order = 0), ForeignKey("Student")]
        public string Std_Id { get; set; }
        [Key, Column(Order = 1),ForeignKey("Exam")]
        public int Exam_Id { get; set; }

        public virtual Student Student { get; set; }
        public virtual Exam Exam { get; set; }
        public virtual int Std_Grade { get; set; }
    }
}