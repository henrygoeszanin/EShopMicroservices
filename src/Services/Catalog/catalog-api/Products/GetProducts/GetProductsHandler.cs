namespace Catalog.API.Products.GetProducts;

// Define um record para a query de produtos, com parâmetros opcionais para número da página e tamanho da página (padrão é 10)
public record GetProductsQuery(int? PageNumber, int? PageSize = 10) : IQuery<GetProductsResult>;

// Define um record para o resultado da query de produtos, contendo uma lista de produtos
public record GetProductsResult(IEnumerable<Product> Products);

// Define a classe GetProductsQueryHandler que implementa a interface IQueryHandler para lidar com a query de produtos
internal class GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    // Método para lidar com a query de produtos
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        // Loga a chamada do método Handle com a query recebida
        logger.LogInformation("GetProductsQueryHandler.Handle called with {@Query}", query);

        // Executa a query no banco de dados para obter a lista de produtos paginada
        var products = await session.Query<Product>().ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

        // Retorna o resultado da query com a lista de produtos
        return new GetProductsResult(products);
    }
}