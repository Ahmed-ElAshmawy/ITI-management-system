using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ITI_System.Models
{
    public class Answers
    {
        [Key, Column(Order = 0), ForeignKey("Question")]
        public int Question_Id { get; set; }

        [Key, Column(Order = 1)]
        public string Ans { get; set; }
        public virtual Question Question { get; set; }
    }
}