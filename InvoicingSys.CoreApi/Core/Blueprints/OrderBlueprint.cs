namespace InvoicingSys.CoreApi.Core.Blueprints;

public class OrderBlueprint
{
    public Guid? Id { get; init; }
    public List<OrderLineBlueprint>? OrderLines { get; init; }
    public DateTime? OrderDate { get; init; }
    public CustomerBlueprint? Customer { get; init; }
}