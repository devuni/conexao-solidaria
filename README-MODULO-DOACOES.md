# Módulo 4 — Doação assíncrona com RabbitMQ e Worker

Este módulo implementa o fluxo central exigido no trabalho:

```text
Doador autenticado
    ↓
POST /api/v1/doacoes
    ↓
API registra intenção de doação
    ↓
API publica DoacaoRecebidaEvent no RabbitMQ
    ↓
DonationWorker consome a mensagem
    ↓
Worker atualiza ValorTotalArrecadado da campanha
    ↓
Worker registra auditoria no MongoDB
```

## Aplicar o módulo

Na raiz do projeto:

```bash
unzip -o ~/Downloads/conexao-solidaria-modulo-doacoes.zip -d .
```

Depois:

```bash
dotnet clean ConexaoSolidaria.sln
dotnet restore ConexaoSolidaria.sln
dotnet build ConexaoSolidaria.sln
dotnet test ConexaoSolidaria.sln
```

## Recriar banco e containers

Como o módulo cria as tabelas `doacoes` e `eventos_processados`, recrie os volumes:

```bash
docker compose down -v
docker compose up --build
```

## Endpoint novo

```text
POST /api/v1/doacoes
```

Acesso: somente `Doador` autenticado.

Payload:

```json
{
  "idCampanha": "COLE_O_GUID_DA_CAMPANHA",
  "valorDoacao": 150.00
}
```

Resposta esperada:

```json
{
  "idDoacao": "GUID_DA_DOACAO",
  "status": "RecebidaParaProcessamento",
  "mensagem": "Doação recebida e enviada para processamento assíncrono."
}
```

HTTP esperado: `202 Accepted`.

## Como demonstrar no vídeo

1. Fazer login como gestor.
2. Criar uma campanha ativa.
3. Fazer login como doador.
4. Enviar uma doação para a campanha.
5. Abrir RabbitMQ em `http://localhost:15672`.
6. Mostrar a fila `doacao-recebida`.
7. Ver os logs do worker:

```bash
docker compose logs -f donation-worker
```

8. Consultar o painel público:

```text
GET /api/v1/transparencia/campanhas
```

O valor `valorTotalArrecadado` deverá estar atualizado.

## Evidência arquitetural

Este módulo prova que a API não atualiza diretamente o valor arrecadado. Ela apenas cria a intenção de doação e publica o evento. A atualização da campanha é feita pelo `ConexaoSolidaria.DonationWorker`.
