using FoodOrderApp.Models;

namespace FoodOrderApp.Services
{
    public interface IOrderService
    {
        Task<OrderDto?> CreateOrderAsync(CreateOrderDto createOrderDto);
        Task<List<OrderDto>> GetUserOrdersAsync();
        Task<OrderDto?> GetOrderAsync(int orderId);
        Task<bool> CancelOrderAsync(int orderId);
    }
}
