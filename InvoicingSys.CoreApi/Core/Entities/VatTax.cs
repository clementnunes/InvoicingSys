using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace InvoicingSys.CoreApi.Core.Entities;

[Table("vat_tax")]
public class VatTax
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid _id;

    [NotNull] 
    [Column("label")] 
    [Required(ErrorMessage = "Label cannot be empty.")]
    public string? _label;

    [NotNull] 
    [Column("rate")]
    [Range(0.01,1, ErrorMessage = "Please enter a value bigger than {1}")]
    [Required(ErrorMessage = "VAT Rate cannot be empty.")]
    public decimal? _rate;
    
    public Guid Id
    {
        get => _id;
        private set => _id = value;
    }
    
    public string? Label
    {
        get => _label;
        set => _label = value ?? throw new ArgumentNullException(nameof(Label), "Label cannot be null");
    }
    
    public decimal? Rate
    {
        get => _rate;
        set => _rate = value ?? throw new ArgumentNullException(nameof(Rate), "VAT Rate cannot be null");
    }
    
    public VatTax()
    {
        _id = Guid.NewGuid();
    }
    
    public VatTax(string? label, decimal? rate)
    {
        _id = Guid.NewGuid();
        _label = label;
        _rate = rate;
    }
}