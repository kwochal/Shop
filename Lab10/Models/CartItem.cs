using System.ComponentModel.DataAnnotations;

namespace Lab10.Models
{
    public class CartItem
    {
        public int ArticleId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        [Display(Name ="Image")] 
        public string ImagePath { get; set; }
        public Category Category { get; set; }
        public int Quantity { get; set; }

        public CartItem (Article art, int quantity)
        {
            ArticleId = art.ArticleId;
            Name = art.Name;
            Price = art.Price;
            ImagePath = art.ImagePath;
            Category = art.Category;
            Quantity = quantity;
        }

    }
}
