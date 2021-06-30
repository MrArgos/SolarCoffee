using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffee.Services.Product;
using SolarCoffee.Web.Serialization;
using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet("/api/product")]
        public ActionResult GetProduct()
        {
            _logger.LogInformation("Getting all products");
            var products = _productService.GetAllProducts();
            var productViewModels = products.Select(ProductMapper.SerializeProductModel);
            return Ok(productViewModels);
        }

        [HttpGet("/api/product/{id:int}")]
        public ActionResult GetProduct(int id)
        {
            _logger.LogInformation("Getting product with id: {Id}", id);
            var product = _productService.GetProductById(id);
            var productViewModel = ProductMapper.SerializeProductModel(product);
            return Ok(productViewModel);
        }

        [HttpPost("/api/product/")]
        public ActionResult AddProduct([FromBody] ProductModel product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            _logger.LogInformation("Creating new product with name '{Name}'", product.Name);
            var productData = ProductMapper.SerializeProductModel(product);
            var response = _productService.CreateProduct(productData);
            return Ok(response);
        }

        [HttpPatch("/api/product/{id:int}")]
        public ActionResult ArchiveProduct(int id)
        {
            _logger.LogInformation("Archiving Product");
            var archiveResult = _productService.ArchiveProduct(id);
            return Ok(archiveResult);
        }
        
    }
}