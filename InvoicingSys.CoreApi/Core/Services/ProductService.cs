using InvoicingSys.CoreApi.Core.DataContext.DBContexts;
using InvoicingSys.CoreApi.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvoicingSys.CoreApi.Core.Services;

public class ProductService
{
    private readonly ApplicationDbContext _context;
    
    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public Product CreateProduct(string? name, decimal? price, VatTax? vatTax)
    {
        if (name is null)
            throw new ArgumentNullException(nameof(name), "Name cannot be null");
        
        if (price is null)
            throw new ArgumentNullException(nameof(price), "Price cannot be null");
        
        Product product = new Product(name, price, vatTax);
        
        _context.Products.Add(product);
        _context.SaveChanges();
        return product;
    }
    
    public Product ModifyProduct(Product product, string? name, decimal? price, VatTax? vatTax)
    {
        if(name is not null)
            product.Name = name;
        
        if(price is not null)
            product.Price = (decimal) price;
        
        if(vatTax is not null)
            product.VatTax = vatTax;
        
        _context.SaveChanges();
        
        return product;
    }

    public bool DeleteProduct(Product product)
    {
        _context.Products.Remove(product);
        _context.SaveChanges();
        return true;
    }

    public List<Product> GetProducts()
    {
        return _context.Products.Include(p => p.VatTax).ToList();
    }

    public Product? GetProductById(Guid productId)
    {
        return _context.Products.Include(p => p.VatTax).FirstOrDefault(i => i.Id == productId);
    }
}