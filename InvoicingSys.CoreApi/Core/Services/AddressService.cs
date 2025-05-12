using InvoicingSys.CoreApi.Core.DataContext.DBContexts;
using InvoicingSys.CoreApi.Core.Entities;

namespace InvoicingSys.CoreApi.Core.Services;

public class AddressService
{
    private readonly ApplicationDbContext _context;
    
    public AddressService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public Address CreateAddress(string? laneNumber, string? street, string? zipCode, string? city)
    {
        if (laneNumber is null)
            throw new ArgumentNullException(nameof(laneNumber), "Lane number cannot be null");
        
        if (street is null)
            throw new ArgumentNullException(nameof(street), "Street cannot be null");
        
        if (zipCode is null)
            throw new ArgumentNullException(nameof(zipCode), "Zip Code cannot be null");
        
        if (city is null)
            throw new ArgumentNullException(nameof(city), "City cannot be null");
        
        Address address = new Address(laneNumber, street, zipCode, city);
        
        _context.Addresses.Add(address);
        _context.SaveChanges();
        return address;
    }
    
    public Address ModifyAddress(Address address, string? laneNumber, string? street, string? zipCode, string? city)
    {
        if(laneNumber is not null)
            address.LaneNumber = laneNumber;
        
        if(street is not null)
            address.Street = street;
        
        if(zipCode is not null)
            address.ZipCode = zipCode;

        if (city is not null)
            address.City = city;
        
        _context.SaveChanges();
        
        return address;
    }

    public bool DeleteAddress(Address address)
    {
        _context.Addresses.Remove(address);
        _context.SaveChanges();
        return true;
    }

    public List<Address> GetAddresses()
    {
        return _context.Addresses.ToList();
    }

    public Address? GetAddressById(Guid addressId)
    {
        return _context.Addresses.FirstOrDefault(i => i.Id == addressId);
    }
}