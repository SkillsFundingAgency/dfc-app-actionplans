using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.ActionPlans.Helpers;

namespace DFC.App.ActionPlans.Models
{
    public class SplitDate
    {
        [RegularExpression(RegexPatterns.Day, ErrorMessage = "Day must be a number between 1 and 31")]
        [Required]
        public string Day { get; set; }
        [Required]
        [RegularExpression(RegexPatterns.Month, ErrorMessage = "Month must be a number between 1 and 12")]
        public string Month { get; set; }
        [Required]
        [RegularExpression(RegexPatterns.Numeric, ErrorMessage = "Year must be a number")]
        public string Year { get; set; }
    }
}
