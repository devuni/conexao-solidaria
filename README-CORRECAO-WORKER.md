# Correção do Worker de Doações

Este projeto contém as correções para o problema em que o `donation-worker` não processava as mensagens do RabbitMQ.

## Problemas corrigidos

1. O Dockerfile do Worker usava `mcr.microsoft.com/dotnet/runtime:8.0`, mas o projeto passou a depender do runtime `Microsoft.AspNetCore.App` por causa das dependências de infraestrutura. A imagem foi alterada para `mcr.microsoft.com/dotnet/aspnet:8.0`.

2. O método `AddInfrastructure` registrava autenticação e autorização HTTP. Isso fazia o Worker falhar ao iniciar com erro de `AuthorizationPolicyCache` e `EndpointDataSource`, porque Worker não possui pipeline HTTP. A configuração foi separada em:

   - `AddInfrastructure(...)`: dependências compartilhadas para API e Worker;
   - `AddJwtAuthentication(...)`: autenticação e autorização, chamada somente pela API.

## Como recriar o ambiente

Execute na raiz do projeto:

```bash
dotnet clean ConexaoSolidaria.sln
dotnet restore ConexaoSolidaria.sln
dotnet build ConexaoSolidaria.sln
dotnet test ConexaoSolidaria.sln
```

Depois recrie os containers:

```bash
docker compose down -v
docker compose build --no-cache
docker compose up
```

## Como validar

Em outro terminal:

```bash
docker compose logs -f donation-worker
```

O Worker deve iniciar sem erro. Depois envie uma doação pelo Swagger e valide:

```bash
docker exec -it conexao-solidaria-rabbitmq rabbitmqctl list_queues name messages_ready messages_unacknowledged consumers
```

A fila `doacao-recebida` deve aparecer com `consumers = 1`.

Por fim, consulte:

```text
GET /api/v1/transparencia/campanhas
```

O campo `valorTotalArrecadado` deve ser atualizado pelo Worker.
