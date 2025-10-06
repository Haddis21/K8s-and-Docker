using Microsoft.EntityFrameworkCore;
using Order_Services.Models;

namespace Order_Services.Service
{
    public class OrderServiceImpl : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly IUserServiceClient _userServiceClient;
        private readonly IRestaurantServiceClient _restaurantServiceClient;

        public OrderServiceImpl(
            AppDbContext context,
            IUserServiceClient userServiceClient,
            IRestaurantServiceClient restaurantServiceClient)
        {
            _context = context;
            _userServiceClient = userServiceClient;
            _restaurantServiceClient = restaurantServiceClient;
        }

        public async Task<OrderDto> CreateOrderAsync(int userId, CreateOrderDto createDto, string authToken)
        {
            // Validate user exists
            var user = await _userServiceClient.GetUserAsync(userId, authToken);
            if (user == null)
                throw new ArgumentException("User not found");

            // Validate restaurant and get menu items
            var restaurant = await _restaurantServiceClient.GetRestaurantAsync(createDto.RestaurantId);
            if (restaurant == null)
                throw new ArgumentException("Restaurant not found");

            // Validate menu items and calculate totals
            var orderItems = new List<OrderItem>();
            decimal subtotal = 0;

            foreach (var item in createDto.Items)
            {
                var menuItem = restaurant.MenuItems.FirstOrDefault(m => m.Id == item.MenuItemId);
                if (menuItem == null || !menuItem.IsAvailable)
                    throw new ArgumentException($"Menu item {item.MenuItemId} not available");

                var orderItem = new OrderItem
                {
                    MenuItemId = item.MenuItemId,
                    MenuItemName = menuItem.Name,
                    Price = menuItem.Price,
                    Quantity = item.Quantity,
                    SpecialInstructions = item.SpecialInstructions
                };

                orderItems.Add(orderItem);
                subtotal += menuItem.Price * item.Quantity;
            }

            // Calculate totals
            var deliveryFee = restaurant.DeliveryFee;
            var tax = subtotal * 0.12m; // 12% tax
            var total = subtotal + deliveryFee + tax;

            // Create order
            var order = new Order
            {
                UserId = userId,
                RestaurantId = createDto.RestaurantId,
                Items = orderItems,
                Subtotal = subtotal,
                DeliveryFee = deliveryFee,
                Tax = tax,
                Total = total,
                Status = OrderStatus.Pending,
                DeliveryAddress = new Address
                {
                    Street = createDto.DeliveryAddress.Street,
                    City = createDto.DeliveryAddress.City,
                    PostalCode = createDto.DeliveryAddress.PostalCode,
                    Latitude = createDto.DeliveryAddress.Latitude,
                    Longitude = createDto.DeliveryAddress.Longitude
                },
                SpecialInstructions = createDto.SpecialInstructions,
                EstimatedDeliveryTime = DateTime.UtcNow.AddMinutes(restaurant.EstimatedDeliveryTime)
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return await MapToDto(order, restaurant.Name);
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int orderId, int userId)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null)
                return null;

            // Get restaurant name
            var restaurant = await _restaurantServiceClient.GetRestaurantAsync(order.RestaurantId);
            var restaurantName = restaurant?.Name ?? "Unknown Restaurant";

            return await MapToDto(order, restaurantName);
        }

        public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId)
        {
            var orders = await _context.Orders
                .Include(o => o.Items)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            var orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                var restaurant = await _restaurantServiceClient.GetRestaurantAsync(order.RestaurantId);
                var restaurantName = restaurant?.Name ?? "Unknown Restaurant";
                orderDtos.Add(await MapToDto(order, restaurantName));
            }

            return orderDtos;
        }

        public async Task<IEnumerable<OrderDto>> GetRestaurantOrdersAsync(int restaurantId)
        {
            var orders = await _context.Orders
                .Include(o => o.Items)
                .Where(o => o.RestaurantId == restaurantId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            var restaurant = await _restaurantServiceClient.GetRestaurantAsync(restaurantId);
            var restaurantName = restaurant?.Name ?? "Unknown Restaurant";

            return orders.Select(order => MapToDto(order, restaurantName).Result);
        }

        public async Task<OrderDto?> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto updateDto)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return null;

            // Validate status transition
            if (!IsValidStatusTransition(order.Status, updateDto.Status))
                throw new ArgumentException("Invalid status transition");

            order.Status = updateDto.Status;

            if (updateDto.EstimatedDeliveryTime.HasValue)
                order.EstimatedDeliveryTime = updateDto.EstimatedDeliveryTime;

            if (updateDto.Status == OrderStatus.Delivered)
                order.CompletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var restaurant = await _restaurantServiceClient.GetRestaurantAsync(order.RestaurantId);
            var restaurantName = restaurant?.Name ?? "Unknown Restaurant";

            return await MapToDto(order, restaurantName);
        }

        public async Task<bool> CancelOrderAsync(int orderId, int userId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
            if (order == null)
                return false;

            // Can only cancel if not yet confirmed
            if (order.Status != OrderStatus.Pending)
                return false;

            order.Status = OrderStatus.Cancelled;
            order.CompletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        private static bool IsValidStatusTransition(OrderStatus current, OrderStatus next)
        {
            return (current, next) switch
            {
                (OrderStatus.Pending, OrderStatus.Confirmed) => true,
                (OrderStatus.Pending, OrderStatus.Cancelled) => true,
                (OrderStatus.Confirmed, OrderStatus.Preparing) => true,
                (OrderStatus.Confirmed, OrderStatus.Cancelled) => true,
                (OrderStatus.Preparing, OrderStatus.Ready) => true,
                (OrderStatus.Ready, OrderStatus.OutForDelivery) => true,
                (OrderStatus.OutForDelivery, OrderStatus.Delivered) => true,
                _ => false
            };
        }

        private static async Task<OrderDto> MapToDto(Order order, string restaurantName)
        {
            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                RestaurantId = order.RestaurantId,
                RestaurantName = restaurantName,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    MenuItemId = i.MenuItemId,
                    MenuItemName = i.MenuItemName,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    SpecialInstructions = i.SpecialInstructions
                }).ToList(),
                Subtotal = order.Subtotal,
                DeliveryFee = order.DeliveryFee,
                Tax = order.Tax,
                Total = order.Total,
                Status = order.Status,
                DeliveryAddress = new AddressDto
                {
                    Street = order.DeliveryAddress.Street,
                    City = order.DeliveryAddress.City,
                    PostalCode = order.DeliveryAddress.PostalCode,
                    Latitude = order.DeliveryAddress.Latitude,
                    Longitude = order.DeliveryAddress.Longitude
                },
                SpecialInstructions = order.SpecialInstructions,
                CreatedAt = order.CreatedAt,
                EstimatedDeliveryTime = order.EstimatedDeliveryTime,
                CompletedAt = order.CompletedAt
            };
        }
    }
}
