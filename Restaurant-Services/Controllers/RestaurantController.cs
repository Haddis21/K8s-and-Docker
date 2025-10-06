using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Restaurant_Services.Models;
using Restaurant_Services.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Restaurant_Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantsController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetRestaurants()
        {
            var restaurants = await _restaurantService.GetAllRestaurantsAsync();
            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantDto>> GetRestaurant(int id)
        {
            var restaurant = await _restaurantService.GetRestaurantByIdAsync(id);
            if (restaurant == null)
                return NotFound();

            return Ok(restaurant);
        }

        [HttpPost]
        public async Task<ActionResult<RestaurantDto>> CreateRestaurant(CreateRestaurantDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var restaurant = await _restaurantService.CreateRestaurantAsync(createDto);
            return CreatedAtAction(nameof(GetRestaurant), new { id = restaurant.Id }, restaurant);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RestaurantDto>> UpdateRestaurant(int id, CreateRestaurantDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var restaurant = await _restaurantService.UpdateRestaurantAsync(id, updateDto);
            if (restaurant == null)
                return NotFound();

            return Ok(restaurant);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            var result = await _restaurantService.DeleteRestaurantAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> SearchRestaurants([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Query parameter is required");

            var restaurants = await _restaurantService.SearchRestaurantsAsync(query);
            return Ok(restaurants);
        }

        [HttpGet("{id}/menu")]
        public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetMenuItems(int id)
        {
            var menuItems = await _restaurantService.GetMenuItemsAsync(id);
            return Ok(menuItems);
        }

        [HttpPost("{id}/menu/items")]
        public async Task<ActionResult<MenuItemDto>> AddMenuItem(int id, CreateMenuItemDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var menuItem = await _restaurantService.AddMenuItemAsync(id, createDto);
            return CreatedAtAction(nameof(GetMenuItems), new { id }, menuItem);
        }

        [HttpPut("{id}/menu/items/{itemId}")]
        public async Task<ActionResult<MenuItemDto>> UpdateMenuItem(int id, int itemId, CreateMenuItemDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var menuItem = await _restaurantService.UpdateMenuItemAsync(id, itemId, updateDto);
            if (menuItem == null)
                return NotFound();

            return Ok(menuItem);
        }

        [HttpDelete("{id}/menu/items/{itemId}")]
        public async Task<IActionResult> DeleteMenuItem(int id, int itemId)
        {
            var result = await _restaurantService.DeleteMenuItemAsync(id, itemId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }


}
