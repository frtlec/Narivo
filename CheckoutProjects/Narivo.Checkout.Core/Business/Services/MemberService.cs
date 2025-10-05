using Narivo.Checkout.Core.Clients.Dtos.NarivoCatalogApiDtos;
using Narivo.Checkout.Core.Clients.RefitClients;
using Narivo.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narivo.Checkout.Core.Business.Services;

public interface IMemberService
{
    Task<List<MemberDto>> GetAllMembers();
}   
public class MemberService: IMemberService
{
    private readonly IMembershipApiClient _membershipApiClient;


    public MemberService(IMembershipApiClient membershipApiClient)
    {
        _membershipApiClient = membershipApiClient;
    }

    public async Task<List<MemberDto>> GetAllMembers()
    {
        return await RefitWrapper.ExecuteAsync<List<MemberDto>>(() => _membershipApiClient.GetAll());
    }
}
