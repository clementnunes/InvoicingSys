using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace InvoicingSys.CoreApi.Core.Entities;

[Table("customer")]
public class Customer
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid _id;
    
    [NotNull]
    [Column("code")] 
    [Required(ErrorMessage = "Code cannot be empty.")]
    public string? _code;

    [NotNull]
    [Column("first_name")] 
    [Required(ErrorMessage = "First Name cannot be empty.")]
    public string? _firstName;

    [NotNull] 
    [Column("last_name")] 
    [Required(ErrorMessage = "Last Name cannot be empty.")]
    public string? _lastName;

    [NotNull] 
    [Column("email")] 
    [Required(ErrorMessage = "Email cannot be empty.")]
    public string? _email;

    [NotNull] 
    [Column("phone_number")] 
    [Required(ErrorMessage = "Phone Number cannot be empty.")]
    public string? _phoneNumber;

    [NotNull] 
    [Column("address")] 
    [Required(ErrorMessage = "Address cannot be empty.")]
    public Address? _address;
    
    public Guid Id
    {
        get => _id;
        private set => _id = value;
    }
    
    public string? Code
    {
        get => _code;
        set => _code = value ?? throw new ArgumentNullException(nameof(Code), "Code cannot be null");
    }
    
    public string? FirstName
    {
        get => _firstName;
        set => _firstName = value ?? throw new ArgumentNullException(nameof(FirstName), "FirstName cannot be null");
    }
    
    public string? LastName
    {
        get => _lastName;
        set => _lastName = value ?? throw new ArgumentNullException(nameof(LastName), "LastName cannot be null");
    }
    
    public string? Email
    {
        get => _email;
        set => _email = value ?? throw new ArgumentNullException(nameof(Email), "Email cannot be null");
    }
    
    public string? PhoneNumber
    {
        get => _phoneNumber;
        set => _phoneNumber = value ?? throw new ArgumentNullException(nameof(PhoneNumber), "PhoneNumber cannot be null");
    }
    
    public Address? Address
    {
        get => _address;
        set => _address = value ?? throw new ArgumentNullException(nameof(Address), "Address cannot be null");
    }

    public Customer()
    {
        _id = Guid.NewGuid();
    }
    
    public Customer(string? code, string? firstName, string? lastName, string? email, string? phoneNumber, Address? address)
    {
        _id = Guid.NewGuid();
        _code = code;
        _firstName = firstName;
        _lastName = lastName;
        _email = email;
        _phoneNumber = phoneNumber;
        _address = address;
    }
}