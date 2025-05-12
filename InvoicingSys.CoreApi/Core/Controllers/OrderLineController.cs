using System.Net;
using InvoicingSys.Core.Entities;
using InvoicingSys.CoreApi.Core.Blueprints;
using InvoicingSys.CoreApi.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using InvoicingSys.CoreApi.Core.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace InvoicingSys.CoreApi.Core.Controllers;

[Route("/core/orderLines")]
[ApiController]
public class OrderLineController : ControllerBase
{
    private readonly OrderLineService _orderLineService;
    private readonly ProductService _productService;

    public OrderLineController(OrderLineService orderLineService, ProductService productService)
    {
        _orderLineService = orderLineService;
        _productService = productService;
    }

    [HttpPost("add")]
    [SwaggerOperation(Summary = "Creates an orderLine")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(OrderLine), 200)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public ActionResult<OrderLine> Post([FromBody] OrderLineBlueprint body)
    {
        OrderLine? orderLine;
        Product? product;
        
        if (body.BoughtProduct is null || body.BoughtProduct.Id is null)
            throw new BadHttpRequestException("Invalid bought product");

        _orderLineService.ValidateOrderLine(body);

        try
        {
            product = _productService.GetProductById((Guid) body.BoughtProduct.Id);

            if (product is null)
                return NotFound("Product not found");
            
            orderLine = _orderLineService.CreateOrderLine(product, body.Quantity);
        }
        catch (Exception e)
        {
            if (e is ArgumentNullException)
                return BadRequest();

            Console.WriteLine(e);
            throw;
        }

        return orderLine;
    }

    [HttpPost("")]
    [SwaggerOperation(Summary = "Creates orderLines")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<OrderLine>), 200)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public ActionResult<IEnumerable<OrderLine>> Post([FromBody] IEnumerable<OrderLineBlueprint> body)
    {
        OrderLine? orderLine;
        Product? product;
        var orderLines = new List<OrderLine>();

        var orderLineBlueprints = body as OrderLineBlueprint[] ?? body.ToArray();
        
        List<Error> errors = _orderLineService.CheckOrderLines(orderLineBlueprints);

        if (errors.Count > 0)
            return BadRequest(errors);
        
        _orderLineService.ValidateOrderLines(orderLineBlueprints);
        
        foreach (var orderLineBlueprint in orderLineBlueprints)
        {
            if (orderLineBlueprint.BoughtProduct is null || orderLineBlueprint.BoughtProduct.Id is null)
                throw new BadHttpRequestException("Invalid bought product");
            
            try
            {
                product = _productService.GetProductById((Guid) orderLineBlueprint.BoughtProduct.Id);

                if (product is null)
                    return NotFound("Product not found");
            
                orderLine = _orderLineService.CreateOrderLine(product, orderLineBlueprint.Quantity);
                orderLines.Add(orderLine);
            }
            catch (Exception e)
            {
                if (e is ArgumentNullException)
                    return BadRequest();

                Console.WriteLine(e);
                throw;
            }
        }
            
        
        return orderLines;
    }

    [HttpGet("")]
    [SwaggerOperation(Summary = "Returns all orderLines")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<OrderLine>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<OrderLine>> GetOrderLines()
    {
        var orderLines = _orderLineService.GetOrderLines();

        return orderLines;
    }


    [HttpGet("{orderLineId:guid}")]
    [SwaggerOperation(Summary = "Return OrderLine from OrderLineId")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(OrderLine), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<OrderLine> GetOrderLine([FromQuery] Guid orderLineId)
    {
        var orderLine = _orderLineService.GetOrderLineById(orderLineId);

        if (orderLine is null)
            return NotFound();

        return orderLine;
    }
    
    [HttpPatch("{orderLineId:guid}")]
    [SwaggerOperation(Summary = "Modify an OrderLine")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(OrderLine), 200)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public ActionResult<OrderLine> Patch([FromBody] OrderLineBlueprint body, [FromQuery] Guid orderLineId)
    {
        if(orderLineId == Guid.Empty) return BadRequest("OrderLine Id cannot be empty");
        OrderLine? orderLine = _orderLineService.GetOrderLineById(orderLineId);
        
        if(orderLine is null)
            return NotFound("OrderLine not found");
        
        if(orderLine.BoughtProduct is null)
            return NotFound("Product not found");
        
        Product? product = _productService.GetProductById(orderLine.BoughtProduct.Id);
        
        try
        {
            orderLine = _orderLineService.ModifyOrderLine(
                orderLine,
                product,
                body.Quantity);
        }
        catch (Exception e)
        {
            if (e is ArgumentNullException)
                return BadRequest();

            Console.WriteLine(e);
            throw;
        }

        return orderLine;
    }
}