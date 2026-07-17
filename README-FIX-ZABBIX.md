# Correção do Zabbix no Kubernetes

Este patch troca o `zabbix/zabbix-appliance` por uma instalação componentizada:

- `zabbix-postgres`
- `zabbix-server`
- `zabbix-web`

## Aplicar

```bash
unzip -o ~/Downloads/conexao-solidaria-fix-zabbix.zip -d .
chmod +x scripts/k8s-port-forward-zabbix.sh
```

## Remover o deployment antigo quebrado

```bash
kubectl delete deployment zabbix -n conexao-solidaria --ignore-not-found=true
kubectl delete svc zabbix -n conexao-solidaria --ignore-not-found=true
```

## Aplicar novamente

```bash
kubectl apply -k deploy/kubernetes
kubectl get pods -n conexao-solidaria
```

## Acessar Zabbix

```bash
./scripts/k8s-port-forward-zabbix.sh
```

Depois acesse:

```text
http://localhost:8081
```

Credenciais padrão:

```text
Admin
zabbix
```
