using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Narivo.Membership.API.Dtos;
using Narivo.Membership.API.Infastructure.Persistence;

namespace Narivo.Membership.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class MembersController : ControllerBase
{
    private readonly MemberDbContext _memberDbContext;

    public MembersController(MemberDbContext memberDbContext)
    {
        _memberDbContext = memberDbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var members = _memberDbContext.Members.ToList();
        return Ok(members);
    }

    [HttpGet]
    public async Task<IActionResult> GetById([FromQuery] int id)
    {
        var member = await _memberDbContext.Members.Include(f => f.Carts).Include(f => f.Addresses).FirstOrDefaultAsync(f => f.Id == id);
        if (member == null)
            return NotFound();
        return Ok(new MemberDto(member));
    }

    [HttpGet]
    public async Task<IActionResult> GetByIdAndAddressId([FromQuery] int id, [FromQuery] int addressId)
    {
        var member = await _memberDbContext.Members.Include(f => f.Addresses.Where(f => f.Id == addressId)).FirstOrDefaultAsync(f => f.Id == id);
        if (member == null || member.Addresses.Any() == false)
            return NotFound("Üye bulunamadı");
        return Ok(new GetByIdAndAddressIdDto(member, member.Addresses.First()));
    }

}
