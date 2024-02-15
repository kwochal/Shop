using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lab10.ViewModels
{
    public class ArticleViewModel
    {
        public int ArticleId { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        

        [DisplayName("Category")]
        public string CategoryName { get; set; }
        public IFormFile Image { get; set; }
    }
}
