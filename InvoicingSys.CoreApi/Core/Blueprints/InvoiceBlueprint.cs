namespace InvoicingSys.CoreApi.Core.Blueprints;

public class InvoiceBlueprint
{
    public Guid Id { get; init; }
    public OrderBlueprint? Order { get; init; }
    public DateTime? DueDate { get; init; }
    public DateTime? InvoicingDate { get; init; }
    public BankDetailBlueprint? BankDetail { get; init; }
}