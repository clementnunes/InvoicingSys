using InvoicingSys.CoreApi.Core.DataContext.DBContexts;
using InvoicingSys.CoreApi.Core.Entities;

namespace InvoicingSys.CoreApi.Core.Services;

public class InvoicingService
{
    private readonly ApplicationDbContext _context;
    
    public InvoicingService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public Order? _order = default!;
    public DateTime? _dueDate = default!;
    public DateTime? _invoicingDate = default!;
    public BankDetail? _bankDetail = default!;
    
    public Invoice CreateInvoice(Order? order, DateTime? dueDate, BankDetail? bankDetail)
    {
        if (order is null)
            throw new ArgumentNullException(nameof(order), "Order cannot be null");
        
        if (dueDate is null)
            throw new ArgumentNullException(nameof(dueDate), "DueDate name cannot be null");
        
        if (bankDetail is null)
            throw new ArgumentNullException(nameof(bankDetail), "BankDetail cannot be null");

        Invoice invoice = new Invoice(order, dueDate, DateTime.Today, bankDetail);
        
        _context.Invoices.Add(invoice);
        _context.SaveChanges();
        return invoice;
    }
    
    public Invoice ModifyInvoice(Invoice invoice, Order? order, DateTime? dueDate, BankDetail? bankDetail)
    {
        if(order is not null)
            invoice.Order = order;
            
        if(dueDate is not null)
            invoice.DueDate = dueDate;
        
        if(bankDetail is not null)
            invoice.BankDetail = bankDetail;
        
        _context.SaveChanges();
        
        return invoice;
    }

    public bool DeleteInvoice(Invoice invoice)
    {
        _context.Invoices.Remove(invoice);
        _context.SaveChanges();
        return true;
    }

    public List<Invoice> GetInvoices()
    {
        return _context.Invoices.ToList();
    }

    public Invoice? GetInvoiceById(Guid invoiceId)
    {
        return _context.Invoices.FirstOrDefault(i => i.Id == invoiceId);
    }
}