using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Order_Services;
using Order_Services.Models;
using Order_Services.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace Order_Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            try
            {
                var authToken = Request.Headers.Authorization.ToString();
                var order = await _orderService.CreateOrderAsync(userId, createDto, authToken);
                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetUserOrders()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var orders = await _orderService.GetUserOrdersAsync(userId);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var order = await _orderService.GetOrderByIdAsync(id, userId);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpGet("restaurant/{restaurantId}")]
        [Authorize(Roles = "Admin,RestaurantOwner")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetRestaurantOrders(int restaurantId)
        {
            var orders = await _orderService.GetRestaurantOrdersAsync(restaurantId);
            return Ok(orders);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin,RestaurantOwner")]
        public async Task<ActionResult<OrderDto>> UpdateOrderStatus(int id, UpdateOrderStatusDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var order = await _orderService.UpdateOrderStatusAsync(id, updateDto);
                if (order == null)
                    return NotFound();

                return Ok(order);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var result = await _orderService.CancelOrderAsync(id, userId);
            if (!result)
                return BadRequest("Order cannot be cancelled");

            return NoContent();
        }
    }




}
