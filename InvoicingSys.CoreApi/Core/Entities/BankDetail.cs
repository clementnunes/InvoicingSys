using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace InvoicingSys.CoreApi.Core.Entities;

[Table("bank_detail")]
public class BankDetail
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid _id;
    
    [NotNull]
    [Column("location")]
    [Required(ErrorMessage = "Location cannot be empty.")]
    public string? _location = default!;
    
    [NotNull]
    [Column("owner_name")]
    [Required(ErrorMessage = "OwnerName cannot be empty.")]
    public string? _ownerName = default!;
    
    [NotNull]
    [Column("iban")]
    [Required(ErrorMessage = "IBAN cannot be empty.")]
    public string? _iban = default!;
    
    [NotNull]
    [Column("bic")]
    [Required(ErrorMessage = "BIC/SWIFT cannot be empty.")]
    public string? _bic = default!;
    
    public Guid Id
    {
        get => _id;
        private set => _id = value;
    }
    
    public string? Location
    {
        get => _location;
        set => _location = value ?? throw new ArgumentNullException(nameof(Location), "Location cannot be null");
    }
    
    public string? OwnerName
    {
        get => _ownerName;
        set => _ownerName = value ?? throw new ArgumentNullException(nameof(OwnerName), "OwnerName cannot be null");
    }
    
    public string? Iban
    {
        get => _iban;
        set => _iban = value ?? throw new ArgumentNullException(nameof(Iban), "Iban cannot be null");
    }
    
    public string? Bic
    {
        get => _bic;
        set => _bic = value ?? throw new ArgumentNullException(nameof(Bic), "Bic cannot be null");
    }
    
    public BankDetail()
    {
        _id = Guid.NewGuid();
    }
    
    public BankDetail(string? location, string? ownerName, string? iban, string? bic)
    {
        _id = Guid.NewGuid();
        _location = location;
        _ownerName = ownerName;
        _iban = iban;
        _bic = bic;
    }
}