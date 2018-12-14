using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FileProcessor.Models.Results
{
    public class Result
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUsers { get; set; }

        public int FileId { get; set; }
        [ForeignKey("FileId")]
        public virtual FileDetail FileDetails { get; set; }

        public DateTime DateProcessed { get; set; }

        public int Formula { get; set; }

        
        public int A { get; set; }

        public int B { get; set; }

        public int C { get; set; }

        public double CalculatedResult { get; set; }

        [Required]
        [MaxLength(100)]
        public string FileName { get; set; }

    }
}