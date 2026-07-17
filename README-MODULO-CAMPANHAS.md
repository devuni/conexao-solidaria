# Módulo 3 — Gestão de Campanhas e Painel de Transparência

Este módulo adiciona:

- entidade `Campanha`;
- enum `StatusCampanha`;
- regras de negócio para criação e edição de campanhas;
- CRUD inicial de campanhas para `GestorONG`;
- endpoint público de transparência retornando somente campanhas `Ativa`;
- configuração EF Core da tabela `campanhas`;
- testes de domínio da campanha.

## Aplicar no projeto

Na raiz do projeto atual, aplique o pacote com:

```bash
unzip -o ~/Downloads/conexao-solidaria-modulo-campanhas.zip -d .
```

## Recriar o banco local

Como o projeto ainda está usando `EnsureCreated`, recrie os volumes para o PostgreSQL criar a tabela nova:

```bash
docker compose down -v
docker compose up --build
```

## Build e testes

```bash
dotnet clean ConexaoSolidaria.sln
dotnet restore ConexaoSolidaria.sln
dotnet build ConexaoSolidaria.sln
dotnet test ConexaoSolidaria.sln
```

## Endpoints adicionados

Gestão de campanhas, acesso restrito ao perfil `GestorONG`:

```text
POST /api/v1/campanhas
PUT  /api/v1/campanhas/{id}
GET  /api/v1/campanhas
GET  /api/v1/campanhas/{id}
```

Painel público de transparência:

```text
GET /api/v1/transparencia/campanhas
```

## Payload para criar campanha

Faça login como gestor:

```json
{
  "email": "gestor@conexaosolidaria.org",
  "senha": "Gestor@123456"
}
```

Use o token no Swagger e envie:

```json
{
  "titulo": "Natal Solidário",
  "descricao": "Campanha para arrecadação de recursos para crianças em situação de vulnerabilidade.",
  "dataInicio": "2026-07-15T00:00:00Z",
  "dataFim": "2026-12-31T23:59:59Z",
  "metaFinanceira": 50000,
  "status": "Ativa"
}
```

## Resultado esperado no painel público

```json
[
  {
    "id": "GUID_DA_CAMPANHA",
    "titulo": "Natal Solidário",
    "metaFinanceira": 50000,
    "valorTotalArrecadado": 0
  }
]
```
