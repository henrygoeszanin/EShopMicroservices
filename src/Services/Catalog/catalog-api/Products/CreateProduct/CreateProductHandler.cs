namespace Catalog.API.Products.CreateProduct;

// Define o comando de criação de produto
public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<CreateProductResult>;
// Define o resultado que será retornado após a criação do produto
public record CreateProductResult(Guid Id);

// Classe para validação dos dados recebidos
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

// Manipulador do comando CreateProductCommand que trata a lógica de criação de um novo produto
internal class CreateProductCommandHandler(IDocumentSession session, IValidator<CreateProductCommand> validator) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {

        // Chama a validação de entrada de dados
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        var validationErrors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
        if (validationErrors.Any()) {
            throw new ValidationException(validationErrors.FirstOrDefault());
        }

        // Criar uma entidade de produto a partir do command object
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