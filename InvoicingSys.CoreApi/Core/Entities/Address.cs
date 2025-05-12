using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace InvoicingSys.CoreApi.Core.Entities;

[Table("address")]
public class Address
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid _id;
    
    [NotNull]
    [Column("lane_number")]
    [Required(ErrorMessage = "LaneNumber cannot be empty.")]
    public string? _laneNumber = default!;
    
    [NotNull]
    [Column("street")]
    [Required(ErrorMessage = "Street cannot be empty.")]
    public string? _street = default!;
    
    [NotNull]
    [Column("zip_code")]
    [Required(ErrorMessage = "ZipCode cannot be empty.")]
    public string? _zipCode = default!;
    
    [NotNull]
    [Column("city")]
    [Required(ErrorMessage = "City cannot be empty.")]
    public string? _city = default!;
    
    public Guid Id
    {
        get => _id;
        private set => _id = value;
    }
    
    public string? LaneNumber
    {
        get => _laneNumber;
        set => _laneNumber = value ?? throw new ArgumentNullException(nameof(LaneNumber), "LaneNumber cannot be null");
    }
    
    public string? Street
    {
        get => _street;
        set => _street = value ?? throw new ArgumentNullException(nameof(Street), "Street cannot be null");
    }
    
    public string? ZipCode
    {
        get => _zipCode;
        set => _zipCode = value ?? throw new ArgumentNullException(nameof(ZipCode), "ZipCode cannot be null");
    }
    
    public string? City
    {
        get => _city;
        set => _city = value ?? throw new ArgumentNullException(nameof(City), "City cannot be null");
    }
    
    public Address()
    {
        _id = Guid.NewGuid();
    }
    
    public Address(string? laneNumber, string? street, string? zipCode, string? city)
    {
        _id = Guid.NewGuid();
        _laneNumber = laneNumber;
        _street = street;
        _zipCode = zipCode;
        _city = city;
    }
}