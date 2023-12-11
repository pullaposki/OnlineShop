using ShopOnline.Api.Entities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Extensions
{
	public static class DtoConversions
	{

		// in summary, this method takes in a list of Product entities and a list of ProductCategory entities,
		// joins them together based on category ID,
		// and then projects the result into a list of ProductDto objects.
		// Each ProductDto includes the product’s category name directly,
		// which might be useful when sending data to a client application.
		public static IEnumerable<ProductDto> ConvertToDto(
			this IEnumerable<Product> products, IEnumerable<ProductCategory> productCategories)
		{
			return (from product in products
					join productCategory in productCategories
					on product.CategoryId equals productCategory.Id
					select new ProductDto
					{
						Id = product.Id,
						Name = product.Name,
						Description = product.Description,
						ImageURL = product.ImageURL,
						Price = product.Price,
						Qty = product.Qty,
						CategoryId = product.CategoryId,
						CategoryName = productCategory.Name
					}).ToList();
		}
	}
}
