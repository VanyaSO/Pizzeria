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
    public HomeController(IProduct products)
    {
        _products = products;
    }
    
    [Route("/")]
    [HttpGet]
    public IActionResult Index(QueryOptions options)
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

        var product = _products.GetAllProducts(options);
        PagedList<ProductViewModel> pvm = new PagedList<ProductViewModel>(
            product.Select(p => new ProductViewModel
            {
                Id = p.Id.ToString(),
                Name = p.Name,
                Price = p.Price,
                Image = p.Image
            }).AsQueryable(), product.Options
        );

        return View(pvm);
    }
    
    [Route("/product/{id}")]
    [HttpGet]
    public async Task<IActionResult> GetProduct(string id, string returnUrl = null)
    {
        Product? product = await _products.GetProductWithCategoryAsync(id);
        if (product == null)
            return NotFound();
        
        ViewBag.ReturnUrl = returnUrl;
        return View(new ProductViewModel()
        {
            Id = product.Id.ToString(),
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

}