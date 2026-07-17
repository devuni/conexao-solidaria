# Módulo 2 — Usuários, autenticação JWT e RBAC

Este pacote adiciona:

- entidade `Usuario`;
- enum `PerfilUsuario`;
- validação de CPF;
- hash de senha com BCrypt;
- cadastro público de doador;
- login;
- geração de JWT;
- autorização por role;
- seed automático do usuário `GestorONG`;
- persistência inicial no PostgreSQL via EF Core `EnsureCreated`;
- endpoints de validação de usuário logado e roles.

## Aplicar no projeto

Descompacte este pacote na raiz do projeto existente, sobrescrevendo os arquivos quando solicitado.

## Recriar containers e banco

Como este módulo cria tabelas novas no PostgreSQL, execute:

```bash
docker compose down -v
docker compose up --build
```

## Usuário gestor inicial

```text
Email: gestor@conexaosolidaria.org
Senha: Gestor@123456
Role: GestorONG
```

## Endpoints novos

```text
POST /api/v1/doadores
POST /api/v1/auth/login
GET  /api/v1/usuarios/me
GET  /api/v1/usuarios/gestor/check
GET  /api/v1/usuarios/doador/check
```

## Cadastro de doador

```json
{
  "nomeCompleto": "Maria Doadora",
  "email": "maria@email.com",
  "cpf": "52998224725",
  "senha": "Doador@123456"
}
```

## Login do gestor

```json
{
  "email": "gestor@conexaosolidaria.org",
  "senha": "Gestor@123456"
}
```

## Login do doador

```json
{
  "email": "maria@email.com",
  "senha": "Doador@123456"
}
```
