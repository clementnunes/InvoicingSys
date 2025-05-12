using System.Net;
using InvoicingSys.CoreApi.Core.Blueprints;
using InvoicingSys.CoreApi.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using InvoicingSys.CoreApi.Core.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace InvoicingSys.CoreApi.Core.Controllers;

[Route("/core/vat-taxes")]
[ApiController]
public class VatTaxController : ControllerBase
{
    private readonly VatTaxService _vatTaxService;

    public VatTaxController(VatTaxService vatTaxService)
    {
        _vatTaxService = vatTaxService;
    }

    [HttpPost("add")]
    [SwaggerOperation(Summary = "Creates a vatTax")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(VatTax), 200)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public ActionResult<VatTax> Post([FromBody] VatTaxBlueprint body)
    {
        VatTax? vatTax = null;

        try
        {
            vatTax = _vatTaxService.CreateVatTax(body.Label, body.Rate);
        }
        catch (Exception e)
        {
            if (e is ArgumentNullException)
                return BadRequest(e.Message);

            Console.WriteLine(e);
            throw;
        }

        return vatTax;
    }

    [HttpPost("")]
    [SwaggerOperation(Summary = "Creates vatTaxes")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<VatTax>), 200)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public ActionResult<IEnumerable<VatTax>> Post([FromBody] IEnumerable<VatTaxBlueprint> body)
    {
        VatTax? vatTax = null;
        var vatTaxes = new List<VatTax>();
        
        foreach (var vatTaxBlueprint in body)
            try
            {
                vatTax = _vatTaxService.CreateVatTax(vatTaxBlueprint.Label, vatTaxBlueprint.Rate);

                vatTaxes.Add(vatTax);
            }
            catch (Exception e)
            {
                if (e is ArgumentNullException)
                    return BadRequest(e.Message);

                Console.WriteLine(e);
                throw;
            }
        
        return vatTaxes;
    }

    [HttpGet("")]
    [SwaggerOperation(Summary = "Returns all vatTaxes")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<VatTax>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<VatTax>> GetVatTaxes()
    {
        var vatTaxes = _vatTaxService.GetVatTaxes();

        return vatTaxes;
    }


    [HttpGet("{vatTaxId:guid}")]
    [SwaggerOperation(Summary = "Return VatTax from VatTaxId")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(VatTax), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<VatTax> GetVatTax([FromQuery] Guid vatTaxId)
    {
        var vatTax = _vatTaxService.GetVatTaxById(vatTaxId);

        if (vatTax is null)
            return NotFound();

        return vatTax;
    }
    
    [HttpPatch("{vatTaxId:guid}")]
    [SwaggerOperation(Summary = "Modify a vatTax")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(VatTax), 200)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public ActionResult<VatTax> Patch([FromBody] VatTaxBlueprint body, [FromQuery] Guid vatTaxId)
    {
        if(vatTaxId == Guid.Empty) return BadRequest("VatTax Id cannot be empty");
        VatTax? vatTax = _vatTaxService.GetVatTaxById(vatTaxId);
        
        if(vatTax is null)
            return NotFound("VatTax not found");
        try
        {
            vatTax = _vatTaxService.ModifyVatTax(
                vatTax,
                body.Label,
                body.Rate);
        }
        catch (Exception e)
        {
            if (e is ArgumentNullException)
                return BadRequest();

            Console.WriteLine(e);
            throw;
        }

        return vatTax;
    }
}