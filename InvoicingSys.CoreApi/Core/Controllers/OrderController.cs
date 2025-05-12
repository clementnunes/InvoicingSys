using System.Net;
using InvoicingSys.CoreApi.Core.Blueprints;
using InvoicingSys.CoreApi.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using InvoicingSys.CoreApi.Core.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace InvoicingSys.CoreApi.Core.Controllers;

[Route("/core/orders")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;
    private readonly OrderLineService _orderLineService;
    private readonly CustomerService _customerService;
    public OrderController(OrderService orderService, OrderLineService orderLineService, CustomerService customerService)
    {
        _orderService = orderService;
        _orderLineService = orderLineService;
        _customerService = customerService;
    }

    [HttpPost("add")]
    [SwaggerOperation(Summary = "Creates an order")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Order), 200)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public ActionResult<Order> Post([FromBody] OrderBlueprint body)
    {
        Order? order;

        if (body.OrderLines is null || body.OrderLines.Count <= 0) 
            return BadRequest("OrderLines is null, please add at least one line");
        if (body.Customer is null || body.Customer.Id is null) return BadRequest("Customer is null or CustomerId is null");
        
        _orderLineService.ValidateOrderLines(body.OrderLines);

        try
        {
            Customer? customer = _customerService.GetCustomerById((Guid) body.Customer.Id);
            order = _orderService.CreateOrder(body.OrderLines, customer);
        }
        catch (Exception e)
        {
            if (e is ArgumentNullException)
                return BadRequest(e.Message);

            Console.WriteLine(e);
            throw;
        }

        return order;
    }

    [HttpGet("")]
    [SwaggerOperation(Summary = "Returns all orders")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<Order>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<Order>> GetOrders()
    {
        var orders = _orderService.GetOrders();

        return orders;
    }

    [HttpGet("{orderId:guid}")]
    [SwaggerOperation(Summary = "Return Order from OrderId")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<Order> GetOrder([FromQuery] Guid orderId)
    {
        var order = _orderService.GetOrderById(orderId);

        if (order is null)
            return NotFound();

        return order;
    }
    
    [HttpPatch("{orderId:guid}")]
    [SwaggerOperation(Summary = "Modify an order")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Order), 200)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public ActionResult<Order> Patch([FromBody] OrderBlueprint body, [FromQuery] Guid orderId)
    {
        if(orderId == Guid.Empty) return BadRequest("OrderLine Id cannot be empty");
        Order? order = _orderService.GetOrderById(orderId);
        
        if(order is null)
            return NotFound("Order not found");
        
        try
        {
            order = _orderService.ModifyOrder(
                order,
                order.OrderLines,
                body.OrderDate);
        }
        catch (Exception e)
        {
            if (e is ArgumentNullException)
                return BadRequest();

            Console.WriteLine(e);
            throw;
        }

        return order;
    }
}