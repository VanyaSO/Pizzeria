using Microsoft.EntityFrameworkCore;
using Pizzeria.Models;
using Pizzeria.Models.Pages;
using Pizzeria.Views.Panel;

namespace Pizzeria.Interfaces;

public interface ICategory
{
    public PagedList<Category> GetAllCategories(QueryOptions options);
    public Task<IEnumerable<Category>> GetAllCategoriesAsync();
    public Task<Category?> GetCategoryByIdAsync(string id);
    public Task DeleteCategoryAsync(Category category);
    public Task CreateCategoryAsync(Category category);
    public bool IsHasCategoryWithName(string name);
    public Task UpdateCategoryAsync(Category category);
}