using Pizzeria.Models;
using Pizzeria.Models.Pages;

namespace Pizzeria.Interfaces;

public interface IOrder
{
    PagedList<Order> GetAllOrdersWithDetails(QueryOptions options);
    PagedList<Order> GetAllOrdersByUserWithDetails(QueryOptions options, string userId);
    Task<Order> GetOrderAsync(int id);
 
    Task AddOrderAsync(Order order);
    Task EditOrderAsync(Order order);
    Task RemoveOrderAsync(Order order);
}