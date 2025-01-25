## Iniciar o docker compose das aplicações

### Configuração para catalog.api
1. Dentro da pasta src digitar o comando de build:
```
docker build -t catalogapi -f Services/Catalog/Catalog.API/Dockerfile .
```

## Comando para subir o docker compose com todas as aplicações

```
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
```

### Comando para criar um novo projeto vazio
```
dotnet new web -o NomeDoProjeto
```

# Create new web API project
dotnet new webapi -o ProjectName

# Create new web app
dotnet new web -o ProjectName

# Create new class library
dotnet new classlib -o ProjectName

# Create new solution
dotnet new sln -n SolutionName

# Build project
dotnet build

# Run project
dotnet run

# Watch for changes and rebuild
dotnet watch run

# Run with specific profile
dotnet run --launch-profile profilename

# Publish project
dotnet publish -c Release

# Build project
dotnet build

# Run project
dotnet run

# Watch for changes and rebuild
dotnet watch run

# Run with specific profile
dotnet run --launch-profile profilename

# Publish project
dotnet publish -c Release

# Add package to project
dotnet add package PackageName

# Add specific version
dotnet add package PackageName -v VersionNumber

# Remove package
dotnet remove package PackageName

# Restore packages
dotnet restore

# Run tests
dotnet test

# Run tests with filter
dotnet test --filter "FullyQualifiedName~TestName"

# Run tests with coverage
dotnet test /p:CollectCoverage=true

# Watch tests
dotnet watch test

# Add project to solution
dotnet sln add ProjectPath

# Remove project from solution
dotnet sln remove ProjectPath

# List projects in solution
dotnet sln list

# Install entity framework tools
dotnet tool install --global dotnet-ef

# Install user-secrets tool
dotnet tool install --global dotnet-user-secrets

# Update tools
dotnet tool update --global toolname

# Install entity framework tools
dotnet tool install --global dotnet-ef

# Install user-secrets tool
dotnet tool install --global dotnet-user-secrets

# Update tools
dotnet tool update --global toolname

# Trust HTTPS development certificate
dotnet dev-certs https --trust

# Clean HTTPS development certificates
dotnet dev-certs https --clean