using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Interfaces;
using Pizzeria.Models;
using Pizzeria.Models.Pages;
using Pizzeria.ViewModels;

namespace Pizzeria.Controllers;

[Authorize(Roles = "Admin,Editor")]
public class PanelController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public PanelController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }
    
    [Route("/panel")]
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    
    [Authorize(Roles = "Admin")]
    [Route("/panel/users")]
    [HttpGet]
    public IActionResult Users(QueryOptions options)
    {
        ViewBag.RoutAction = "/panel/users";
        ViewBag.SortOptions = new SelectList(new List<SelectListItem>()
        {
            new SelectListItem() { Value = "Email", Text = "Email" },
            new SelectListItem() { Value = "PhoneNumber", Text = "Номер телефона" },
            new SelectListItem() { Value = "Year", Text = "Год рождения" }
        }, "Value", "Text", options.OrderPropertyName);
        
        ViewBag.SearchOptions = new SelectList(new List<SelectListItem>()
        {
            new SelectListItem() { Value = "Email", Text = "Email" },
            new SelectListItem() { Value = "PhoneNumber", Text = "Номер телефона" },
            new SelectListItem() { Value = "Username", Text = "Имя" }
        }, "Value", "Text", options.SearchPropertyName);

        return View(new PagedList<User>(_userManager.Users, options));
    }

    [Authorize(Roles = "Admin")]
    [Route("/panel/create-update-user")]
    [HttpGet]
    public async Task<IActionResult> CreateOrUpdateUser(string? userId)
    {
        if (!string.IsNullOrEmpty(userId))
        {
            User? user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                CreateOrUpdateUserViewModel uvm = new CreateOrUpdateUserViewModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Year = user.Year,
                    Phone = user.PhoneNumber,
                    Password = "",
                    NewPassword = ""
                };

                return View(uvm);
            }
            return NotFound();
        }

        return View(new CreateOrUpdateUserViewModel());
    }
    
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    [Route("/panel/create-update-user")]
    [HttpPost]
    public async Task<IActionResult> CreateOrUpdateUser(CreateOrUpdateUserViewModel model)
    {
        if (string.IsNullOrEmpty(model.Id) && ModelState.IsValid)
        {
            User user = new User
                { Email = model.Email, UserName = model.Email, Year = model.Year, PhoneNumber = model.Phone };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Client");
                return RedirectToAction("Users");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        else if (ModelState.IsValid)
        {
            User? user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            if (!user.Email.Equals(model.Email) && await _userManager.FindByEmailAsync(model.Email) != null) 
            {
                ModelState.AddModelError("Email", "Такой email уже используется");
                return View(model);
            }
            

            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("NewPassword", "Неудалось обновит пароль");
                    return View(model);
                }
            }
            
            user.Email = model.Email;
            user.Year = model.Year;
            user.PhoneNumber = model.Phone;
            var resultUpdate = await _userManager.UpdateAsync(user);
            if (!resultUpdate.Succeeded)
            {
                ModelState.AddModelError("", "Произошла ошибка, попробуйте позже");
                return View(model);
            }
            
            return RedirectToAction("Users");
        }

        return View(model);
    }
    
    [Authorize(Roles = "Admin")]
    [Route("/panel/delete-user")]
    [HttpDelete]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        User? user = await _userManager.FindByIdAsync(userId);
        
        if (user == null) 
            return BadRequest(new { message = $"Пользователь с Id: {userId} не найден" });
        
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
            return BadRequest(new { message = "Не удалось удалить пользователя, попробуйте позже" });

        return Ok();
    }
    
    [Authorize(Roles = "Admin")]
    [Route("/panel/user-roles")]
    [HttpGet]
    public async Task<IActionResult> EditRoles(string userId)
    {
        User? user = await _userManager.FindByIdAsync(userId);
        if (user == null) 
            return NotFound(); 
        
        var userRoles = await _userManager.GetRolesAsync(user);
        var allRoles = await _roleManager.Roles.ToListAsync();
        ChangeRoleViewModel model = new ChangeRoleViewModel
        {
            UserId = user.Id,
            UserEmail = user.Email,
            UserRoles = userRoles,
            AllRoles = allRoles
        };
        return View(model);
    }
 
    [Authorize(Roles = "Admin")]
    [Route("/panel/user-roles")]
    [HttpPost]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> EditRoles(string userId, List<string> roles)
    {
        User user = await _userManager.FindByIdAsync(userId);
        if (user == null) 
            return NotFound();
        
        var userRoles = await _userManager.GetRolesAsync(user);
        var addedRoles = roles.Except(userRoles);
        var removedRoles = userRoles.Except(roles);

        await _userManager.AddToRolesAsync(user, addedRoles);
        await _userManager.RemoveFromRolesAsync(user, removedRoles);

        return RedirectToAction("Users");
    }
}




