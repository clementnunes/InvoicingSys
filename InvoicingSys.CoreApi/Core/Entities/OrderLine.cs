using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace InvoicingSys.CoreApi.Core.Entities;

[Table("order_line")]
public class OrderLine
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid _id;
    
    [NotNull] 
    [Column("bought_product")] 
    [Required(ErrorMessage = "BoughtProduct cannot be empty.")]
    public Product? _boughtProduct = default!;
    
    [NotNull] 
    [Column("quantity")] 
    [Required(ErrorMessage = "Quantity cannot be empty.")]
    public int? _quantity = default!;
    
    [NotNull] 
    [Column("vat_tax")] 
    [Required(ErrorMessage = "VAT Tax cannot be empty.")]
    public decimal? _vatTax = default!;
    
    [NotNull] 
    [Column("unit_price")] 
    [Required(ErrorMessage = "UnitPrice cannot be empty.")]
    public decimal? _unitPrice = default!;
    
    public Guid Id
    {
        get => _id;
        private set => _id = value;
    }
    
    public Product? BoughtProduct
    {
        get => _boughtProduct;
        set => _boughtProduct = value ?? throw new ArgumentNullException(nameof(BoughtProduct), "BoughtProduct cannot be null");
    }
    
    public int? Quantity
    {
        get => _quantity;
        set => _quantity = value ?? throw new ArgumentNullException(nameof(Quantity), "Quantity cannot be null");
    }
    
    public decimal? VatTax
    {
        get => _vatTax;
        set => _vatTax = value ?? throw new ArgumentNullException(nameof(VatTax), "VAT Tax cannot be null");
    }
    
    public decimal? UnitPrice
    {
        get => _unitPrice;
        set => _unitPrice = value ?? throw new ArgumentNullException(nameof(UnitPrice), "UnitPrice cannot be null");
    }
    
    public OrderLine()
    {
        _id = Guid.NewGuid();
    }
    
    public OrderLine(Product? boughtProduct, int? quantity, decimal? vatTax, decimal? unitPrice)
    {
        _id = Guid.NewGuid();
        _boughtProduct = boughtProduct;
        _quantity = quantity;
        _vatTax = vatTax;
        _unitPrice = unitPrice;
    }
}