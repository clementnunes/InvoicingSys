using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace InvoicingSys.CoreApi.Core.Entities;

[Table("invoice")]
public class Invoice
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid _id;

    [NotNull] 
    [Column("order")] 
    [Required(ErrorMessage = "Order cannot be empty.")]
    public Order? _order = default!;

    [NotNull] 
    [Column("due_date")] 
    [Required(ErrorMessage = "Due Date cannot be empty.")]
    public DateTime? _dueDate = default!;
    
    [NotNull] 
    [Column("invoicing_date")] 
    [Required(ErrorMessage = "Invoicing Date cannot be empty.")]
    public DateTime? _invoicingDate = default!;
    
    [NotNull] 
    [Column("bank_detail")] 
    [Required(ErrorMessage = "Bank Detail cannot be empty.")]
    public BankDetail? _bankDetail = default!;
    
    public Guid Id
    {
        get => _id;
        private set => _id = value;
    }
    
    public Order? Order
    {
        get => _order;
        set => _order = value ?? throw new ArgumentNullException(nameof(Order), "Order cannot be null");
    }
    
    public DateTime? DueDate
    {
        get => _dueDate;
        set => _dueDate = value ?? throw new ArgumentNullException(nameof(DueDate), "DueDate cannot be null");
    }
    
    public DateTime? InvoicingDate
    {
        get => _invoicingDate;
        set => _invoicingDate = value ?? throw new ArgumentNullException(nameof(InvoicingDate), "InvoicingDate cannot be null");
    }
    
    public BankDetail? BankDetail
    {
        get => _bankDetail;
        set => _bankDetail = value ?? throw new ArgumentNullException(nameof(BankDetail), "BankDetail cannot be null");
    }
    
    public Invoice()
    {
        _id = Guid.NewGuid();
    }

    public Invoice(Order? order, DateTime? dueDate, DateTime? invoicingDate, BankDetail? bankDetail)
    {
        _id = Guid.NewGuid();
        _order = order;
        _dueDate = dueDate;
        _invoicingDate = invoicingDate;
        _bankDetail = bankDetail;
    }
}