using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileProcessor.ViewModels
{
    public class VMSummary
    {
        public int TotalFiles { get; set; }
        public int TotalCalculations { get; set; }
        public int TotalErred { get; set; }
        public int TotalDuplicates { get; set; }
    }
}