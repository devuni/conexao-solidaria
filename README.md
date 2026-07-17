# Conexão Solidária

MVP desenvolvido para o Hackaton da pós-graduação FIAP.

## Arquitetura inicial

A solução possui:

- ASP.NET Core Web API;
- Worker assíncrono;
- PostgreSQL;
- MongoDB;
- RabbitMQ;
- Docker Compose;
- pipeline inicial com GitHub Actions;
- projeto de testes xUnit.

## Pré-requisitos

- .NET SDK 8;
- Docker Desktop ou Docker Engine com Docker Compose;
- Git.

## Executar toda a solução com Docker

Na raiz do repositório:

```bash
docker compose up --build
```

Serviços disponíveis:

| Serviço | Endereço |
|---|---|
| API | http://localhost:8080 |
| Swagger | http://localhost:8080/swagger |
| Health Check | http://localhost:8080/health |
| Status | http://localhost:8080/api/v1/system/status |
| RabbitMQ Management | http://localhost:15672 |
| PostgreSQL | localhost:5432 |
| MongoDB | localhost:27017 |

Credenciais locais do RabbitMQ:

- usuário: `guest`
- senha: `guest`

## Executar somente a infraestrutura

PowerShell:

```powershell
./scripts/start-infra.ps1
```

Linux/macOS:

```bash
chmod +x ./scripts/start-infra.sh
./scripts/start-infra.sh
```

## Compilar e testar localmente

```bash
dotnet restore ConexaoSolidaria.sln
dotnet build ConexaoSolidaria.sln
dotnet test ConexaoSolidaria.sln
```

## Estrutura

```text
src/
├── ConexaoSolidaria.Api
├── ConexaoSolidaria.Application
├── ConexaoSolidaria.Contracts
├── ConexaoSolidaria.Domain
├── ConexaoSolidaria.Infrastructure
└── ConexaoSolidaria.DonationWorker

tests/
└── ConexaoSolidaria.Domain.Tests
```

## Próximas etapas

1. Modelar usuários e perfis.
2. Implementar cadastro de doador.
3. Implementar JWT e RBAC.
4. Modelar campanhas.
5. Implementar doação assíncrona com RabbitMQ.
6. Adicionar Kubernetes.
7. Adicionar Grafana e Zabbix.


## Módulo 3 — Campanhas

Endpoints de campanhas para gestor:

```text
POST /api/v1/campanhas
PUT  /api/v1/campanhas/{id}
GET  /api/v1/campanhas
GET  /api/v1/campanhas/{id}
```

Endpoint público de transparência:

```text
GET /api/v1/transparencia/campanhas
```

Status permitidos:

```text
Ativa
Concluida
Cancelada
```


## Correção do Worker de Doações

Veja `README-CORRECAO-WORKER.md` para os detalhes da correção do Worker, Dockerfile e separação da autenticação JWT da infraestrutura compartilhada.
