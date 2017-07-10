using ITI_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ITI_System.Models
{
    [Table("Question")]
    public class Question
    {
        [Key]
        public int Question_Id { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public string Correct_Answer { get; set; }


        public virtual List<Answers> Answers { get; set; }
        public virtual List<Exam> ExamList { get; set; }
    }
}