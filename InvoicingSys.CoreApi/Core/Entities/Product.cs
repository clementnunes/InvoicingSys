using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace InvoicingSys.CoreApi.Core.Entities;

[Table("product")]
public class Product
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid _id;

    [NotNull] 
    [Column("name")] 
    [Required(ErrorMessage = "Name cannot be empty.")]
    public string? _name = default!;

    [NotNull] 
    [Column("price")] 
    [Required(ErrorMessage = "Price cannot be empty.")]
    public decimal? _price = default!;
    
    [NotNull] 
    [Column("vat_tax")] 
    [Required(ErrorMessage = "VAT Tax cannot be empty.")]
    public decimal? _vatTax = default!;
    
    public Guid Id
    {
        get => _id;
        private set => _id = value;
    }
    
    public string? Name
    {
        get => _name;
        set => _name = value ?? throw new ArgumentNullException(nameof(Name), "Name cannot be null");
    }
    
    public decimal? Price
    {
        get => _price;
        set => _price = value ?? throw new ArgumentNullException(nameof(Price), "Price cannot be null");
    }
    
    public decimal? VatTax
    {
        get => _vatTax;
        set => _vatTax = value ?? throw new ArgumentNullException(nameof(VatTax), "VAT Tax cannot be null");
    }
    
    public Product()
    {
        _id = Guid.NewGuid();
    }
    
    public Product(string? name, decimal? price, decimal? vatTax)
    {
        _id = Guid.NewGuid();
        _name = name;
        _price = price;
        _vatTax = vatTax;
    }
}