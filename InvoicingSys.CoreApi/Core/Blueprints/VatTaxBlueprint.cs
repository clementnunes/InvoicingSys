namespace InvoicingSys.CoreApi.Core.Blueprints;

public class VatTaxBlueprint
{
    public Guid? Id { get; init; }
    public string? Label { get; init; }
    public decimal? Rate { get; init; }
}