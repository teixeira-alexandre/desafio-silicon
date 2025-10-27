# Database (MySQL)
```bash 
docker compose up -d
```

Pro arquivo de docker compose funcionar, é preciso que  algumas variáveis estejam setadas a nível de usuário.

Crie um arquivo .env na raiz do projeto com as seguintes variáveis:

```bash

MYSQL_ROOT_PASSWORD=sv_root
MYSQL_DATABASE=sv_banco
MYSQL_USER=sv_user
MYSQL_PASSWORD=sv_senha

```

- Adminer: `http://localhost:8080` (server: `mysql`, user: `sv_user`, pass: `sv_senha`, db: `sv_banco`)


# Backend (.NET 8 + MySQL)

## Rodar
```bash
dotnet restore
dotnet run
```
Swagger: `http://localhost:5000/swagger`

Se precisar trocar a porta do MySQL, ajuste `appsettings.json`.

## Dica
Se aparecer erro de build, limpe antes:
```bash
dotnet clean ProjetoBanco.Api.csproj
rm -rf bin obj
dotnet restore ProjetoBanco.Api.csproj
dotnet run --project ProjetoBanco.Api.csproj
```

# Frontend (React + VITE)

```bash 
npm install
npm run dev
```
Por padrão a URL do front é http://localhost/5173/ 
