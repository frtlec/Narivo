using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Narivo.Membership.API.Infastructure.Persistence;

namespace Narivo.Membership.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class CardsController : ControllerBase
{

    private readonly MemberDbContext _dbContext;

    public CardsController(MemberDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int memberId, [FromQuery] int cardId)
    {
        var memberCart = await _dbContext.Cards.FirstOrDefaultAsync(f => f.MemberId == memberId && f.Id == cardId);

        return memberCart == null ? NotFound() : Ok(memberCart);
    }
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int memberId)
    {
        var memberCarts = await _dbContext.Cards.Where(f => f.MemberId == memberId).ToListAsync();
        return Ok(memberCarts);
    }   
}
