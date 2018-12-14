using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileProcessor.Models.Results
{
    public class CalculationDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int FileId { get; set; }
        [ForeignKey("FileId")]
        public virtual FileDetail FileDetails { get; set; }

        [Range(1, 3, ErrorMessage = "Formula can only be between 1 and 3.")]
        public int Formula { get; set; }
        
        public int A { get; set; }

        public int B { get; set; }

        public int C { get; set; }

        public double? CalculatedResult { get; set; }

        public string ErrorMessage { get; set; }
        
    }
}