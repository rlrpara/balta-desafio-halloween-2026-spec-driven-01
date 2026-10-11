# Constituição do projeto

## Stack

- A aplicação deve usar .NET 10 e C#.
- A API deve expor endpoints HTTP com Minimal API.
- A persistência deve usar EF Core com SQLite.
- Os testes devem usar xUnit e, para HTTP, WebApplicationFactory quando compatível.

## Arquitetura

- `PasswordGenerator.Api` contém somente composição, contratos HTTP e mapeamento de respostas.
- `PasswordGenerator.Service` contém casos de uso e regras de geração/consulta.
- `PasswordGenerator.Domain` contém entidades e contratos de persistência.
- `PasswordGenerator.Infra.Data` contém DbContext, mapeamento EF Core e repositório.
- `PasswordGenerator.Infra.Database` contém configuração de banco e migrations.
- Endpoints não podem acessar DbContext ou repositórios diretamente.

## Qualidade

- Toda regra de senha deve ter teste unitário.
- A cobertura de linhas dos testes unitários deve ser no mínimo 80%, com a validação configurada para falhar abaixo do limite.
- Cada endpoint deve ter teste de integração de caminho feliz.
- GUID inválido deve retornar `400` com ProblemDetails e GUID inexistente deve retornar `404`.
- Build e testes devem ser executáveis localmente sem serviços externos.

## Convenções

- Código em inglês, documentação do desafio em português.
- Nullable reference types e implicit usings habilitados.
- Operações de I/O devem ser assíncronas e aceitar cancelamento quando aplicável.
- Não adicionar autenticação, criptografia, expiração, exclusão ou frontend.

## Governança

- Toda alteração deve ser rastreável a `spec.md`, `plan.md` e `tasks.md`.
- Dependências novas exigem justificativa no plano técnico.
- A senha é deliberadamente educacional e não deve ser usada em produção.
