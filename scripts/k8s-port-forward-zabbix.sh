#!/usr/bin/env bash
set -euo pipefail

kubectl port-forward svc/zabbix-web 8081:8080 -n conexao-solidaria
