using PromoService.Models.Users;

namespace PromoService.Services.Users;

public interface IUsersService
{
    Task<User> GetUser(long userId);
}