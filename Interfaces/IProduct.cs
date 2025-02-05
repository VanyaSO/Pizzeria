using Pizzeria.Models;
using Pizzeria.Models.Pages;

namespace Pizzeria.Interfaces;

public interface IProduct
{
    PagedList<Product> GetAllProducts(QueryOptions options);
    Task<Product> GetProductAsync(string id);
    Task<Product> GetProductWithCategoryAsync(string id);
    Task AddProductAsync(Product product);
    Task DeleteProductAsync(Product product);
    Task EditProductAsync(Product product);
}