using PromoService.Models.Users;

namespace PromoService.Services.Users;

public class UsersService(ILogger<UsersService> logger) : IUsersService
{
    public async Task<User> GetUser(long userId)
    {
        // TODO: implementation REQUIRED
        throw new NotImplementedException();

        return new User();
    }
}