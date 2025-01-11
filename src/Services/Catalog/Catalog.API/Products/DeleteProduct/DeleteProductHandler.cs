namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : IRequest<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, DeleteProductResult>
{
    private readonly IDocumentSession _session;
    private readonly ILogger<DeleteProductHandler> _logger;

    public DeleteProductHandler(IDocumentSession session, ILogger<DeleteProductHandler> logger)
    {
        _session = session;
        _logger = logger;
    }

    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("DeleteProductHandler.Handle called with {@Command}", command);

        _session.Delete<Product>(command.Id);

        await _session.SaveChangesAsync(cancellationToken);

        return new DeleteProductResult(true);
    }
}