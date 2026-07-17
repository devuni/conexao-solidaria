# Módulo 6 — CI/CD com GitHub Actions

Este módulo adiciona uma pipeline GitHub Actions para o projeto Conexão Solidária.

## O que a pipeline faz

Ao executar em `main` ou em pull request para `main`, a pipeline faz:

1. Checkout do código;
2. Instalação do .NET 8;
3. `dotnet restore`;
4. `dotnet build`;
5. `dotnet test`;
6. Build da imagem Docker da API;
7. Build da imagem Docker do Worker;
8. Publicação opcional das imagens no GitHub Container Registry em push para `main`.

## Arquivos criados

```text
.github/workflows/ci-cd.yml
.dockerignore
README-MODULO-CICD.md
```

## Aplicar no projeto

Na raiz do projeto:

```bash
unzip -o ~/Downloads/conexao-solidaria-modulo-cicd.zip -d .
```

## Validar localmente antes do push

```bash
dotnet restore ConexaoSolidaria.sln
dotnet build ConexaoSolidaria.sln --configuration Release
dotnet test ConexaoSolidaria.sln --configuration Release
```

Também valide as imagens:

```bash
docker build -f src/ConexaoSolidaria.Api/Dockerfile -t conexao-solidaria-api:local .
docker build -f src/ConexaoSolidaria.DonationWorker/Dockerfile -t conexao-solidaria-worker:local .
docker images | grep conexao-solidaria
```

## Enviar para o GitHub

```bash
git add .
git commit -m "ci: adicionar pipeline de build, testes e docker"
git push origin main
```

## Evidência para o vídeo

Na entrega, mostre no GitHub:

1. Aba `Actions`;
2. Workflow `Conexão Solidária - CI/CD`;
3. Job `Build e testes .NET` executado com sucesso;
4. Job `Gerar imagens Docker` executado com sucesso;
5. Logs mostrando `dotnet build`, `dotnet test` e build das imagens Docker.

## Observação sobre GHCR

O job `docker-build-ghcr` publica imagens no GitHub Container Registry usando `GITHUB_TOKEN`.

Para funcionar corretamente, confirme no GitHub:

```text
Settings > Actions > General > Workflow permissions
```

Marque:

```text
Read and write permissions
```

Se não quiser publicar imagens, basta manter a evidência do job `Gerar imagens Docker`, pois ele já cumpre o requisito de geração de imagem Docker.
