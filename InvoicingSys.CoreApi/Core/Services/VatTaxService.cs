using InvoicingSys.CoreApi.Core.DataContext.DBContexts;
using InvoicingSys.CoreApi.Core.Entities;

namespace InvoicingSys.CoreApi.Core.Services;

public class VatTaxService
{
    private readonly ApplicationDbContext _context;
    
    public VatTaxService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public VatTax CreateVatTax(string? label, decimal? rate)
    {
        if (label is null)
            throw new ArgumentNullException(nameof(label), "Label cannot be null");
        
        if (rate is null)
            throw new ArgumentNullException(nameof(rate), "Rate cannot be null");
        
        VatTax vatTax = new VatTax(label, rate);
        
        _context.VatTaxes.Add(vatTax);
        _context.SaveChanges();
        return vatTax;
    }
    
    public VatTax ModifyVatTax(VatTax vatTax, string? label, decimal? rate)
    {
        if(label is not null)
            vatTax.Label = label;
        
        if(rate is not null)
            vatTax.Rate = (decimal) rate;
        
        _context.SaveChanges();
        
        return vatTax;
    }

    public bool DeleteVatTax(VatTax vatTax)
    {
        _context.VatTaxes.Remove(vatTax);
        _context.SaveChanges();
        return true;
    }

    public List<VatTax> GetVatTaxes()
    {
        return _context.VatTaxes.ToList();
    }

    public VatTax? GetVatTaxById(Guid vatTaxId)
    {
        return _context.VatTaxes.FirstOrDefault(i => i.Id == vatTaxId);
    }
}