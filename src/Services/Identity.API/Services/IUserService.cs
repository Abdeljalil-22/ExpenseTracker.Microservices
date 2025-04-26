using Identity.API.Models;

namespace Identity.API.Services
{
    public interface IUserService
    {
        Task<UserModel> RegisterAsync(RegisterModel model);
        Task<UserModel> LoginAsync(LoginModel model);
        Task<UserModel> GetUserByIdAsync(string userId);
    }
}