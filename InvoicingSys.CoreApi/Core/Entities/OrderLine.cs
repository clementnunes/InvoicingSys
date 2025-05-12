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
    public Product? _boughtProduct;
    
    [NotNull] 
    [Column("quantity")]
    [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    [Required(ErrorMessage = "Quantity cannot be empty.")]
    public int? _quantity;
    
    [NotNull] 
    [Column("vat_tax")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    [Required(ErrorMessage = "VAT Tax cannot be empty.")]
    public decimal? _vatTax;
    
    [NotNull] 
    [Column("unit_price")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    [Required(ErrorMessage = "UnitPrice cannot be empty.")]
    public decimal? _unitPrice;
    
    [Column("VAT_unit_amount")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    public decimal? _VATunitAmount;
    
    [Column("IAT_unit_amount")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    public decimal? _IATunitAmount;
    
    [Column("VAT_total_amount")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    public decimal? _VATtotalAmount;
    
    [Column("IAT_total_amount")]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    public decimal? _IATtotalAmount;
    
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
    
    public decimal? VATunitAmount
    {
        get => _VATunitAmount;
        private set => _VATunitAmount = value ?? throw new ArgumentNullException(nameof(_VATunitAmount), "VAT Unit Amount cannot be null");
    }
    
    public decimal? IATunitAmount
    {
        get => _IATunitAmount;
        private set => _IATunitAmount = value ?? throw new ArgumentNullException(nameof(_IATunitAmount), "IAT Unit Amount cannot be null");
    }
    
    public decimal? VATtotalAmount
    {
        get => _VATtotalAmount;
        private set => _VATtotalAmount = value ?? throw new ArgumentNullException(nameof(_VATtotalAmount), "VAT Total Amount cannot be null");
    }
    
    public decimal? IATtotalAmount
    {
        get => _IATtotalAmount;
        private set => _IATtotalAmount = value ?? throw new ArgumentNullException(nameof(_IATtotalAmount), "IAT Total Amount cannot be null");
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
        UpdateAmounts();
    }

    public void UpdateAmounts()
    {
        _VATunitAmount = _unitPrice;
        _IATunitAmount = _unitPrice * (1 + _vatTax);
        _VATtotalAmount = _quantity * _VATunitAmount;
        _IATtotalAmount = _quantity * _IATunitAmount;
    }
}