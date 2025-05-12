namespace InvoicingSys.CoreApi.Core.Blueprints;

public class AddressBlueprint
{
    public Guid? Id { get; init; }
    public string? LaneNumber { get; init; }
    public string? Street { get; init; }
    public string? ZipCode { get; init; }
    public string? City { get; init; }
}