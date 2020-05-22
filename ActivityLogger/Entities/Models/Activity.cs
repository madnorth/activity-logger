using System;
using System.ComponentModel.DataAnnotations;

namespace ActivityLogger.Entities.Models
{
    public class Activity
    {
        public long Id { get; set; }
        
        [Required]
        public Category Category { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string Comment { get; set; }
    }
}