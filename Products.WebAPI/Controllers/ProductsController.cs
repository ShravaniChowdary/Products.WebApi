using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Api.Models;
using Products.Api.Services;
using System.Net;

namespace Products.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await this.productService.GetAllAsync();
                return new JsonResult(products);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    Message = "An error occured while fetching the products.",
                    ExceptionDetails = ex.Message
                });
            }
        }

        [HttpGet("{productId}")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById([FromRoute] int productId)
        {
            try
            {
                var product = await this.productService.GetByIdAsync(productId);
                return product != null ? new JsonResult(product) : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    Message = "An error occured while fetching the product.",
                    ExceptionDetails = ex.Message
                });
            }
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] ProductDto product)
        {
            try
            {
                var isCreated = await this.productService.CreateAsync(product);
                return isCreated ? Ok("Product created successfully!") : BadRequest("Something went wrong, please try again");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    Message = "An error occured while Creating the product.",
                    ExceptionDetails = ex.Message
                });
            }
        }

        [HttpPut("{productId}")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update([FromRoute] int productId, [FromBody] ProductDto product)
        {
            try
            {
                var isUpdated = await this.productService.UpdateAsync(productId, product);
                return isUpdated ? Ok("Product updated successfully") : BadRequest("Product updation failed");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    Message = "An error occured while updating the product.",
                    ExceptionDetails = ex.Message
                });
            }
        }

        [HttpDelete("{productId}")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] int productId)
        {
            try
            {
                var isDeleted = await this.productService.DeleteAsync(productId);
                return isDeleted ? Ok("Product deleted successfully") : BadRequest("Product deletion failed");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    Message = "An error occured while deleting the product.",
                    ExceptionDetails = ex.Message
                });
            }
        }

        [HttpPut("decrement-stock/{productId}/{quantity}")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> DecrementProductStock([FromRoute] int productId, [FromRoute] int quantity)
        {
            try
            {
                string decrementStockResponse = await this.productService.DecrementStockAsync(productId, quantity);
                return Ok(decrementStockResponse);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    Message = "An error occured while decrementing the stock of product.",
                    ExceptionDetails = ex.Message
                });
            }
        }

        [HttpPut("add-to-stock/{productId}/{quantity}")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> IncrementProductStock([FromRoute] int productId, [FromRoute] int quantity)
        {
            try
            {
                string incrementStockResponse = await this.productService.IncrementStockAsync(productId, quantity);
                return Ok(incrementStockResponse);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    Message = "An error occured while incrementing the stock of product.",
                    ExceptionDetails = ex.Message
                });
            }
        }
    }
}
