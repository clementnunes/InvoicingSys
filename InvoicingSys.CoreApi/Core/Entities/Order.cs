using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace InvoicingSys.CoreApi.Core.Entities;

[Table("order")]
public class Order
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid _id;
    
    [NotNull] 
    [Column("order_lines")] 
    [Required(ErrorMessage = "OrderLines cannot be empty.")]
    public List<OrderLine>? _orderLines = default!;

    [NotNull] 
    [Column("order_date")] 
    [Required(ErrorMessage = "OrderDate cannot be empty.")]
    public DateTime? _orderDate = default!;
    
    [NotNull] 
    [Column("customer")] 
    [Required(ErrorMessage = "Customer cannot be empty.")]
    public Customer? _customer = default!;
    
    public Guid Id
    {
        get => _id;
        private set => _id = value;
    }
    
    public List<OrderLine>? OrderLines
    {
        get => _orderLines;
        set => _orderLines = value ?? throw new ArgumentNullException(nameof(OrderLines), "OrderLines cannot be null");
    }
    
    public DateTime? OrderDate
    {
        get => _orderDate;
        set => _orderDate = value ?? throw new ArgumentNullException(nameof(OrderDate), "OrderDate cannot be null");
    }
    
    public Customer? Customer
    {
        get => _customer;
        set => _customer = value ?? throw new ArgumentNullException(nameof(Customer), "Customer cannot be null");
    }
    
    public Order()
    {
        _id = Guid.NewGuid();
        _orderLines = new List<OrderLine>();
    }
    
    public Order(List<OrderLine> orderLines, DateTime orderDate, Customer? customer)
    {
        _id = Guid.NewGuid();
        _orderLines = orderLines;
        _orderDate = orderDate;
        _customer = customer;
    }
}