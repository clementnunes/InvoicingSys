﻿using System.Net;
using InvoicingSys.CoreApi.Core.Blueprints;
using InvoicingSys.CoreApi.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using InvoicingSys.CoreApi.Core.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace InvoicingSys.CoreApi.Core.Controllers;

[Route("/core/customers")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _customerService;
    private readonly AddressService _addressService;

    public CustomerController(CustomerService customerService, AddressService addressService)
    {
        _customerService = customerService;
        _addressService = addressService;
    }

    [HttpPost("add")]
    [SwaggerOperation(Summary = "Creates an customer")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Customer), 200)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public ActionResult<Customer> Post([FromBody] CustomerBlueprint body)
    {
        Customer? customer = null;
        Address? address = null;
        
        try
        {
            if (body.Address is not null)
            {
                if (body.Address.Id is null)
                {
                    address = _addressService.CreateAddress(
                        body.Address.LaneNumber, 
                        body.Address.Street, 
                        body.Address.ZipCode, 
                        body.Address.City);
                }
                else
                {
                    address = _addressService.GetAddressById((Guid) body.Address.Id);
                }
            }
            
            customer = _customerService.CreateCustomer(
                body.Code,
                body.FirstName, 
                body.LastName, 
                body.Email, 
                body.PhoneNumber, 
                address);
        }
        catch (Exception e)
        {
            if (e is ArgumentNullException)
                return BadRequest(e.Message);

            Console.WriteLine(e);
            throw;
        }

        return customer;
    }

    [HttpPost("")]
    [SwaggerOperation(Summary = "Creates customers")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<Customer>), 200)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public ActionResult<IEnumerable<Customer>> Post([FromBody] IEnumerable<CustomerBlueprint> body)
    {
        Customer? customer = null;
        Address? address = null;
        var customers = new List<Customer>();
        
        foreach (var customerBlueprint in body)
            try
            {
                if (customerBlueprint.Address is not null)
                {
                    if (customerBlueprint.Address.Id is null)
                    {
                        address = _addressService.CreateAddress(
                            customerBlueprint.Address.LaneNumber, 
                            customerBlueprint.Address.Street, 
                            customerBlueprint.Address.ZipCode, 
                            customerBlueprint.Address.City);
                    }
                    else
                    {
                        address = _addressService.GetAddressById((Guid) customerBlueprint.Address.Id);
                    }
                }
            
                customer = _customerService.CreateCustomer(
                    customerBlueprint.Code,
                    customerBlueprint.FirstName, 
                    customerBlueprint.LastName, 
                    customerBlueprint.Email, 
                    customerBlueprint.PhoneNumber, 
                    address);
                
                customers.Add(customer);
            }
            catch (Exception e)
            {
                if (e is ArgumentNullException)
                    return BadRequest(e.Message);

                Console.WriteLine(e);
                throw;
            }
        
        return customers;
    }

    [HttpGet("")]
    [SwaggerOperation(Summary = "Returns all customers")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<Customer>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<Customer>> GetCustomers()
    {
        var customers = _customerService.GetCustomers();

        return customers;
    }


    [HttpGet("{customerId:guid}")]
    [SwaggerOperation(Summary = "Return Customer from CustomerId")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<Customer> GetCustomer([FromQuery] Guid customerId)
    {
        var customer = _customerService.GetCustomerById(customerId);

        if (customer is null)
            return NotFound("Customer not found");

        return customer;
    }
    
    [HttpGet("/codes/{code}")]
    [SwaggerOperation(Summary = "Return Customer from CustomerCode")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<Customer> GetCustomerByCode([FromQuery] string code)
    {
        var customer = _customerService.GetCustomerByCode(code);

        if (customer is null)
            return NotFound("Customer not found");

        return customer;
    }
    
    [HttpGet("/emails/{email}")]
    [SwaggerOperation(Summary = "Return Customer from Email")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<Customer> GetCustomerByEmail([FromQuery] string email)
    {
        var customer = _customerService.GetCustomerByEmail(email);

        if (customer is null)
            return NotFound("Customer not found");

        return customer;
    }
    
    [HttpPatch("{customerId:guid}")]
    [SwaggerOperation(Summary = "Modify a customer")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Customer), 200)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public ActionResult<Customer> Patch([FromBody] CustomerBlueprint body, [FromQuery] Guid customerId)
    {
        
        if(customerId == Guid.Empty) return BadRequest("Id is required");
        
        Customer? customer = _customerService.GetCustomerById(customerId);
        
        if(customer is null) return NotFound("Customer not found");
        
        Address? address = null;
        
        if(body.Address is not null)
            address = _addressService.GetAddressById((Guid) body.Address.Id);

        try
        {
            customer = _customerService.ModifyCustomer(
                customer,
                body.Code, 
                body.FirstName, 
                body.LastName, 
                body.Email,
                body.PhoneNumber,
                address);
        }
        catch (Exception e)
        {
            if (e is ArgumentNullException)
                return BadRequest(e.Message);

            Console.WriteLine(e);
            throw;
        }

        return customer;
    }
}