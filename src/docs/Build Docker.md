## Iniciar o docker compose das aplicações

### Configuração para catalog.api
1. Dentro da pasta src digitar o comando de build:
```
docker build -t catalogapi -f Services/Catalog/catalog-api/Dockerfile .
```

## Comando para subir o docker compose com todas as aplicações

```
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
```
