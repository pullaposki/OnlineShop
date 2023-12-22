using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Repositories;

public class ShoppingCartRepository : IShoppingCartRepository
{
    readonly ShopOnlineDbContext _shopOnlineDbContext;

    public ShoppingCartRepository(ShopOnlineDbContext shopOnlineDbContext)
    {
        _shopOnlineDbContext = shopOnlineDbContext;
    }

    public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto)
    {
        if (await CartItemExists(cartItemToAddDto.CartId, cartItemToAddDto.ProductId)) return null;
        
        var item = await (from product in _shopOnlineDbContext.Products
            where product.Id == cartItemToAddDto.ProductId
            select new CartItem
            {
                CartId = cartItemToAddDto.CartId,
                ProductId = product.Id,
                Qty = cartItemToAddDto.Qty
            }).SingleOrDefaultAsync();

        if (item != null)
        {
            var result = await _shopOnlineDbContext.CartItems.AddAsync(item);
            await _shopOnlineDbContext.SaveChangesAsync();
            return result.Entity;
        }

        return null;
    }

    async Task<bool> CartItemExists(int cartId, int productId)
    {
        // The method uses the LINQ extension method AnyAsync,
        // which checks if any element in a sequence satisfies a given condition.
        var cartItemExists = await _shopOnlineDbContext.CartItems.AnyAsync(
            c => c.CartId == cartId && c.ProductId == productId);
        
        return cartItemExists;
    }

    public Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
    {
        throw new NotImplementedException();
    }

    public Task<CartItem> DeleteItem(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<CartItem> GetItem(int id)
    {
        return await
            (from cart in _shopOnlineDbContext.Carts
                join cartItem in _shopOnlineDbContext.CartItems on cart.Id equals cartItem.CartId
                where cartItem.Id == id
                select new CartItem
                {
                    Id = cartItem.Id,
                    ProductId = cartItem.ProductId,
                    Qty = cartItem.Qty,
                    CartId = cartItem.CartId
                }).SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<CartItem>> GetItems(int userId)
    {
        var cartId = await GetCartId(userId);
        
        var query = (from cart in _shopOnlineDbContext.Carts
            join cartItem in _shopOnlineDbContext.CartItems on cart.Id equals cartItem.CartId
            where cart.Id == cartId
            select new CartItem
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                Qty = cartItem.Qty,
                CartId = cartItem.CartId
            });

        return await query.ToListAsync();

        // return await (from cart in _shopOnlineDbContext.Carts
        //     join cartItem in _shopOnlineDbContext.CartItems
        //         on cart.Id equals cartItem.CartId
        //     where cart.UserId == userId
        //     select new CartItem
        //     {
        //         Id = cartItem.Id,
        //         ProductId = cartItem.ProductId,
        //         Qty = cartItem.Qty,
        //         CartId = cartItem.CartId
        //     }).ToListAsync();
    }

    async Task<int> GetCartId(int userId)
    {
        var cart = await _shopOnlineDbContext.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        return cart?.Id ?? 0;
    }
}