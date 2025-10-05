using Narivo.Checkout.Core.Business.Dtos.RequestDtos;
using Narivo.Checkout.Core.Business.Dtos.ResponseDtos;
using Narivo.Checkout.Core.Clients.Dtos.NarivoCatalogApiDtos;
using Narivo.Checkout.Core.Clients.RefitClients;
using Narivo.Shared.Helpers;
using Refit;

namespace Narivo.WebUI.Services;

public class MemberUIService
{
    private readonly IMembershipApiClient _membershipApiClient;

    public MemberUIService(IMembershipApiClient membershipApiClient)
    {
        _membershipApiClient = membershipApiClient;
    }

    public async Task<MemberDto> Get(int memberId)
    {
        var t = await _membershipApiClient.Get(memberId);
        return await RefitWrapper.ExecuteAsync<MemberDto>(() => _membershipApiClient.Get(memberId));
    }
}
