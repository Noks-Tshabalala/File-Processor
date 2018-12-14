using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileProcessor.ViewModels
{
    public class VMCalculationDetail
    {
        public int Id { get; set; }
        
        public int FileId { get; set; }
        [ForeignKey("FileId")]
        public virtual VMFileDetail VMFileDetails { get; set; }
        
        public int Formula { get; set; }

        [Display(Name = "Formula Description")]
        public string FormulaDescription { get; set; }

        [Display(Name = "Variable A")]
        public int A { get; set; }

        [Display(Name = "Variable B")]
        public int B { get; set; }

        [Display(Name = "Variable C")]
        public int C { get; set; }
        
        [Display(Name = "Result")]
        public double? CalculatedResult { get; set; }
        
        public string ErrorMessage { get; set; }
    }
    
}