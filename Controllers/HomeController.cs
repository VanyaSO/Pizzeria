using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pizzeria.Interfaces;
using Pizzeria.Models;
using Pizzeria.Models.Pages;
using Pizzeria.ViewModels.Home;

namespace Pizzeria.Controllers;

public class HomeController : Controller
{
    private readonly IProduct _products;
    private readonly ICategory _categories;
    public HomeController(IProduct products, ICategory categories)
    {
        _products = products;
        _categories = categories;
    }
    
    [Route("/")]
    [HttpGet]
    public async Task<IActionResult> Index(QueryOptions options, int categoryId)
    {
        ViewBag.RoutAction = "/";
        ViewBag.SortOptions = new SelectList(new List<SelectListItem>()
        {
            new SelectListItem() { Value = "Name", Text = "Название" },
            new SelectListItem() { Value = "Price", Text = "Цена" },
            new SelectListItem() { Value = "Weight", Text = "Вес" },
        }, "Value", "Text", options.OrderPropertyName);
        ViewBag.SearchOptions = new SelectList(new List<SelectListItem>()
        {
            new SelectListItem() { Value = "Name", Text = "Название" },
            new SelectListItem() { Value = "Type", Text = "Тип" }
        }, "Value", "Text", options.SearchPropertyName);


        if (categoryId == 0)
        {
            ViewData["Title"] = "Головна";
            var products = _products.GetAllProducts(options);
            var vpm = new PagedList<ProductViewModel>(
                products.Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Image = p.Image
                }).AsQueryable(),
                products.Options
            );
            return View(vpm);
        }
        else
        {
            Category? currentCategory = await _categories.GetCategoryByIdAsync(categoryId);
            if (currentCategory == null)
                return BadRequest("Invalid categoryId");

            ViewBag.CategoryId = categoryId;
            ViewData["Title"] = currentCategory.Name;

            var products = _products.GetAllProductsByCategory(options, categoryId);
            PagedList<ProductViewModel> pvm = new PagedList<ProductViewModel>(
                products.Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Image = p.Image
                }).AsQueryable(), products.Options
            );
        
            return View(pvm);
        }
    }
    
    
    [Route("/product/{id}")]
    [HttpGet]
    public async Task<IActionResult> GetProduct(int id, string returnUrl = null)
    {
        Product? product = await _products.GetProductWithCategoryAsync(id);
        if (product == null)
            return NotFound();
        
        ViewBag.ReturnUrl = returnUrl;
        return View(new ProductViewModel()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Image = product.Image,
            Weight = product.Weight,
            Calories = product.Calories,
            Price = product.Price,
            Brand = product.Brand,
            Category = product.Category,
        });
    }
    
    public IActionResult Error(int statuscode)
    {
        if (statuscode == 404)
            return View("NotFound");
        
        return RedirectToAction("Index", "Home");
    }
}

