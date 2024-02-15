using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab10.Models
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public ICollection<Article> Articles { get; set; }

        
    }
}
