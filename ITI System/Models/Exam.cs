using ITI_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ITI_System.Models
{
    [Table("Exams")]
    public class Exam
    {
        [Key]
        public int Exam_Id { get; set; }

        [Required(ErrorMessage ="Plz Enter Enter Exam Start Time")]
        public DateTime Exam_Start_Time { get; set; }

        [Required(ErrorMessage = "Plz Enter Enter Exam End Time")]
        public DateTime Exam_End_Time { get; set; }

        [ForeignKey("Course")]
        public int? Crs_Id { get; set; }

        public virtual Course Course { get; set; }

        public virtual ICollection<StudentExam> StdExam { get; set; }
        public virtual List<Question> QuestionsList { get; set; }

    }
}