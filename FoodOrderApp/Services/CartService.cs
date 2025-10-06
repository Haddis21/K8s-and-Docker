using FoodOrderApp.Models;

namespace FoodOrderApp.Services
{
    public class CartService
    {
        private Cart _cart = new();

        public event Action? OnCartChanged;

        public Cart GetCart() => _cart;

        public void AddItem(int restaurantId, string restaurantName, MenuItemDto menuItem, int quantity = 1)
        {
            // Clear cart if switching restaurants
            if (_cart.RestaurantId != 0 && _cart.RestaurantId != restaurantId)
            {
                ClearCart();
            }

            _cart.RestaurantId = restaurantId;
            _cart.RestaurantName = restaurantName;

            var existingItem = _cart.Items.FirstOrDefault(i => i.MenuItemId == menuItem.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                _cart.Items.Add(new CartItem
                {
                    MenuItemId = menuItem.Id,
                    MenuItemName = menuItem.Name,
                    Price = menuItem.Price,
                    Quantity = quantity
                });
            }

            OnCartChanged?.Invoke();
        }

        public void UpdateQuantity(int menuItemId, int quantity)
        {
            var item = _cart.Items.FirstOrDefault(i => i.MenuItemId == menuItemId);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    _cart.Items.Remove(item);
                }
                else
                {
                    item.Quantity = quantity;
                }

                OnCartChanged?.Invoke();
            }
        }

        public void RemoveItem(int menuItemId)
        {
            var item = _cart.Items.FirstOrDefault(i => i.MenuItemId == menuItemId);
            if (item != null)
            {
                _cart.Items.Remove(item);
                OnCartChanged?.Invoke();
            }
        }

        public void ClearCart()
        {
            _cart = new Cart();
            OnCartChanged?.Invoke();
        }

        public int GetItemCount() => _cart.ItemCount;
        public decimal GetSubtotal() => _cart.Subtotal;
    }
}
