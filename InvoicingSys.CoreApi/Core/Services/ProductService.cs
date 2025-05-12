using InvoicingSys.CoreApi.Core.DataContext.DBContexts;
using InvoicingSys.CoreApi.Core.Entities;

namespace InvoicingSys.CoreApi.Core.Services;

public class ProductService
{
    private readonly ApplicationDbContext _context;
    
    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public Product CreateProduct(string? name, decimal? price, decimal? vatTax)
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
    
    public Product ModifyProduct(Product product, string? name, decimal? price, decimal? vatTax)
    {
        if(name is not null)
            product.Name = name;
        
        if(price is not null)
            product.Price = (decimal) price;
        
        if(vatTax is not null)
            product.VatTax = (decimal) vatTax;
        
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
        return _context.Products.ToList();
    }

    public Product? GetProductById(Guid productId)
    {
        return _context.Products.FirstOrDefault(i => i.Id == productId);
    }
}