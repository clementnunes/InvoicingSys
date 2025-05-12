using System.Net;
using InvoicingSys.CoreApi.Core.Blueprints;
using InvoicingSys.CoreApi.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using InvoicingSys.CoreApi.Core.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace InvoicingSys.CoreApi.Core.Controllers;

[Route("/core/invoices")]
[ApiController]
public class InvoiceController : ControllerBase
{
    private readonly InvoiceService _invoiceService;
    private readonly OrderService _orderService;
    private readonly BankDetailService _bankDetailService;

    public InvoiceController(InvoiceService invoiceService, OrderService orderService, BankDetailService bankDetailService)
    {
        _invoiceService = invoiceService;
        _orderService = orderService;
        _bankDetailService = bankDetailService;
    }

    [HttpPost("add")]
    [SwaggerOperation(Summary = "Creates an invoice")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Invoice), 200)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public ActionResult<Invoice> Post([FromBody] InvoiceBlueprint body)
    {
        Invoice? invoice = null;
        Order? order = null;
        BankDetail? bankDetail = null;
        
        if(body.Order is null || body.Order.Id is null)
            return BadRequest("Order provided is invalid");
        
        try
        {
            order = _orderService.GetOrderById((Guid) body.Order.Id);
            
            if(body.BankDetail is not null) 
                bankDetail = _bankDetailService.GetBankDetailById(body.BankDetail.Id);
            
            invoice = _invoiceService.CreateInvoice(order, body.DueDate, bankDetail);
        }
        catch (Exception e)
        {
            if (e is ArgumentNullException)
                return BadRequest(e.Message);

            Console.WriteLine(e);
            throw;
        }

        return invoice;
    }
    

    [HttpGet("")]
    [SwaggerOperation(Summary = "Returns all invoices")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<Invoice>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<Invoice>> GetInvoices()
    {
        var invoices = _invoiceService.GetInvoices();

        return invoices;
    }
    
    [HttpGet("{invoiceId:guid}")]
    [SwaggerOperation(Summary = "Return Invoice from InvoiceId")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Invoice), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<Invoice> GetInvoice([FromQuery] Guid invoiceId)
    {
        var invoice = _invoiceService.GetInvoiceById(invoiceId);

        if (invoice is null)
            return NotFound("Invoice not found");

        return invoice;
    }
}