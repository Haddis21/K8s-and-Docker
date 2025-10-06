using Order_Services.Models;

namespace Order_Services.Service
{
    public interface IUserServiceClient
    {
        Task<UserDto?> GetUserAsync(int userId, string authToken);
    }
}
