namespace InvoicingSys.CoreApi.Core.Blueprints;

public class CustomerBlueprint
{
    public Guid? Id { get; init; }
    public string? Code { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public AddressBlueprint? Address { get; init; }
}