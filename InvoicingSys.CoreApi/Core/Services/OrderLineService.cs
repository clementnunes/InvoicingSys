using InvoicingSys.Core.Entities;
using InvoicingSys.CoreApi.Core.Blueprints;
using InvoicingSys.CoreApi.Core.DataContext.DBContexts;
using InvoicingSys.CoreApi.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvoicingSys.CoreApi.Core.Services;

public class OrderLineService
{
    private readonly ApplicationDbContext _context;
    private readonly ProductService _productService;
    
    public OrderLineService(ApplicationDbContext context, ProductService productService)
    {
        _context = context;
        _productService = productService;
    }
    
    public OrderLine CreateOrderLine(Product? boughtProduct, int? quantity)
    {
        if (boughtProduct is null)
            throw new ArgumentNullException(nameof(boughtProduct), "BoughtProduct cannot be null");
        
        if (quantity is null)
            throw new ArgumentNullException(nameof(quantity), "Quantity cannot be null");
        
        if (boughtProduct.VatTax is null)
            throw new ArgumentNullException(nameof(boughtProduct.VatTax), "VAT Tax cannot be null");
        
        if (boughtProduct.Price is null)
            throw new ArgumentNullException(nameof(boughtProduct.Price), "Price cannot be null");
        
        OrderLine orderLine = new OrderLine(boughtProduct, quantity, boughtProduct.VatTax.Rate, boughtProduct.Price);
        
        _context.OrderLines.Add(orderLine);
        _context.SaveChanges();
        return orderLine;
    }
    
    public List<OrderLine> CreateOrderLines(List<OrderLineBlueprint> orderLineBlueprints)
    {
        List<Error> errors = CheckOrderLines(orderLineBlueprints);

        if (errors.Count > 0)
            throw new ApplicationException(string.Join(',', errors));

        OrderLine orderLine;
        Product? boughtProduct = null;
        List<OrderLine> orderLines = new List<OrderLine>();
            
        foreach (var orderLineBlueprint in orderLineBlueprints)
        {
            boughtProduct = _productService.GetProductById((Guid) orderLineBlueprint.BoughtProduct.Id);
            orderLine = CreateOrderLine(boughtProduct, orderLineBlueprint.Quantity);
            
            orderLines.Add(orderLine);
        }
        
        return orderLines;
    }
    
    public OrderLine ModifyOrderLine(OrderLine orderLine, Product? boughtProduct, int? quantity)
    {
        if (boughtProduct is not null)
            orderLine.BoughtProduct = boughtProduct;
        
        if (quantity is not null)
            orderLine.Quantity = quantity;
        
        if (boughtProduct.VatTax is not null)
            orderLine.VatTax = boughtProduct.VatTax.Rate;
        
        if (boughtProduct.Price is not null)
            orderLine.UnitPrice = boughtProduct.Price;
        
        orderLine.UpdateAmounts();
        
        _context.SaveChanges();
        
        return orderLine;
    }

    public bool DeleteOrderLine(OrderLine orderLine)
    {
        _context.OrderLines.Remove(orderLine);
        _context.SaveChanges();
        return true;
    }

    public List<OrderLine> GetOrderLines()
    {
        return _context.OrderLines
            .Include(o => o.BoughtProduct)
            .ThenInclude(boughtProduct => boughtProduct.VatTax)
            .ToList();
    }

    public OrderLine? GetOrderLineById(Guid orderLineId)
    {
        return _context.OrderLines
            .Include(o => o.BoughtProduct)
            .ThenInclude(boughtProduct => boughtProduct.VatTax)
            .FirstOrDefault(i => i.Id == orderLineId);
    }
    
    public bool IsValidOrderLine(OrderLineBlueprint orderLineBlueprint)
    {
        if (orderLineBlueprint.BoughtProduct is null || orderLineBlueprint.BoughtProduct.Id is null)
            return false;
        
        if(orderLineBlueprint.Quantity is null)
            return false;
        
        if(orderLineBlueprint.Quantity <= 0)
            return false;

        Product? product = _productService.GetProductById((Guid) orderLineBlueprint.BoughtProduct.Id);
        
        if(product is null)
            return false;

        return true;
    }
    
    public Error? CheckOrderLine(OrderLineBlueprint orderLineBlueprint)
    {
        if (orderLineBlueprint.BoughtProduct is null || orderLineBlueprint.BoughtProduct.Id is null)
            return new Error("BoughtProduct cannot be null", nameof(orderLineBlueprint.BoughtProduct));
        
        if(orderLineBlueprint.Quantity is null)
            return new Error("Quantity cannot be null", nameof(orderLineBlueprint.BoughtProduct));
        
        if(orderLineBlueprint.Quantity <= 0)
            return new Error("Quantity must be greater than 0", nameof(orderLineBlueprint.BoughtProduct));

        Product? product = _productService.GetProductById((Guid) orderLineBlueprint.BoughtProduct.Id);
        
        if(product is null)
            return new Error("Product cannot be null", nameof(product));

        return null;
    }

    public bool ValidateOrderLine(OrderLineBlueprint orderLineBlueprint)
    {
        if (orderLineBlueprint.BoughtProduct is null || orderLineBlueprint.BoughtProduct.Id is null)
            throw new ArgumentNullException(nameof(orderLineBlueprint.BoughtProduct), "BoughtProduct cannot be null");
        
        if(orderLineBlueprint.Quantity is null)
            throw new ArgumentNullException(nameof(orderLineBlueprint.Quantity), "Quantity cannot be null");
        
        if(orderLineBlueprint.Quantity <= 0)
            throw new InvalidOperationException("Quantity must be greater than 0");

        Product? product = _productService.GetProductById((Guid) orderLineBlueprint.BoughtProduct.Id);
        
        if(product is null)
            throw new KeyNotFoundException("Product cannot be found");

        return true;
    }
    
    public bool ValidateOrderLines(IEnumerable<OrderLineBlueprint> orderLineBlueprints)
    {
        foreach (var orderLineBlueprint in orderLineBlueprints)
            ValidateOrderLine(orderLineBlueprint);
        
        return true;
    }
    
    public List<Error> CheckOrderLines(IEnumerable<OrderLineBlueprint> orderLineBlueprints)
    {
        List<Error> errors = new List<Error>();
        Error? error = null;
        
        foreach (var orderLineBlueprint in orderLineBlueprints)
        {
            error = CheckOrderLine(orderLineBlueprint);
            
            if(error is not null)
                errors.Add(error);
        }
        return errors;
    }
}