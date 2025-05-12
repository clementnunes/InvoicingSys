namespace InvoicingSys.CoreApi.Core.Blueprints;

public class OrderLineBlueprint
{
    public Guid? Id { get; init; }
    public ProductBlueprint? BoughtProduct { get; init; }
    public int? Quantity { get; init; }
    public decimal? VatTax { get; init; }
    public decimal? UnitPrice { get; init; }
}