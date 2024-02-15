using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab10.Models
{
    public class Article
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ArticleId { get; set; }
        
        public string Name { get; set; }
        public decimal Price { get; set; }
        [DisplayName("Image")]
        public string ImagePath { get; set; }
        [DisplayName("Category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

      
    }

}
