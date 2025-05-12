using System.Net;
using InvoicingSys.CoreApi.Core.Blueprints;
using InvoicingSys.CoreApi.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using InvoicingSys.CoreApi.Core.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace InvoicingSys.CoreApi.Core.Controllers;

[Route("/core/addresses")]
[ApiController]
public class AddressController : ControllerBase
{
    private readonly AddressService _addressService;

    public AddressController(AddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpPost("add")]
    [SwaggerOperation(Summary = "Creates an address")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Address), 200)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public ActionResult<Address> Post([FromBody] AddressBlueprint body)
    {
        Address? address = null;

        try
        {
            address = _addressService.CreateAddress(
                body.LaneNumber, 
                body.Street, 
                body.ZipCode, 
                body.City);
        }
        catch (Exception e)
        {
            if (e is ArgumentNullException)
                return BadRequest();

            Console.WriteLine(e);
            throw;
        }

        return address;
    }

    [HttpPost("")]
    [SwaggerOperation(Summary = "Creates addresses")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<Address>), 200)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public ActionResult<IEnumerable<Address>> Post([FromBody] IEnumerable<AddressBlueprint> body)
    {
        Address? address = null;
        var addresses = new List<Address>();
        
        foreach (var addressBlueprint in body)
            try
            {
                address = _addressService.CreateAddress(
                    addressBlueprint.LaneNumber, 
                    addressBlueprint.Street, 
                    addressBlueprint.ZipCode, 
                    addressBlueprint.City);

                addresses.Add(address);
            }
            catch (Exception e)
            {
                if (e is ArgumentNullException)
                    return BadRequest();

                Console.WriteLine(e);
                throw;
            }
        
        return addresses;
    }

    [HttpGet("")]
    [SwaggerOperation(Summary = "Returns all addresses")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<Address>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public ActionResult<IEnumerable<Address>> GetAddresses()
    {
        var addresses = _addressService.GetAddresses();

        return addresses;
    }


    [HttpGet("{addressId:guid}")]
    [SwaggerOperation(Summary = "Return Address from AddressId")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Address), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public ActionResult<Address> GetAddress([FromQuery] Guid addressId)
    {
        var address = _addressService.GetAddressById(addressId);

        if (address is null)
            return NotFound();

        return address;
    }
    
    [HttpPatch("{addressId:guid}")]
    [SwaggerOperation(Summary = "Modify an address")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Address), 200)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public ActionResult<Address> Patch([FromBody] AddressBlueprint body, [FromQuery] Guid addressId)
    {
        
        if(addressId == Guid.Empty) return BadRequest("Id is required");
        
        Address? address = _addressService.GetAddressById(addressId);
        
        if(address is null) return NotFound("Address not found");

        try
        {
            address = _addressService.ModifyAddress(
                address,
                body.LaneNumber, 
                body.Street, 
                body.ZipCode, 
                body.City);
        }
        catch (Exception e)
        {
            if (e is ArgumentNullException)
                return BadRequest(e.Message);

            Console.WriteLine(e);
            throw;
        }

        return address;
    }
}