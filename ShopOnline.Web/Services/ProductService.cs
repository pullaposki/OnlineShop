using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Net.Http.Json;

namespace ShopOnline.Web.Services
{
	// used to interact with a web API to retrieve product data.
	public class ProductService : IProductService
	{
		// used to send HTTP requests and receive HTTP responses from a URI
		private readonly HttpClient _httpClient;

		public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

		// returns a task of an enumerable collection of ProductDto objects.
		// It sends a GET request to the “api/Product” endpoint
		public async Task<IEnumerable<ProductDto>> GetItems()
		{
			try
			{
				var response = await _httpClient.GetAsync("api/Product");

				if (response.IsSuccessStatusCode)
				{
					if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
					{
						return Enumerable.Empty<ProductDto>();
					}

					return await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
				}

				var message = await response.Content.ReadAsStringAsync();
				throw new Exception(message);
			}
			catch (Exception)
			{

				throw;
			}
		}

		// returns a task of a ProductDto object.
		// It takes an integer id as a parameter, sends a GET request to the “api/Product/{id}” endpoint
		public async Task<ProductDto> GetItem(int id)
		{
			try
			{
				var response = await _httpClient.GetAsync($"api/Product/{id}");

				if (response.IsSuccessStatusCode)
				{
					if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
					{
						return default(ProductDto);
					}

					return await response.Content.ReadFromJsonAsync<ProductDto>();
				}
				
				var message = await response.Content.ReadAsStringAsync();
				throw new Exception(message);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
