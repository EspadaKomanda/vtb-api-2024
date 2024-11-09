using PromoService.Database.Models;
using PromoService.Models.PromoApplication;
using PromoService.Models.PromoApplication.Requests;
using PromoService.Models.PromoApplication.Responses;
using PromoService.Models.Users;
using PromoService.Repositories;
using PromoService.Services.Promocode;
using PromoService.Services.Users;

namespace PromoService.Services.PromoApplication;

public class PromoApplicationService(IUnitOfWork unitOfWork, IUsersService usersService, ILogger<PromocodeService> logger) : IPromoApplicationService
{
    private readonly IUnitOfWork _uow = unitOfWork;
    private readonly IUsersService _usersService = usersService;
    private readonly ILogger<PromocodeService> _logger = logger;

    public GetMyPromoApplicationsResponse GetMyPromoApplications(long userId, GetMyPromoApplicationsRequest request)
    {
        List<UserPromo> applications;
        List<PromoAppliedProduct> appliedProducts;
        List<Promo> promos;
        try
        {
            _logger.LogDebug("Finding applications for user with id {UserId}", userId);
            applications = [.. _uow.UserPromoRepo.Find(p => p.UserId == userId)];
            appliedProducts = [.. _uow.PromoAppliedProductRepo.Find(p => applications.Any(a => a.Id == p.UserPromoId))];
            promos = [.. _uow.PromoRepo.Find(p => applications.Any(a => a.PromoId == p.Id))];

            List<PromocodeApplication> promocodeApplications = [];
            foreach (var promo in promos)
            {
                // FIXME: testing
                promocodeApplications.Add(new()
                {
                    PromocodeId = promo.Id,
                    TourIds = appliedProducts.Where(ap => ap.UserPromoId == applications.First(a => a.PromoId == promo.Id).Id).Select(ap => ap.TourId).ToList(),
                    EntertainmentIds = appliedProducts.Where(ap => ap.UserPromoId == applications.First(a => a.PromoId == promo.Id).Id).Select(ap => ap.EntertainmentId).ToList(),
                });
            }
            return (GetMyPromoApplicationsResponse)promocodeApplications;

        }
        catch
        {
            _logger.LogError("Failed to find applications for user with id {UserId}", userId);
            throw;
        }


    }

    public Task<RegisterPromoUseResponse> RegisterPromoUse(long userId, RegisterPromoUseRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<ValidatePromocodeApplicationResponse> ValidatePromocodeApplication(long userId, ValidatePromocodeApplicationRequest request)
    {
        User user;

        try
        {
            _logger.LogDebug("Finding user with id {UserId}", userId);
            user = await _usersService.GetUser(userId);

            // TODO: make checks
            return new ValidatePromocodeApplicationResponse
            {
                IsSuccess = true
            };
        }
        catch
        {
            _logger.LogError("Failed to find user with id {UserId}", userId);
            throw;
        }
    }
}
