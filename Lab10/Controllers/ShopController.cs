using Lab10.Data;
using Lab10.Models;
using Lab10.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab10.Controllers
{
    public class ShopController : Controller
    {
        private readonly StoreDbContext _context;
        public ShopController(StoreDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "NormalUser")]
        public IActionResult ConfirmOrder()
        {
            return View("OrderConfirmation", GetCartItems());
        }
        [HttpPost]
        [Authorize(Roles = "NormalUser")]
        public IActionResult OrderConfirmed(OrderViewModel model)
        {
            clearCart();
            return View("OrderConfirmed", model);
        }

        private void clearCart()
        {
            var cartCookies = Request.Cookies.Where(c => c.Key.StartsWith("article"));

            foreach (var cookie in cartCookies)
            {
                Response.Cookies.Delete(cookie.Key);
            }
        }

        public async Task<IActionResult> Index()
        {
            var articles = await _context.Articles.Include(a => a.Category).ToListAsync();

            ViewBag.Categories = _context.Categories
               .Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name })
               .ToList();

            return View(articles);
        }

        public async Task<IActionResult> FilterArticles(int categoryId)
        {
            var categories = await _context.Categories
                .Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.Name })
                .ToListAsync();

            ViewBag.Categories = categories;

            var articles = await _context.Articles
                .Include(a => a.Category)
                .Where(a => categoryId == 0 || a.CategoryId == categoryId)
                .ToListAsync();

            return View("Index", articles);
        }
        [AllowAnonymous]
        [ExcludeRoles("Admin")]
        public IActionResult AddToCart(int articleId)
        {
            string key = "article"+articleId;
            var currQuantity = Request.Cookies[key];
            if(!string.IsNullOrEmpty(currQuantity) && int.TryParse(currQuantity, out int quantity)) 
            {
                quantity++;
            }
            else
            {
                quantity = 1;
            }
            Response.Cookies.Append(key, quantity.ToString(), new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7)
            });
            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        [ExcludeRoles("Admin")]
        public IActionResult Cart()
        {
            var cartItems = GetCartItems(); 

            return View(cartItems);
        }
        private List<CartItem> GetCartItems()
        {
            List<CartItem> cartItems = new List<CartItem>();
            var cartCookies = Request.Cookies.Where(c => c.Key.StartsWith("article"));
            var articles = _context.Articles.Include(a => a.Category);

            foreach (var cookie in cartCookies)
            {
                if (int.TryParse(cookie.Key.Substring("article".Length), out int articleId) && int.TryParse(cookie.Value, out int quantity))
                { 
                    var article = articles
                        .Where(a => a.ArticleId == articleId)
                        .FirstOrDefault();

                    if (article != null)
                    {
                        cartItems.Add(new CartItem(article, quantity));
                    }
                }
            }

            return cartItems;
        }
        [AllowAnonymous]
        [ExcludeRoles("Admin")]
        public IActionResult UpdateCart(int articleId, int quantityChange)
        {
            string cookieKey = $"article{articleId}";
            var currentQuantity = Request.Cookies[cookieKey];

            if (int.TryParse(currentQuantity, out int currentQuantityValue))
            {
                currentQuantityValue += quantityChange;

                if (currentQuantityValue > 0 && quantityChange!=0)
                {
                    Response.Cookies.Append(cookieKey, currentQuantityValue.ToString(), new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(7)
                    });
                }
                else
                {
                    Response.Cookies.Delete(cookieKey);
                }
            }

            return RedirectToAction("Cart");
        }

    }


}
