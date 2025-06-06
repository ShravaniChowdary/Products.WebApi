using Microsoft.AspNetCore.Mvc;
using Moq;
using Products.Api.Controllers;
using Products.Api.Models;
using Products.Api.Services;
using Products.WebAPI.Models.Domain;
using System.Net;

namespace Products.Tests.Controllers
{
    [TestClass]
    public class ProductControllerTests
    {
        readonly List<Product> products = new List<Product>
            {
                new Product
                {
                    Id = 123456,
                    Name = "Test",
                    Price = 12500.00M,
                    Stock = 100,
                    CreatedAt = DateTime.Now,
                }
            };

        private Mock<IProductService> productService = default!;
        private ProductsController productController =  default!;

        [TestInitialize]
        public void Setup()
        {
            productService = new Mock<IProductService>();
            productController = new ProductsController(productService.Object);
        }

        [TestMethod]
        public async Task GetAll_ReturnsProductsSuccessfully()
        {
            // Arrange
            productService.Setup(x => x.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await productController.GetAll() as JsonResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(products, result.Value);
        }

        [TestMethod]
        public async Task GetAll_ReturnsInternalServerError()
        {
            // Arrange
            productService.Setup(x => x.GetAllAsync()).ThrowsAsync(new Exception("Something went wrong!"));

            // Act
            var result = await productController.GetAll() as ObjectResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, result?.StatusCode);
        }

        [TestMethod]
        public async Task GetById_ReturnProductSuccessfully_IfFound()
        {
            productService.Setup(s => s.GetByIdAsync(123456)).ReturnsAsync(products.FirstOrDefault());
            var result = await productController.GetById(123456) as JsonResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(products.FirstOrDefault(), result.Value);
        }

        [TestMethod]
        public async Task GetById_ReturnNotFound_IfNotFound()
        {
            productService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((Product)null);
            var result = await productController.GetById(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Create_ShouldReturnOk_IfCreated()
        {
            var model = new ProductDto { Name = "New Product" };
            productService.Setup(s => s.CreateAsync(model)).ReturnsAsync(true);

            var result = await productController.Create(model) as OkObjectResult;

            Assert.AreEqual((int)HttpStatusCode.OK, result?.StatusCode);
        }

        [TestMethod]
        public async Task Create_ShouldReturnBadRequest_IfCreationFails()
        {
            var model = new ProductDto { Name = "Invalid Product" };
            productService.Setup(s => s.CreateAsync(model)).ReturnsAsync(false);

            var result = await productController.Create(model) as BadRequestObjectResult;

            Assert.AreEqual((int)HttpStatusCode.BadRequest, result?.StatusCode);
        }

        [TestMethod]
        public async Task Update_ShouldReturnOk_IfUpdated()
        {
            var model = new ProductDto { Name = "Updated Product" };
            productService.Setup(s => s.UpdateAsync(123456, model)).ReturnsAsync(true);

            var result = await productController.Update(123456, model) as OkObjectResult;

            Assert.AreEqual((int)HttpStatusCode.OK, result?.StatusCode);
        }

        [TestMethod]
        public async Task Update_ShouldReturnBadRequest_IfUpdateFails()
        {
            var model = new ProductDto { Name = "Updated Product" };
            productService.Setup(s => s.UpdateAsync(1, model)).ReturnsAsync(false);

            var result = await productController.Update(1, model) as BadRequestObjectResult;

            Assert.AreEqual((int)HttpStatusCode.BadRequest, result?.StatusCode);
        }

        [TestMethod]
        public async Task Delete_ShouldReturnOk_IfDeleted()
        {
            productService.Setup(s => s.DeleteAsync(123456)).ReturnsAsync(true);

            var result = await productController.Delete(123456) as OkObjectResult;

            Assert.AreEqual((int)HttpStatusCode.OK, result?.StatusCode);
        }

        [TestMethod]
        public async Task Delete_ShouldReturnBadRequest_IfDeleteFails()
        {
            productService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(false);

            var result = await productController.Delete(1) as BadRequestObjectResult;

            Assert.AreEqual((int)HttpStatusCode.BadRequest, result?.StatusCode);
        }

        [TestMethod]
        public async Task DecrementStock_ShouldReturnOk()
        {
            productService.Setup(s => s.DecrementStockAsync(123456, 2)).ReturnsAsync("Stock decremented");

            var result = await productController.DecrementProductStock(123456, 2) as OkObjectResult;

            Assert.AreEqual("Stock decremented", result?.Value);
        }

        [TestMethod]
        public async Task IncrementStock_ShouldReturnOk()
        {
            productService.Setup(s => s.IncrementStockAsync(123456, 2)).ReturnsAsync("Stock incremented");

            var result = await productController.IncrementProductStock(123456, 2) as OkObjectResult;

            Assert.AreEqual("Stock incremented", result?.Value);
        }
    }
}
