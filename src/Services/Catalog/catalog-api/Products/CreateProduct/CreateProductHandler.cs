namespace Catalog.API.Products.CreateProduct;

// Define o comando de criação de produto
public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<CreateProductResult>;
// Define o resultado que será retornado após a criação do produto
public record CreateProductResult(Guid Id);

// Manipulador do comando CreateProductCommand que trata a lógica de criação d eum novo produto
internal class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // Criar um entidade de produto a partir do command object
        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price,
        };

        //armazena o novo produto na sessao do banco de dados
        session.Store(product);

        //salva as alterações no banco de dados de forma assíncrona
        await session.SaveChangesAsync(cancellationToken);

        // Retornar o resultado contendo o ID do produto criado
        return new CreateProductResult(product.Id);


    }
}