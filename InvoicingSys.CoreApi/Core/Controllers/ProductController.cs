using System.Net;
using InvoicingSys.CoreApi.Core.Blueprints;
using InvoicingSys.CoreApi.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using InvoicingSys.CoreApi.Core.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace InvoicingSys.CoreApi.Core.Controllers;

[Route("/core/products")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;
    private readonly VatTaxService _vatTaxService;

    public ProductController(ProductService productService, VatTaxService vatTaxService)
    {
        _productService = productService;
        _vatTaxService = vatTaxService;
    }

    [HttpPost("add")]
    [SwaggerOperation(Summary = "Creates a product")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Product), 200)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public ActionResult<Product> Post([FromBody] ProductBlueprint body)
    {
        Product? product = null;
        VatTax? vatTax = null;

        if (body.VatTax is not null)
        {
            vatTax = _vatTaxService.GetVatTaxById((Guid) body.VatTax.Id);
            
            if(vatTax is null)
                return NotFound("VatTax not found");
        }

        try
        {
            product = _productService.CreateProduct(
                body.Name,
                body.Price,
                vatTax);
        }
        catch (Exception e)
        {
            if (e is ArgumentNullException)
                return BadRequest(e.Message);

            Console.WriteLine(e);
            throw;
        }

        return product;
    }

    [HttpPost("")]
    [SwaggerOperation(Summary = "Creates products")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public ActionResult<IEnumerable<Product>> Post([FromBody] IEnumerable<ProductBlueprint> body)
    {
        Product? product = null;
        var products = new List<Product>();
        VatTax? vatTax = null;

        foreach (var productBlueprint in body)
            try
            {

                if (productBlueprint.VatTax is not null)
                {
                    vatTax = _vatTaxService.GetVatTaxById((Guid) productBlueprint.VatTax.Id);
            
                    if(vatTax is null)
                        return NotFound("VatTax not found");
                }
                
                product = _productService.CreateProduct(
                    productBlueprint.Name, 
                    productBlueprint.Price, 
                    vatTax);

                products.Add(product);
            }
            catch (Exception e)
            {
                if (e is ArgumentNullException)
                    return BadRequest(e.Message);

                Console.WriteLine(e);
                throw;
            }
        
        return products;
    }

    [HttpGet("")]
    [SwaggerOperation(Summary = "Returns all products")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<Product>> GetProducts()
    {
        var products = _productService.GetProducts();

        return products;
    }


    [HttpGet("{productId:guid}")]
    [SwaggerOperation(Summary = "Return Product from ProductId")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<Product> GetProduct([FromQuery] Guid productId)
    {
        var product = _productService.GetProductById(productId);

        if (product is null)
            return NotFound();

        return product;
    }
    
    [HttpPatch("{productId:guid}")]
    [SwaggerOperation(Summary = "Modify a product")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Product), 200)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public ActionResult<Product> Patch([FromBody] ProductBlueprint body, [FromQuery] Guid productId)
    {
        if(productId == Guid.Empty) return BadRequest("Product Id cannot be empty");
        Product? product = _productService.GetProductById(productId);
        
        if(product is null)
            return NotFound("Product not found");
        
        VatTax? vatTax = null;
        
        if (product.VatTax is not null)
        {
            vatTax = _vatTaxService.GetVatTaxById((Guid) product.VatTax.Id);
            
            if(vatTax is null)
                return NotFound("VatTax not found");
        }
        
        try
        {
            product = _productService.ModifyProduct(
                product,
                body.Name,
                body.Price,
                vatTax);
        }
        catch (Exception e)
        {
            if (e is ArgumentNullException)
                return BadRequest();

            Console.WriteLine(e);
            throw;
        }

        return product;
    }
}