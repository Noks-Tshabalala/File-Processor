using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileProcessor.Models.Results
{
    public class FileDetail
    {
        [Key]
        public int Id { get; set; }
       
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUsers { get; set; }

        public DateTime DateProcessed { get; set; }

        [Required]
        [MaxLength(200)]

        [Index("FileNameUniqIndex",IsUnique = true)]
        public string FileName { get; set; }
    }
}