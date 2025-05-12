namespace InvoicingSys.CoreApi.Core.Blueprints;

public class BankDetailBlueprint
{
    public Guid Id { get; init; }
    public string? Location { get; init; }
    public string? OwnerName { get; init; }
    public string? Iban { get; init; }
    public string? Bic { get; init; }
}