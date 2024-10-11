namespace Catalog.API.Exceptions;

public class ProductNotFoundException : Exception
{
    // Configura uma exceeção personalizada
    public ProductNotFoundException() : base("Product Not Found!")
    {
        
    }
}

