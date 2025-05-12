using InvoicingSys.CoreApi.Core.Blueprints;
using InvoicingSys.CoreApi.Core.DataContext.DBContexts;
using InvoicingSys.CoreApi.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvoicingSys.CoreApi.Core.Services;

public class OrderService
{
    private readonly ApplicationDbContext _context;
    private readonly OrderLineService _orderLineService;
    
    public OrderService(ApplicationDbContext context, OrderLineService orderLineService)
    {
        _context = context;
        _orderLineService = orderLineService;
    }
    
    public Order CreateOrder(List<OrderLine>? orderLines, Customer customer)
    {
        if (orderLines is null)
            throw new ArgumentNullException(nameof(orderLines), "OrderLines cannot be null");
        
        if (orderLines.Count == 0)
            throw new ArgumentException("OrderLines cannot be empty", nameof(orderLines));
        
        
        Order order = new Order(orderLines, DateTime.Now, customer);
        
        _context.Orders.Add(order);
        _context.SaveChanges();
        return order;
    }
    
    public Order CreateOrder(List<OrderLineBlueprint>? orderLineBlueprints, Customer? customer)
    {
        if (orderLineBlueprints is null)
            throw new ArgumentNullException(nameof(orderLineBlueprints), "OrderLines cannot be null");
        
        if (orderLineBlueprints.Count == 0)
            throw new ArgumentException("OrderLines cannot be empty", nameof(orderLineBlueprints));
        
        List<OrderLine> orderLines = _orderLineService.CreateOrderLines(orderLineBlueprints);
        
        Order order = new Order(orderLines, DateTime.Now, customer);
        
        _context.Orders.Add(order);
        _context.SaveChanges();
        return order;
    }
    
    public Order ModifyOrder(Order order, List<OrderLine>? orderLines, DateTime? orderDate)
    {
        if(orderLines is not null)
            order.OrderLines = orderLines;
        
        if (orderLines.Count != 0)
            order.OrderLines = orderLines;
        
        if(orderDate is not null)
            order.OrderDate = orderDate;
        
        _context.SaveChanges();
        
        return order;
    }

    public bool DeleteOrder(Order order)
    {
        _context.Orders.Remove(order);
        _context.SaveChanges();
        return true;
    }

    public List<Order> GetOrders()
    {
        return _context.Orders
            .Include(o => o.Customer)
            .ThenInclude(customer => customer.Address)
            .Include(o => o.OrderLines)
            .ThenInclude(orderLine => orderLine.BoughtProduct)
            .ThenInclude(boughtProduct => boughtProduct.VatTax)
            .ToList();
    }

    public Order? GetOrderById(Guid orderId)
    {
        return _context.Orders
            .Include(o => o.Customer)
            .ThenInclude(customer => customer.Address)
            .Include(o => o.OrderLines)
            .ThenInclude(orderLine => orderLine.BoughtProduct)
            .ThenInclude(boughtProduct => boughtProduct.VatTax)
            .FirstOrDefault(i => i.Id == orderId);
    }
}