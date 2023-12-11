using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Controllers
{
	// In summary, this controller provides an API endpoint
	// for retrieving all products and their categories,
	// converting them to DTOs, and returning them to the client.
	// If anything goes wrong, it returns an appropriate HTTP status code.
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		readonly IProductRepository _productRepository;

		public ProductController(IProductRepository productRepository)
        {
			_productRepository = productRepository;
		}

		// This is an action method that handles HTTP GET requests.
		// It’s asynchronous, meaning it returns a Task and can use the await keyword to
		// perform asynchronous operations without blocking the thread.
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
		{
			try
			{
				var products = await _productRepository.GetItems();
				var productCategories = await _productRepository.GetCategories();

				if(products == null || productCategories == null)
				{
					return NotFound();
				}
				else
				{
					var productDtos = products.ConvertToDto(productCategories);

					return Ok(productDtos);
				}
			}
			// If an exception is thrown during the execution of the method,
			// it’s caught and the method returns a 500 Internal Server Error status code.
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError,
					"Error retrieving data from the database");
			}
		}
    }
}
