using Pizzeria.Models;
using Pizzeria.Models.Pages;

namespace Pizzeria.Interfaces;

public interface IProduct
{
    PagedList<Product> GetAllProducts(QueryOptions options);
    Task<Product?> GetProductAsync(int id);
    Task<Product> GetProductWithCategoryAsync(int id);
    Task AddProductAsync(Product product);
    Task DeleteProductAsync(Product product);
    Task EditProductAsync(Product product);
    Task<IEnumerable<Product>> GetTenSimilarProductsAsync(string categoryName);
    PagedList<Product> GetAllProductsByCategory(QueryOptions options, int categoryId);
}