using PromoService.Models.PromoApplication.Requests;
using PromoService.Models.PromoApplication.Responses;

namespace PromoService.Services.PromoApplication;

public class PromoApplicationService : IPromoApplicationService
{
    public Task<GetMyPromoApplicationsResponse> GetMyPromoApplications(GetMyPromoApplicationsRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<RegisterPromoUseResponse> RegisterPromoUse(RegisterPromoUseRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ValidatePromocodeApplicationResponse> ValidatePromocodeApplication(ValidatePromocodeApplicationRequest request)
    {
        throw new NotImplementedException();
    }
}
