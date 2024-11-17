using PromoService.Database.Models;
using PromoService.Exceptions.PromoApplication;
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

    public GetMyPromoApplicationsResponse GetMyPromoApplications(GetMyPromoApplicationsRequest request)
    {
        List<UserPromo> applications;
        List<PromoAppliedProduct> appliedProducts;
        List<Promo> promos;
        try
        {
            _logger.LogDebug("Finding applications for user with id {UserId}", request.UserId);
            applications = [.. _uow.UserPromoRepo.Find(p => p.UserId == request.UserId)];
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
            _logger.LogError("Failed to find applications for user with id {UserId}", request.UserId);
            throw;
        }


    }

    public async Task<RegisterPromoUseResponse> RegisterPromoUse(RegisterPromoUseRequest request)
    {
        using var transaction = _uow.BeginTransaction();
        try
        {  
            if ((await ValidatePromocodeApplication( (ValidatePromocodeApplicationRequest)request)).IsSuccess)
            {
                Promo promo = await _uow.PromoRepo.FindOneAsync(p => p.Code == request.PromoCode);

                _logger.LogDebug("Adding application to database");

                UserPromo userPromo;
                try 
                {
                    userPromo = await _uow.UserPromoRepo.FindOneAsync(p => p.UserId == request.UserId && p.PromoId == promo.Id);
                    _logger.LogDebug("Application already exists in database, acquired it");
                }
                catch
                {
                    _logger.LogDebug("Application not found in database, creating it");
                    try
                    {
                        userPromo = new()
                        {
                            UserId = request.UserId,
                            PromoId = promo.Id
                        };
                        await _uow.UserPromoRepo.AddAsync(userPromo);
                    }
                    catch
                    {
                        _logger.LogError("Failed to add application to database");
                        throw;
                    }
                }

                _logger.LogInformation("Added application to database. Application id: {ApplicationId}", userPromo.Id);
                _logger.LogDebug("Adding applied products to database");
                foreach (var tourId in request.TourIds)
                {
                    var appliedProduct = new PromoAppliedProduct()
                    {
                        UserPromoId = userPromo.Id,
                        TourId = tourId
                    };
                    await _uow.PromoAppliedProductRepo.AddAsync(appliedProduct);
                }
                foreach (var entertainmentId in request.EntertainmentIds)
                {
                    var appliedProduct = new PromoAppliedProduct()
                    {
                        UserPromoId = userPromo.Id,
                        EntertainmentId = entertainmentId
                    };
                    await _uow.PromoAppliedProductRepo.AddAsync(appliedProduct);
                }

                _logger.LogInformation("Added applied products to database, saving");
                transaction.SaveAndCommit();

                return new()
                {
                    PromoApplicationId = userPromo.Id
                };
            }
            else
            {
                _logger.LogDebug("Application is invalid");
                throw new PromocodeInapplicapleException("Application is invalid");
            }
        }
        catch
        {
            _logger.LogError("Failed to add application to database, rolling back transaction");
            transaction.Rollback();
            throw;
        }
    }

    public async Task<ValidatePromocodeApplicationResponse> ValidatePromocodeApplication(ValidatePromocodeApplicationRequest request)
    {
        User user;
        Promo promo;

        try
        {
            _logger.LogDebug("Finding user with id {UserId}", request.UserId);
            user = await _usersService.GetUser(request.UserId);
        }
        catch
        {
            _logger.LogError("Failed to find user with id {UserId}", request.UserId);
            throw;
        }

        try
        {
            _logger.LogDebug("Finding promo with code {PromoId}", request.PromoCode);
            promo = await _uow.PromoRepo.FindOneAsync(p => p.Code == request.PromoCode);
        }
        catch
        {
            _logger.LogError("Failed to find promo with code {PromoId}", request.PromoCode);
            throw;
        }

        // Is deleted check
        _logger.LogDebug("Checking if promo is deleted");
        if (promo.Deleted)
        {
            _logger.LogDebug("Promocode is deleted");
            throw new PromocodeInapplicapleException("Promocode is deleted");
        }

        // Account age check
        _logger.LogDebug("Checking account age");
        var accountAge = (DateTime.Now - user.CreationDate).Days / 365;
        if (promo.MinAccountAge != null && accountAge < promo.MinAccountAge || promo.MaxAccountAge != null && accountAge > promo.MaxAccountAge)
        {
            _logger.LogDebug("Account age is out of range");
            throw new PromocodeInapplicapleException("Account age is out of range");
        }

        // Max per user check
        _logger.LogDebug("Checking max per user");
        var userPromos = _uow.UserPromoRepo.Find(p => p.UserId == request.UserId);
        if (promo.MaxPerUser != null && userPromos.Count() >= promo.MaxPerUser)
        {
            _logger.LogDebug("Limit on uses of this promocode has been reached by the user");
            throw new PromocodeInapplicapleException("Limit on uses of this promocode has been reached by the user");
        }

        // Max per everyone check
        _logger.LogDebug("Checking max per everyone");
        if (promo.MaxPerEveryone != null && _uow.UserPromoRepo.Find(p => p.PromoId == promo.Id).Count() >= promo.MaxPerEveryone)
        {
            _logger.LogDebug("Limit on uses of this promocode has been reached by everyone");
            throw new PromocodeInapplicapleException("Limit on uses of this promocode has been reached by everyone");
        }

        // Active date check
        _logger.LogDebug("Checking active date");
        if (promo.StartDate != null && DateTime.Now < promo.StartDate || promo.EndDate != null && DateTime.Now > promo.EndDate)
        {
            _logger.LogDebug("Promocode is not yet active or already expired");
            throw new PromocodeInapplicapleException("Promocode is not yet active or already expired");
        }
        _logger.LogDebug("Promocode {PromoId} for user {UserId} passed all checks", promo.Id, request.UserId);
        return new()
        {
            IsSuccess = true
        };
    }
}
