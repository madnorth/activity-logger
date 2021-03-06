﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ActivityLogger.Data.Models
{
    public class Activity
    {
        public long Id { get; set; }
        
        [Required]
        public int CategoryId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string Comment { get; set; }

        public Category Category { get; set; }
    }
}