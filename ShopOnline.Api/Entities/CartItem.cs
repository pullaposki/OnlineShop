namespace ShopOnline.Api.Entities
{
	public class CartItem
	{
		public int Id { get; set; }

		// a foreign key field. Is used to join with the Cart entity.
		// Cart has one to many relationship with CartItem
		// i.e. many items can be in one cart.
		public int CartId {  get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; }
    }
}
