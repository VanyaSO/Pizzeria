using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pizzeria.Interfaces;
using Pizzeria.Models;
using Pizzeria.Models.Pages;
using Pizzeria.ViewModels;

namespace Pizzeria.Controllers;

public class CategoryController : Controller
{
    private readonly ICategory _categories;
    public readonly IWebHostEnvironment _appEnvironment;

    public CategoryController(ICategory categories, IWebHostEnvironment appEnvironment)
    {
        _categories = categories;
        _appEnvironment = appEnvironment;
    }
    
    [Route("/panel/categories")]
    [HttpGet]
    public IActionResult Categories(QueryOptions options)
    {
        ViewBag.RoutAction = "/panel/categories";
        ViewBag.SortOptions = new SelectList(new List<SelectListItem>()
        {
            new SelectListItem() { Value = "Name", Text = "Название" },
            new SelectListItem() { Value = "DateOfPublication", Text = "Дата публикации" },
        }, "Value", "Text", options.OrderPropertyName);
        ViewBag.SearchOptions = new SelectList(new List<SelectListItem>()
        {
            new SelectListItem() { Value = "Name", Text = "Name" },
            new SelectListItem() { Value = "DateOfPublication", Text = "Дата публикации" },
            new SelectListItem() { Value = "Description", Text = "Описание" }
        }, "Value", "Text", options.SearchPropertyName);
        
        return View(_categories.GetAllCategories(options));
    }
    
    [Route("/panel/delete-category")]
    [HttpDelete]
    public async Task<IActionResult> DeleteCategory(string categoryId)
    {
        Category? category = await _categories.GetCategoryByIdAsync(categoryId);
        
        if (category == null) 
            return BadRequest(new { message = $"Категория с Id: {categoryId} не найден" });
        
        if (category.Image != null)
        {
            if (System.IO.File.Exists(_appEnvironment.WebRootPath + category.Image))
            {
                System.IO.File.Delete(_appEnvironment.WebRootPath + category.Image);
            }
        }
        
        await _categories.DeleteCategoryAsync(category);
        return Ok();
    }
    
    
    [Route("/panel/create-update-category")]
    [HttpGet]
    public async Task<IActionResult> CreateOrUpdateCategory(string? categoryId)
    {
        if (!string.IsNullOrEmpty(categoryId))
        {
            Category? category = await _categories.GetCategoryByIdAsync(categoryId);
            if (category != null)
            {
                CreateOrUpdateCategoryViewModel cvm = new CreateOrUpdateCategoryViewModel()
                {
                    Id = category.Id.ToString(),
                    Name = category.Name,
                    Description = category.Description,
                    Image = category.Image
                };

                return View(cvm);
            }
            return NotFound();
        }

        return View(new CreateOrUpdateCategoryViewModel());
    }
    
    [ValidateAntiForgeryToken]
    [Route("/panel/create-update-category")]
    [HttpPost]
    public async Task<IActionResult> CreateOrUpdateCategory(CreateOrUpdateCategoryViewModel model)
    {
        if (string.IsNullOrEmpty(model.Id) && ModelState.IsValid)
        {
            if (_categories.IsHasCategoryWithName(model.Name))
            {
                ModelState.AddModelError("Name", "Категория с таким именем уже существует");
                return View(model);
            }
                
            string? fileImageName = null, imagePath = null;
            if (model.File != null)
            {
                fileImageName = model.File.FileName;
 
                if (fileImageName.Contains("\\"))
                {
                    fileImageName = fileImageName.Substring(fileImageName.LastIndexOf('\\') + 1);
                }
                else if (fileImageName.Contains("/"))
                {
                    fileImageName = fileImageName.Substring(fileImageName.LastIndexOf('/') + 1);
                }
 
                imagePath = "/categoryFiles/" + Guid.NewGuid() + fileImageName;
 
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + imagePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(fileStream);
                }
            }

            await _categories.CreateCategoryAsync(new Category()
            {
                Name = model.Name,
                Description = model.Description,
                Image = imagePath
            });

            return RedirectToAction("Categories");
        }
        else
        {
            Category? category = await _categories.GetCategoryByIdAsync(model.Id);
            if (category == null) 
                return NotFound();
            
            string? fileImageName = null, imagePath = null;
            if (model.File != null)
            {
                if (System.IO.File.Exists(_appEnvironment.WebRootPath + model.Image))
                {
                    System.IO.File.Delete(_appEnvironment.WebRootPath + model.Image);
                }
 
                fileImageName = model.File.FileName;
 
                if (fileImageName.Contains("\\"))
                {
                    fileImageName = fileImageName.Substring(fileImageName.LastIndexOf('\\') + 1);
                }
                else if (fileImageName.Contains("/"))
                {
                    fileImageName = fileImageName.Substring(fileImageName.LastIndexOf('/') + 1);
                }
 
                imagePath = "/categoryFiles/" + Guid.NewGuid() + fileImageName;
 
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + imagePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(fileStream);
                }
            }
            else
            {
                imagePath = category.Image;
            }

            category.Name = model.Name;
            category.Description = model.Description;
            category.Image = imagePath;
            await _categories.UpdateCategoryAsync(category);

            return RedirectToAction("Categories");
        }
    }
}