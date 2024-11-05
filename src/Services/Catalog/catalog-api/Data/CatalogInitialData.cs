using Marten.Schema;
namespace Catalog.API.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        using var session = store.LightweightSession();

        if(await session.Query<Product>().AnyAsync())
            return;
        
        session.Store<Product>(GetPreconfiguredProdutcts());

        await session.SaveChangesAsync();
    }

    private static IEnumerable<Product> GetPreconfiguredProdutcts() => new List<Product>
    {
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Product 1",
            Description = "Description 1",
            Price = 100,
            ImageFile = "product-1.png",
            Category = new List<string> { "Category 1" },
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Product 2",
            Description = "Description 2",
            Price = 200,
            ImageFile = "product-2.png",
            Category = new List<string> { "Category 2" },
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Product 3",
            Description = "Description 3",
            Price = 300,
            ImageFile = "product-3.png",
            Category = new List<string> { "Category 3" },
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Product 4",
            Description = "Description 4",
            Price = 400,
            ImageFile = "product-4.png",
            Category = new List<string> { "Category 4" },
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Product 5",
            Description = "Description 5",
            Price = 500,
            ImageFile = "product-5.png",
            Category = new List<string> { "Category 5" },
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Product 6",
            Description = "Description 6",
            Price = 600,
            ImageFile = "product-6.png",
            Category = new List<string> { "Category 6" },
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Product 7",
            Description = "Description 7",
            Price = 700,
            ImageFile = "product-7.png",
            Category = new List<string> { "Category 7" },
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Product 8",
            Description = "Description 8",
            Price = 800,
            ImageFile = "product-8.png",
            Category = new List<string> { "Category 8" },
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Product 9",
            Description = "Description 9",
            Price = 900,
            ImageFile = "product-9.png",
            Category = new List<string> { "Category 9" },
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Product 10",
            Description = "Description 10",
            Price = 1000,
            ImageFile = "product-10.png",
            Category = new List<string> { "Category 10" },
        }
    };
}
