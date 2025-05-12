using InvoicingSys.CoreApi.Core.DataContext.DBContexts;
using InvoicingSys.CoreApi.Core.Entities;

namespace InvoicingSys.CoreApi.Core.Services;

public class BankDetailService
{
    private readonly ApplicationDbContext _context;
    
    public BankDetailService(ApplicationDbContext context)
    {
        _context = context;
    }

    public BankDetail CreateBankDetail(string? location, string? ownerName, string? iban, string? bic)
    {
        if (location is null)
            throw new ArgumentNullException(nameof(location), "Location cannot be null");
        
        if (ownerName is null)
            throw new ArgumentNullException(nameof(ownerName), "OwnerName cannot be null");
        
        if (iban is null)
            throw new ArgumentNullException(nameof(iban), "IBAN cannot be null");
        
        if (bic is null)
            throw new ArgumentNullException(nameof(bic), "BIC cannot be null");
        
        BankDetail bankDetail = new BankDetail(location, ownerName, iban, bic);
        
        _context.BankDetails.Add(bankDetail);
        _context.SaveChanges();
        return bankDetail;
    }
    
    public BankDetail ModifyBankDetail(BankDetail bankDetail, string? location, string? ownerName, string? iban, string? bic)
    {
        if(location is not null)
            bankDetail.Location = location;
        
        if(ownerName is not null)
            bankDetail.OwnerName = ownerName;
        
        if(iban is not null)
            bankDetail.Iban = iban;
        
        if(bic is not null)
            bankDetail.Bic = bic;
        
        _context.SaveChanges();
        
        return bankDetail;
    }

    public bool DeleteBankDetail(BankDetail bankDetail)
    {
        _context.BankDetails.Remove(bankDetail);
        _context.SaveChanges();
        return true;
    }

    public List<BankDetail> GetBankDetails()
    {
        return _context.BankDetails.ToList();
    }

    public BankDetail? GetBankDetailById(Guid bankDetailId)
    {
        return _context.BankDetails.FirstOrDefault(i => i.Id == bankDetailId);
    }
}