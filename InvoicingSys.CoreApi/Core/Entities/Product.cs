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
    public string? _name;

    [NotNull] 
    [Column("price")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    [Required(ErrorMessage = "Price cannot be empty.")]
    public decimal? _price;
    
    [NotNull] 
    [Column("vat_tax")]
    [Required(ErrorMessage = "VAT Tax cannot be empty.")]
    public VatTax? _vatTax ;
    
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
    
    public VatTax? VatTax
    {
        get => _vatTax;
        set => _vatTax = value ?? throw new ArgumentNullException(nameof(VatTax), "VAT Tax cannot be null");
    }
    
    public Product()
    {
        _id = Guid.NewGuid();
    }
    
    public Product(string? name, decimal? price, VatTax? vatTax)
    {
        _id = Guid.NewGuid();
        _name = name;
        _price = price;
        _vatTax = vatTax;
    }
}