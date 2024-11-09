using PromoService.Database.Models;
using PromoService.Models.Promocode.Requests;
using PromoService.Models.Promocode.Responses;
using PromoService.Repositories;

namespace PromoService.Services.Promocode;

public class PromocodeService(IUnitOfWork unitOfWork, ILogger<PromocodeService> logger) : IPromocodeService
{
    private readonly IUnitOfWork _uow = unitOfWork;
    private readonly ILogger<PromocodeService> _logger = logger;

    public async Task<CreatePromocodeResponse> CreatePromocode(CreatePromocodeRequest request)
    {
        Promo promo = new()
        {
            Code = request.Code ?? Guid.NewGuid().ToString()[..6],
            Description = request.Description,
            Discount = request.Discount,
            IsActive = request.IsActive,
            IsStackable = request.IsStackable,
            MaxAmountDiscounted = request.MaxAmountDiscounted,
            MinAccountAge = request.MinAccountAge,
            MaxAccountAge = request.MaxAccountAge,
            MaxPerUser = request.MaxPerUser,
            MaxPerEveryone = request.MaxPerEveryone,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
        };

        using var transaction = _uow.BeginTransaction();
        try
        {
            _logger.LogDebug("Adding promo to database");
            await _uow.PromoRepo.AddAsync(promo);
            transaction.SaveAndCommit();
            _logger.LogInformation("Created new promo with id {PromoId}", promo.Id);
            return new()
            {
                PromocodeId = promo.Id
            };
        }
        catch
        {
            _logger.LogError("Failed to add promo to database, rolling back transaction");
            transaction.Rollback();
            throw;
        }
    }

    public async Task<DeletePromocodeResponse> DeletePromocode(DeletePromocodeRequest request)
    {
        using var transaction = _uow.BeginTransaction();
        try
        {
            _logger.LogDebug("Deleting promo from database");

            var promo = await _uow.PromoRepo.FindOneAsync(p => p.Id == request.PromoId);
            promo.Deleted = true;
            _uow.PromoRepo.Update(promo);

            transaction.SaveAndCommit();
            _logger.LogInformation("Marked promo with id {PromoId} as deleted", request.PromoId);
            return new(){IsSuccess = true};
        }
        catch
        {
            _logger.LogError("Failed to delete promo from database, rolling back transaction");
            transaction.Rollback();
            throw;
        }
    }

    public async Task<GetActivePromocodesResponse> GetActivePromocodes(GetActivePromocodesResponse request)
    {
        try
        {
            _logger.LogDebug("Getting active promocodes from database");
            var promocodes = _uow.PromoRepo.Find(p => p.IsActive && !p.Deleted).ToList();

            _logger.LogDebug("Found {Count} active promocodes, returning", promocodes.Count);
            return (GetActivePromocodesResponse)promocodes;
        }
        catch
        {
            _logger.LogError("Failed to get active promocodes from database");
            throw;
        }
    }

    public async Task<GetPromocodesResponse> GetPromocodes(GetPromocodesRequest request)
    {
        try
        {
            _logger.LogDebug("Getting promocodes from database");
            var promocodes = _uow.PromoRepo.Find(p => !p.Deleted).ToList();

            _logger.LogDebug("Found {Count} active promocodes, returning", promocodes.Count);
            return (GetPromocodesResponse)promocodes;
        }
        catch
        {
            _logger.LogError("Failed to get promocodes from database");
            throw;
        }
    }

    public async Task<SetPromocodeMetaResponse> SetPromocodeMeta(SetPromocodeMetaRequest request)
    {
        Promo promo;
        try
        {
            _logger.LogDebug("Finding promo with id {PromoId}", request.PromoId);
            promo = await _uow.PromoRepo.FindOneAsync(p => p.Id == request.PromoId);
        }
        catch
        {
            _logger.LogError("Failed to find promo with id {PromoId}", request.PromoId);
            throw;
        }

        try
        {
            _logger.LogDebug("Updating promo with id {PromoId}", request.PromoId);

            promo.Description = request.Description ?? promo.Description;
            promo.Discount = request.Discount ?? promo.Discount;
            promo.IsActive = request.IsActive ?? promo.IsActive;
            promo.IsStackable = request.IsStackable ?? promo.IsStackable;
            promo.MaxAmountDiscounted = request.MaxAmountDiscounted ?? promo.MaxAmountDiscounted;
            promo.MinAccountAge = request.MinAccountAge ?? promo.MinAccountAge;
            promo.MaxAccountAge = request.MaxAccountAge ?? promo.MaxAccountAge;
            promo.MaxPerUser = request.MaxPerUser ?? promo.MaxPerUser;
            promo.MaxPerEveryone = request.MaxPerEveryone ?? promo.MaxPerEveryone;
            promo.StartDate = request.StartDate ?? promo.StartDate;
            promo.EndDate = request.EndDate ?? promo.EndDate;

            _uow.PromoRepo.Update(promo);
            
            _logger.LogInformation("Updated promo with id {PromoId}", request.PromoId);
            return new SetPromocodeMetaResponse() { IsSuccess = true };
        }
        catch
        {
            _logger.LogError("Failed to update promo with id {PromoId}", request.PromoId);
            throw;
        }
    }
}