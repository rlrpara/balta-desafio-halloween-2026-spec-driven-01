<img width="100%" alt="Halloween 2026" src="https://baltaio.blob.core.windows.net/static/images/v4/challenges/halloween-2026/banner.jpg" />

## 🎃 Halloween - Desafio 1

Autor: **Rodrigo Ribeiro**

Este repositório é a minha implementação do desafio **Halloween 2026**, do [balta.io](https://balta.io). 👻

### Sobre este desafio
Este desafio consolida Spec-Driven Development com uma API de geração e consulta de senhas.

#### O que foi implementado
- Uma API pequena com dois endpoints: um gera a senha e outro consulta pelo GUID.
- Senhas com pelo menos 16 caracteres, sem espaços e com caractere especial.
- Persistência local em SQLite usando EF Core e migration aplicada na inicialização.
- Separação entre Domain, Service, Infra.Data, Infra.Database e Api.
- Testes unitários para as regras do Service e testes de integração para o contrato HTTP.

Neste processo eu aprendi:
- ✅ A especificação fica mais útil quando fala do comportamento, sem prender a implementação.
- ✅ O Service fica mais fácil de testar quando o acesso ao banco passa por uma interface.
- ✅ Migration e teste de integração ajudam a perceber cedo quando a persistência não está realmente funcionando.

## Badge
<img src="https://baltaio.blob.core.windows.net/static/images/v4/challenges/halloween-2026/01.png" width="200" />

## Problema

O desafio pedia uma API capaz de gerar senhas fortes, guardar cada senha e permitir uma consulta posterior usando um GUID. A solução construída gera uma senha aleatória com as regras combinadas, salva o registro em SQLite e expõe os endpoints `POST /passwords` e `GET /passwords/{id}`. Também tratei os dois erros esperados: GUID inválido retorna `400` com ProblemDetails e GUID válido sem registro retorna `404`.

## Execução

Requer .NET SDK 10.0 ou superior.

```bash
dotnet restore src/PasswordGenerator.slnx
dotnet build src/PasswordGenerator.slnx --configuration Release
dotnet test src/PasswordGenerator.slnx --configuration Release --no-build
dotnet run --project src/PasswordGenerator.Api
```

Por padrão, a aplicação cria `passwords.db` no diretório de execução e aplica a migration automaticamente. No container, o arquivo fica em `/app/data/passwords.db`, em um diretório gravável pelo usuário da aplicação.

### Cobertura dos testes unitários

O relatório de cobertura unitária pode ser gerado separadamente dos testes de integração:

```bash
dotnet test tests/PasswordGenerator.UnitTests/PasswordGenerator.UnitTests.csproj --configuration Release
```

O projeto unitário coleta Cobertura automaticamente e falha se a cobertura de linhas ficar abaixo de 80%. O relatório fica em `tests/PasswordGenerator.UnitTests/TestResults/UnitCoverage`; os testes de integração não entram nesse percentual.

### Docker

Não é necessário subir um banco separado: o SQLite fica no mesmo container da API. A imagem usa bases fixadas por digest e executa a aplicação sem root.

```bash
docker build -t password-generator-api .
docker run --rm --name password-generator-api -p 8080:8080 password-generator-api
```

Com o container em execução, use `POST http://localhost:8080/passwords` e depois `GET http://localhost:8080/passwords/{id}`.

## Endpoints

### Gerar senha

`POST /passwords` sem corpo retorna `201 Created`:

```json
{
  "id": "00000000-0000-0000-0000-000000000000",
  "password": "Abcdefghijkmno!2"
}
```

### Consultar senha

`GET /passwords/{id}` retorna `200` quando o GUID existe, `400` com ProblemDetails quando o valor não é um GUID e `404` quando não há registro.

## Aviso educacional

Este projeto armazena e retorna senhas em texto puro deliberadamente para fins didáticos. Não use esta aplicação em produção.

## Sobre o Halloween 2026
O desafio **Halloween 2026** consiste em praticar Spec-Driven Development de ponta a ponta, criando uma aplicação completa com IA.

### Veja meu progresso no desafio
O progresso está documentado neste repositório e nos quatro artefatos em `specs/`.

## Evidência SonarQube

Status: `NOT_PROVEN/BLOCKED`.

O dashboard local (`http://localhost:9001/dashboard?id=balta-desafio-halloween-2026-spec-driven-01`) respondeu, mas redirecionou a sessão Playwright existente para `/sessions/new` e exibiu o formulário de login. Não havia sessão autenticada disponível. Nenhum token, credencial ou cookie foi usado, registrado ou armazenado; por isso, nenhum PNG foi produzido.
