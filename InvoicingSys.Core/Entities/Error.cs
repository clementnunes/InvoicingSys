namespace InvoicingSys.Core.Entities;

public class Error
{
    public string Message { get; set; }
    public string? ParamName { get; set; }
    public string? ParamValue { get; set; }

    public Error(string message, string? paramName = null, string? paramValue = null)
    {
        Message = message;
        ParamName = paramName;
        ParamValue = paramValue;
    }
}