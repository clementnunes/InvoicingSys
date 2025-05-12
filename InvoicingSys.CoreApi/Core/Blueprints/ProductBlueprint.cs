namespace InvoicingSys.CoreApi.Core.Blueprints;

public class ProductBlueprint
{
    public Guid? Id { get; init; }
    public string? Name { get; init; }
    public decimal? Price { get; init; }
    public decimal? VatTax { get; init; }
}