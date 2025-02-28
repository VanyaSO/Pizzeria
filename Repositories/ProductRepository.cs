using Microsoft.EntityFrameworkCore;
using Pizzeria.Data;
using Pizzeria.Interfaces;
using Pizzeria.Models;
using Pizzeria.Models.Pages;

namespace Pizzeria.Repositories;

public class ProductRepository : IProduct
{
    private readonly ApplicationContext _applicationContext;


    public ProductRepository(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }


    public PagedList<Product> GetAllProducts(QueryOptions options)
    {
        return new PagedList<Product>(_applicationContext.Products.Include(e => e.Category), options);
    }


    public async Task AddProductAsync(Product product)
    {
        await _applicationContext.Products.AddAsync(product);
        await _applicationContext.SaveChangesAsync();
    }


    public async Task DeleteProductAsync(Product product)
    {
        _applicationContext.Products.Remove(product);
        await _applicationContext.SaveChangesAsync();
    }


    public async Task EditProductAsync(Product product)
    {
        _applicationContext.Entry(product).State = EntityState.Modified;
        _applicationContext.Entry(product).Property(e => e.DateOfPublication).IsModified = false;
        await _applicationContext.SaveChangesAsync();
    }


    public async Task<Product?> GetProductAsync(int id)
    {
        return await _applicationContext.Products.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Product> GetProductWithCategoryAsync(int id)
    {
        return await _applicationContext.Products.Include(e => e.Category).AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Product>> GetTenSimilarProductsAsync(string categoryName)
    {
        return await _applicationContext.Products
            .Where(p => p.Category.Name.Equals(categoryName))
            .OrderBy(_ => Guid.NewGuid())
            .Take(10)
            .ToListAsync();
    }

    public PagedList<Product> GetAllProductsByCategory(QueryOptions options, int categoryId)
    {
        return new PagedList<Product>(_applicationContext.Products.Include(e => e.Category).Where(e => e.CategoryId == categoryId), options);
    }
}