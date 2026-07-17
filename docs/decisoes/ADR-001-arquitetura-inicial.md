# ADR-001 — Arquitetura inicial

## Status

Aceita.

## Contexto

A plataforma Conexão Solidária precisa oferecer autenticação, gestão de campanhas,
consulta pública e processamento assíncrono de doações. A solução também precisa
ser executada em containers, Kubernetes e possuir observabilidade.

## Decisão

A solução será composta inicialmente por:

- `ConexaoSolidaria.Api`: API HTTP responsável por autenticação, usuários,
  campanhas e recebimento de intenções de doação.
- `ConexaoSolidaria.DonationWorker`: consumidor assíncrono responsável por
  processar eventos de doação.
- PostgreSQL: armazenamento transacional.
- MongoDB: histórico e auditoria de doações processadas.
- RabbitMQ: broker de mensageria.
- Grafana, Zabbix e métricas: observabilidade, adicionados em etapa posterior.

## Consequências

### Positivas

- Separação clara entre recebimento e processamento de doações.
- Escalabilidade independente da API e do Worker.
- Demonstração objetiva do uso de comunicação assíncrona.
- Estrutura compatível com Docker e Kubernetes.

### Negativas

- Maior quantidade de componentes de infraestrutura.
- Necessidade de tratar idempotência e consistência eventual.
