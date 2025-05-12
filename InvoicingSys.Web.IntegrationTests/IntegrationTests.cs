using System.Net;
using System.Net.Http.Json;
using InvoicingSys.CoreApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace InvoicingSys.Web.IntegrationTests;

public class IntegrationTests
{
    private readonly HttpClient _client;
    
    [Fact]
    public async Task CreateAddress()
    {
        JsonContent content = JsonContent.Create(
            new
            {
                City = "London",
                Street = "123 Main Street",
                ZipCode = "12345",
                LaneNumber = "15"
            });

        var response = await _client.PostAsync("/core/addresses/add", content);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
    
    [Fact]
    public void GetAllAddresses()
    {
        var response = _client.GetAsync("/core/addresses");
        response.Result.EnsureSuccessStatusCode();
    }
}