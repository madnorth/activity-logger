using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ActivityLogger.Data.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        public int? ParentId { get; set; }

        public Category Parent { get; set; }
        public IList<Category> Children { get; set; }
    }
}