using Order_Services.Models;

namespace Order_Services.Service
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(int userId, CreateOrderDto createDto, string authToken);
        Task<OrderDto?> GetOrderByIdAsync(int orderId, int userId);
        Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId);
        Task<IEnumerable<OrderDto>> GetRestaurantOrdersAsync(int restaurantId);
        Task<OrderDto?> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto updateDto);
        Task<bool> CancelOrderAsync(int orderId, int userId);
    }
}
