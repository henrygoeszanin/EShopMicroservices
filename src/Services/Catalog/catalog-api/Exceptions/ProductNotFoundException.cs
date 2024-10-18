using BuildingBlocks.Exceptions;

namespace Catalog.API.Exceptions;

public class ProductNotFoundException : NotFoundException
{
    // Configura uma exceeção personalizada
    public ProductNotFoundException(Guid id) : base("Product", id)
    {

    }
}
