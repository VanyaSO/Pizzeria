using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pizzeria.Interfaces;
using Pizzeria.Models;
using Pizzeria.Models.Pages;
using Pizzeria.Repositories;
using Pizzeria.ViewModels.Dish;
using Pizzeria.ViewModels.Order;

namespace Pizzeria.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly IOrder _orders;
    private readonly CartRepository _cart;
    private readonly UserManager<User> _userManager;

    public OrderController(IOrder orders, CartRepository cart, UserManager<User> userManager)
    {
        _orders = orders;
        _cart = cart;
        _userManager = userManager;
    }

    [Route("/orders")]
    [HttpGet]
    public async Task<IActionResult> Index(QueryOptions options)
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        if (user != null)
        {
            ViewBag.SortOptions = new SelectList(new List<SelectListItem>()
            {
                new SelectListItem() { Value = "CreatedAt", Text = "Дата" },
            }, "Value", "Text", options.OrderPropertyName);
            
            return View(_orders.GetAllOrdersByUserWithDetails(options, user.Id));
        }
    
        return RedirectToAction("Index", "Home");
    }
    
    [Route("/checkout")]
    public IActionResult Checkout()
    {
        return View();
    }

    [HttpPost]
    [Route("/checkout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(CreateUpdateOrderViewModel model)
    {
        if (ModelState.IsValid)
        {
            var cartItems = await _cart.GetShopCartItemsAsync();

            if (cartItems.Count() > 0)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user != null)
                {

                    Order newOrder = new Order()
                    {
                        UserId = user.Id,
                        Fio = model.Fio,
                        Phone = model.Phone,
                        Email = model.Email,
                        City = model.City,
                        Address = model.Address,
                        OrderDetails = cartItems.Select(e => new OrderDetails()
                        {
                            ProductId = e.ProductId,
                            Quantity = e.Count
                        }).ToList()
                    };

                    await _orders.AddOrderAsync(newOrder);
                    await _cart.ClearCartAsync();
                    
                    return RedirectToAction("Index");   
                }
            }
            
            ModelState.AddModelError("", "Что-то пошло не так, попробуйте еще раз.");
            return View(model);
        }

        return View(model);
    }
    
    [Route("/panel/orders")]
    [HttpGet]
    public IActionResult PanelOrders(QueryOptions options)
    {
        ViewBag.RoutAction = "/panel/orders";
        ViewBag.SortOptions = new SelectList(new List<SelectListItem>()
        {
            new SelectListItem() { Value = "CreatedAt", Text = "Дата" },
        }, "Value", "Text", options.OrderPropertyName);
        ViewBag.SearchOptions = new SelectList(new List<SelectListItem>()
        {
            new SelectListItem() { Value = "Fio", Text = "ФИО" },
            new SelectListItem() { Value = "Phone", Text = "Номер телефона" },
            new SelectListItem() { Value = "Email", Text = "Email" },
        }, "Value", "Text", options.SearchPropertyName);

        return View(_orders.GetAllOrdersWithDetails(options));
    }
    
    [Route("/panel/edit-order")]
    [HttpGet]
    public async Task<IActionResult> EditOrder(int orderId)
    {
        var order = await _orders.GetOrderAsync(orderId);
        if (order == null)
            return NotFound();
        
        return View(new CreateUpdateOrderViewModel()
        {
            Id = order.Id,
            Fio = order.Fio,
            Phone = order.Phone,
            Email = order.Email,
            City = order.City,
            Address = order.Address,
        });
    }
    
    [Route("/panel/edit-order")]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> EditOrder(CreateUpdateOrderViewModel model)
    {
        if (ModelState.IsValid)
        {
            var order = await _orders.GetOrderAsync(model.Id);
            if (order == null)
            {
                ModelState.AddModelError("", "Произошла ошибка, попробуйте еще раз.");
                return View(model);
            }
            
            order.Fio = model.Fio;
            order.Phone = model.Phone;
            order.Email = model.Email;
            order.City = model.City;
            order.Address = model.Address;
            
            await _orders.EditOrderAsync(order);

            return RedirectToAction("PanelOrders");
        }
        
        return View(model);
    }
    
    [Route("/panel/delete-order")]
    [HttpDelete]
    public async Task<IActionResult> DeleteOrder(int orderId)
    {
        Order? currentProduct = await _orders.GetOrderAsync(orderId);
        if (currentProduct == null)
            return BadRequest();

        await _orders.RemoveOrderAsync(currentProduct);
        return Ok();
    }
}























