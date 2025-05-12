using InvoicingSys.CoreApi.Core.DataContext.DBContexts;
using InvoicingSys.CoreApi.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvoicingSys.CoreApi.Core.Services;

public class CustomerService
{
    private readonly ApplicationDbContext _context;
    
    public CustomerService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public Customer CreateCustomer(string? code, string? firstName, string? lastName,  
        string? email, string? phoneNumber, Address? address)
    {
        if (code is null)
            throw new ArgumentNullException(nameof(code), "Code cannot be null");
        
        if (firstName is null)
            throw new ArgumentNullException(nameof(firstName), "First name cannot be null");
        
        if (lastName is null)
            throw new ArgumentNullException(nameof(lastName), "Last name cannot be null");
        
        if (email is null)
            throw new ArgumentNullException(nameof(email), "Email cannot be null");
        
        if (phoneNumber is null)
            throw new ArgumentNullException(nameof(phoneNumber), "PhoneNumber cannot be null");
        
        if (address is null)
            throw new ArgumentNullException(nameof(address), "Address cannot be null");

        Customer customer = new Customer(code, firstName, lastName, email, phoneNumber, address);
        
        _context.Customers.Add(customer);
        _context.SaveChanges();
        return customer;
    }
    
    public Customer ModifyCustomer(Customer customer, string? code, string? firstName, string? lastName,  
                                    string? email, string? phoneNumber, Address? address)
    {
        if(code is not null)
            customer.Code = code;
            
        if(firstName is not null)
            customer.FirstName = firstName;
        
        if(lastName is not null)
            customer.LastName = lastName;
        
        if(email is not null)
            customer.Email = email;

        if (phoneNumber is not null)
            customer.PhoneNumber = phoneNumber;
        
        if(address is not null)
            customer.Address = address;
        
        _context.SaveChanges();
        
        return customer;
    }

    public bool DeleteCustomer(Customer customer)
    {
        _context.Customers.Remove(customer);
        _context.SaveChanges();
        return true;
    }

    public List<Customer> GetCustomers()
    {
        return _context.Customers.Include(c => c.Address).ToList();
    }

    public Customer? GetCustomerById(Guid customerId)
    {
        return _context.Customers
            .Include(c => c.Address)
            .FirstOrDefault(i => i.Id == customerId);
    }
    
    public Customer? GetCustomerByCode(string code)
    {
        return _context.Customers
            .Include(c => c.Address)
            .FirstOrDefault(i => i.Code == code);
    }
    
    public Customer? GetCustomerByEmail(string email)
    {
        return _context.Customers
            .Include(c => c.Address)
            .FirstOrDefault(i => i.Email == email);
    }
    
    public Customer? GetCustomerByAddressId(Guid addressId)
    {
        return _context.Customers
            .Include(c => c.Address)
            .FirstOrDefault(i => i.Address != null && i.Address.Id == addressId);
    }
}