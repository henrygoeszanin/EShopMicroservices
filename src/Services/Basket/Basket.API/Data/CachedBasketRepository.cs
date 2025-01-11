using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Data;

public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
{
    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        await cache.RemoveAsync(userName, cancellationToken);

        await repository.DeleteBasket(userName, cancellationToken);

        return true;
    }

    public async  Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
    {
        var CachedBasket = await cache.GetStringAsync(userName, cancellationToken);

        if (!string.IsNullOrEmpty(CachedBasket))
        {
            return JsonSerializer.Deserialize<ShoppingCart>(CachedBasket)!;
        }

        var basket = await repository.GetBasket(userName, cancellationToken);

        await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket));

        return basket;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        await repository.StoreBasket(basket, cancellationToken);

        await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket));

        return basket;
    }
}