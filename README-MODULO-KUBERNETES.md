# Módulo 4 — Kubernetes

Este módulo adiciona os manifests Kubernetes para executar a solução Conexão Solidária em cluster local.

## Componentes criados

- Namespace `conexao-solidaria`;
- ConfigMap da aplicação;
- Secret com senhas e chave JWT;
- Deployment e Service do PostgreSQL;
- Deployment e Service do MongoDB;
- Deployment e Service do RabbitMQ;
- Deployment e Service da API;
- Deployment do Worker;
- PVCs para PostgreSQL, MongoDB e RabbitMQ;
- Probes de liveness/readiness;
- `kustomization.yaml`;
- scripts auxiliares.

## Pré-requisitos

- Docker Desktop com Kubernetes habilitado, ou Minikube/Kind;
- `kubectl`;
- Docker.

## 1. Aplicar o módulo

Na raiz do projeto, execute:

```bash
unzip -o ~/Downloads/conexao-solidaria-modulo-kubernetes.zip -d .
```

## 2. Gerar imagens locais

```bash
chmod +x scripts/k8s-*.sh
./scripts/k8s-build-images.sh
```

As imagens geradas serão:

```text
conexao-solidaria-api:local
conexao-solidaria-worker:local
```

## 3. Subir no Kubernetes

```bash
./scripts/k8s-deploy.sh
```

Ou manualmente:

```bash
kubectl apply -k deploy/kubernetes
kubectl get pods -n conexao-solidaria
```

## 4. Validar pods

```bash
kubectl get pods -n conexao-solidaria
```

Resultado esperado:

```text
postgres                         Running
mongo                            Running
rabbitmq                         Running
conexao-solidaria-api             Running
conexao-solidaria-worker          Running
```

## 5. Acessar a API

Em um terminal:

```bash
./scripts/k8s-port-forward-api.sh
```

Depois acesse:

```text
http://localhost:8080/swagger
http://localhost:8080/health
```

## 6. Acessar RabbitMQ

Em outro terminal:

```bash
./scripts/k8s-port-forward-rabbitmq.sh
```

Depois acesse:

```text
http://localhost:15672
```

Credenciais:

```text
guest
guest
```

## 7. Testar o fluxo

Siga a mesma sequência usada no Docker Compose:

1. Login como gestor;
2. Criar campanha;
3. Cadastrar doador;
4. Login como doador;
5. Enviar doação;
6. Consultar painel de transparência.

## 8. Logs

API:

```bash
kubectl logs -f deployment/conexao-solidaria-api -n conexao-solidaria
```

Worker:

```bash
kubectl logs -f deployment/conexao-solidaria-worker -n conexao-solidaria
```

RabbitMQ:

```bash
kubectl logs -f deployment/rabbitmq -n conexao-solidaria
```

## 9. Verificar filas no RabbitMQ pelo Kubernetes

```bash
kubectl exec -it deployment/rabbitmq -n conexao-solidaria -- rabbitmqctl list_queues name messages_ready messages_unacknowledged consumers
```

## 10. Apagar ambiente Kubernetes

```bash
./scripts/k8s-delete.sh
```

ou:

```bash
kubectl delete namespace conexao-solidaria
```

## Observação importante

Os manifests usam:

```yaml
imagePullPolicy: Never
```

Isso foi feito para usar imagens locais em cluster local. Para rodar em um cluster remoto, altere para uma imagem publicada em registry, por exemplo:

```yaml
image: ghcr.io/seu-usuario/conexao-solidaria-api:latest
imagePullPolicy: Always
```
