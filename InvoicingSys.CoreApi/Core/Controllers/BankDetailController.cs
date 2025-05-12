using System.Net;
using InvoicingSys.CoreApi.Core.Blueprints;
using InvoicingSys.CoreApi.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using InvoicingSys.CoreApi.Core.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace InvoicingSys.CoreApi.Core.Controllers;

[Route("/core/bankDetails")]
[ApiController]
public class BankDetailController : ControllerBase
{
    private readonly BankDetailService _bankDetailService;

    public BankDetailController(BankDetailService bankDetailService)
    {
        _bankDetailService = bankDetailService;
    }

    [HttpPost("add")]
    [SwaggerOperation(Summary = "Creates an bankDetail")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(BankDetail), 200)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public ActionResult<BankDetail> Post([FromBody] BankDetailBlueprint body)
    {
        BankDetail? bankDetail = null;

        try
        {
            bankDetail = _bankDetailService.CreateBankDetail(
                body.Location, 
                body.OwnerName, 
                body.Iban, 
                body.Bic);
        }
        catch (Exception e)
        {
            if (e is ArgumentNullException)
                return BadRequest();

            Console.WriteLine(e);
            throw;
        }

        return bankDetail;
    }

    [HttpPost("")]
    [SwaggerOperation(Summary = "Creates bankDetails")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<BankDetail>), 200)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public ActionResult<IEnumerable<BankDetail>> Post([FromBody] IEnumerable<BankDetailBlueprint> body)
    {
        BankDetail? bankDetail = null;
        var bankDetails = new List<BankDetail>();
        
        foreach (var bankDetailBlueprint in body)
            try
            {
                bankDetail = _bankDetailService.CreateBankDetail(
                    bankDetailBlueprint.Location, 
                    bankDetailBlueprint.OwnerName, 
                    bankDetailBlueprint.Iban, 
                    bankDetailBlueprint.Bic);

                bankDetails.Add(bankDetail);
            }
            catch (Exception e)
            {
                if (e is ArgumentNullException)
                    return BadRequest();

                Console.WriteLine(e);
                throw;
            }
        
        return bankDetails;
    }

    [HttpGet("")]
    [SwaggerOperation(Summary = "Returns all bankDetails")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<BankDetail>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<BankDetail>> GetBankDetails()
    {
        var bankDetails = _bankDetailService.GetBankDetails();

        return bankDetails;
    }


    [HttpGet("{bankDetailId:guid}")]
    [SwaggerOperation(Summary = "Return BankDetail from BankDetailId")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(BankDetail), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<BankDetail> GetBankDetail([FromQuery] Guid bankDetailId)
    {
        var bankDetail = _bankDetailService.GetBankDetailById(bankDetailId);

        if (bankDetail is null)
            return NotFound();

        return bankDetail;
    }
    
}