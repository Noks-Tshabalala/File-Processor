using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace FileProcessor.ViewModels
{
    public class VMFileDetail
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        [Display(Name ="File Uploader")]
        public string UserName { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "File Name")]
        public string FileName { get; set; }

        [Display(Name = "Upload Date")]
        public DateTime DateProcessed { get; set; }
        
        [Display(Name = "Row Count")]
        public int Total { get; set; }

        [Display(Name = "Erred Records")]
        public int Erred { get; set; }

        [Display(Name = "Duplicates")]
        public int Duplicates { get; set; }
    }
}