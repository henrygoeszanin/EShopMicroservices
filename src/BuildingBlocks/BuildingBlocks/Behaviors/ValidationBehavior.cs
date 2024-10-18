using BuildingBlocks.CQRS;     
using FluentValidation;         
using MediatR;                   

namespace BuildingBlocks.Behaviors  
{
    // Define uma classe que implementa um comportamento de pipeline para validação de requests
    // TRequest representa o tipo do comando/requisição e TResponse o tipo da resposta
    public class ValidationBehavior<TRequest, TResponse>
        (IEnumerable<IValidator<TRequest>> validators) // Injeta os validadores que serão aplicados ao request
        : IPipelineBehavior<TRequest, TResponse> // Implementa o comportamento de pipeline (MediatR)
        where TRequest : ICommand<TRequest> // Restringe o TRequest para ser do tipo ICommand<TRequest>
    {
        // Método principal que intercepta a requisição para aplicar validações antes de passar adiante
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Cria um contexto de validação baseado na requisição atual
            var context = new ValidationContext<TRequest>(request);

            // Executa todos os validadores simultaneamente (em paralelo), aplicando-os ao request
            var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            // Extrai todas as falhas de validação e coleta os erros em uma lista
            var failures = validationResults.Where(r => r.Errors.Any()) // Verifica se algum resultado de validação contém erros
                .SelectMany(r => r.Errors) // Coleta todos os erros de todos os resultados
                .ToList(); // Converte a lista de falhas para um List<T>

            // Se houver qualquer falha de validação, lança uma exceção com os erros encontrados
            if (failures.Any())
                throw new ValidationException(failures);

            // Se não houver erros, continua o pipeline e chama o próximo delegado (handler)
            return await next();
        }
    }
}
