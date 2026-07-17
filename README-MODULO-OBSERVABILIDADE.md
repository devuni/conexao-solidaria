# Módulo 5 — Observabilidade

Este projeto completo já contém:

- API com `/metrics`;
- Prometheus;
- Grafana com dashboard provisionado;
- Zabbix Appliance;
- Kubernetes manifests completos;
- Autenticação, campanhas e doações assíncronas.

## Teste rápido

```bash
dotnet clean ConexaoSolidaria.sln
dotnet restore ConexaoSolidaria.sln
dotnet build ConexaoSolidaria.sln
dotnet test ConexaoSolidaria.sln
```

## Kubernetes

```bash
kubectl config use-context docker-desktop
./scripts/k8s-build-images.sh
kubectl apply -k deploy/kubernetes
kubectl get pods -n conexao-solidaria
```

## Acessos

API:

```bash
./scripts/k8s-port-forward-api.sh
```

```text
http://localhost:8080/swagger
http://localhost:8080/metrics
```

Prometheus:

```bash
./scripts/k8s-port-forward-prometheus.sh
```

```text
http://localhost:9090
```

Grafana:

```bash
./scripts/k8s-port-forward-grafana.sh
```

```text
http://localhost:3000
admin / admin
```

Zabbix:

```bash
./scripts/k8s-port-forward-zabbix.sh
```

```text
http://localhost:8081
Admin / zabbix
```
