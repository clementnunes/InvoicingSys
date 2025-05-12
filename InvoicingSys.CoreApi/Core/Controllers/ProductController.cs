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

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpPost("add")]
    [SwaggerOperation(Summary = "Creates an product")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Product), 200)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public ActionResult<Product> Post([FromBody] ProductBlueprint body)
    {
        Product? product = null;

        try
        {
            product = _productService.CreateProduct(
                body.Name,
                body.Price,
                body.VatTax);
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

    [HttpPost("")]
    [SwaggerOperation(Summary = "Creates products")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    public ActionResult<IEnumerable<Product>> Post([FromBody] IEnumerable<ProductBlueprint> body)
    {
        Product? product = null;
        var products = new List<Product>();
        
        foreach (var productBlueprint in body)
            try
            {
                product = _productService.CreateProduct(
                    productBlueprint.Name, 
                    productBlueprint.Price, 
                    productBlueprint.VatTax);

                products.Add(product);
            }
            catch (Exception e)
            {
                if (e is ArgumentNullException)
                    return BadRequest();

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
    
}