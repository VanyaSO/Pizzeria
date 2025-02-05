using Microsoft.EntityFrameworkCore;
using Pizzeria.Data;
using Pizzeria.Interfaces;
using Pizzeria.Models;
using Pizzeria.Models.Pages;

namespace Pizzeria.Repositories;

public class CategoryRepository : ICategory
{
    private readonly ApplicationContext _context;

    public CategoryRepository(ApplicationContext context)
    {
        _context = context;
    }

    public PagedList<Category> GetAllCategories(QueryOptions options) =>
        new PagedList<Category>(_context.Categories, options);

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync() => await _context.Categories.ToListAsync(); 
    
    public async Task<Category?> GetCategoryByIdAsync(string id) =>
        await _context.Categories.FirstOrDefaultAsync(e => e.Id.ToString() == id);

    public async Task DeleteCategoryAsync(Category category)
    {
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }

    public async Task CreateCategoryAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public bool IsHasCategoryWithName(string name) => _context.Categories.Any(e => e.Name.Equals(name));
    
    public async Task UpdateCategoryAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }
    
}