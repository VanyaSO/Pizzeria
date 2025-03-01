using Microsoft.AspNetCore.Mvc;
using Pizzeria.Interfaces;
using Pizzeria.Models;
using Pizzeria.ViewModels.Home;

namespace Pizzeria.ViewComponents;

public class SimilarProductsViewComponent : ViewComponent
{
    private readonly IProduct _products;

    public SimilarProductsViewComponent(IProduct products)
    {
        _products = products;
    }

    public async Task<IViewComponentResult> InvokeAsync(string categoryName)
    {
        var products = await _products.GetTenSimilarProductsAsync(categoryName);
        var pvm = products.Select(p => new ProductViewModel()
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Image = p.Image,
        });
        
        return View("SimilarProducts", pvm);
    }
}