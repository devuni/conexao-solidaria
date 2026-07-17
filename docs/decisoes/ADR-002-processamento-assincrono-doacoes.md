# ADR-002 — Processamento assíncrono de doações

## Status

Aceita.

## Contexto

O requisito técnico obrigatório determina que, ao receber uma doação, a API não deve atualizar diretamente o valor arrecadado da campanha. A API deve publicar um evento em um broker de mensageria, e um segundo serviço deve consumir a mensagem e atualizar o total arrecadado.

## Decisão

A solução utilizará RabbitMQ com MassTransit.

- A API receberá a intenção de doação em `POST /api/v1/doacoes`.
- A API validará campanha, doador autenticado e valor.
- A API persistirá a intenção de doação com status `Recebida`.
- A API publicará `DoacaoRecebidaEvent` no RabbitMQ.
- O `ConexaoSolidaria.DonationWorker` consumirá a fila `doacao-recebida`.
- O Worker atualizará o `ValorTotalArrecadado` da campanha.
- O Worker marcará a doação como `Processada`.
- O Worker registrará o evento processado para idempotência.
- O Worker registrará auditoria no MongoDB.

## Consequências positivas

- A API permanece rápida, retornando `202 Accepted`.
- O processamento pode ser escalado de forma independente.
- A solução demonstra comunicação assíncrona real.
- O Worker pode usar retry em caso de falha transitória.
- O controle de idempotência reduz risco de duplicidade.

## Consequências negativas

- O sistema passa a trabalhar com consistência eventual.
- O valor arrecadado pode levar alguns instantes para aparecer atualizado.
- Há mais componentes de infraestrutura para monitorar.
