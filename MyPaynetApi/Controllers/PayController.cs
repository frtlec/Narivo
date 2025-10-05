using Microsoft.AspNetCore.Mvc;
using MyPayNetApi.Models;
using MyPayNetApi.Models.Dtos;

namespace MyPaynetApi.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class PayController : ControllerBase
{
    private readonly MyPayNetApiDbContext _dbContext;

    public PayController(MyPayNetApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PayRequestDto pay)
    {
        if (_dbContext.Pays.Any(f => f.RequestUniqueId == pay.UniqueId))
            return BadRequest("Ödeme işlemi daha önce yapılmış");

        if (FailCards.Contains(pay.CardInfo.CardNumber))
            return BadRequest("Ödeme işlemi yapılmadı, kart limiti yetersiz");


        Guid transactionId = Guid.NewGuid();
        await _dbContext.Pays.AddAsync(new Pay
        {
            CardHolderName = pay.CardInfo.CardHolderName,
            CardNumber = pay.CardInfo.CardNumber,
            CVV = pay.CardInfo.CVV,
            Month = pay.CardInfo.Month,
            Year = pay.CardInfo.Year,
            PaymentStatus = PaymentStatus.Completed,
            RequestUniqueId = pay.UniqueId,
            Bank = pay.CardInfo.Bank,
            CancelTransactionDate = null,
            Total = pay.Total,
            TransactionId = transactionId,
            TransactionDate = DateTime.UtcNow
        });
        await _dbContext.SaveChangesAsync();
        return Ok(new PayDto { TransactionId = transactionId, PayStatus = PaymentStatus.Completed });
    }
    [HttpPost]
    public async Task<IActionResult> Cancel([FromBody] CancelPaymentRequestDto request)
    {
        var payment = await _dbContext.Pays.FindAsync(request.TransactionId);
        if (payment == null)
            return NotFound("Ödeme işlemi bulunamadı");
        if (payment.PaymentStatus != PaymentStatus.Completed)
            return BadRequest("Sadece tamamlanmış ödemeler iptal edilebilir");
        payment.PaymentStatus = PaymentStatus.Cancelled;
        payment.CancelTransactionDate = DateTime.UtcNow;
        _dbContext.Pays.Update(payment);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }


    private List<string> FailCards = new List<string>
        {
            "5890040000000016",
            "5555666677778888",
            "9999000011112222"
        };
}
