# Plano técnico

## Contexto
Implementar a API mínima do desafio preservando a separação Service, Infra.Data e Infra.Database observada nas referências locais. O armazenamento deve ser real e local para validar o ciclo de criação e consulta.

## Arquitetura
`Api` compõe dependências e traduz resultados para HTTP. `Service` executa os casos de uso e gera a senha. `Domain` define entidade e contrato do repositório. `Infra.Data` usa EF Core para mapeamento, contexto e persistência. `Infra.Database` registra SQLite e aplica migrations. Testes unitários isolam o repositório; testes de integração hospedam a API e usam SQLite em memória.

## Decisões
- .NET 10 e Minimal API: atendem à stack do desafio e mantêm o host pequeno.
- EF Core: fornece mapeamento e persistência tipada, sem acesso SQL no endpoint.
- SQLite: atende à persistência local exigida sem serviço externo.
- Migration explícita: garante que o schema seja versionado e aplicado no startup.
- `RandomNumberGenerator`: gera valores aleatórios adequados ao objetivo educacional sem prometer armazenamento seguro.
- WebApplicationFactory: valida o contrato HTTP real e a composição de DI.
- `ProblemDetails`: padroniza o erro de GUID inválido como solicitado.
- `coverlet.msbuild`: coleta somente a execução unitária e aplica limite de 80% de linhas; integração não entra nessa métrica.

## Modelo de dados
Tabela `passwords`: `Id` GUID como chave primária e `Password` texto obrigatório. Não há timestamps, usuário, expiração ou outros campos fora do escopo.

## Contratos
- `POST /passwords` sem corpo: `201 Created`, JSON `{ id, password }`.
- `GET /passwords/{id}`: `200 OK` com `{ id, password }`, `400` ProblemDetails para GUID inválido e `404` para GUID inexistente.

## Riscos
- A senha é retornada e armazenada em texto puro por decisão explícita do desafio; não usar em produção.
- SQLite em arquivo pode exigir permissões de escrita no diretório de execução.
- A migration é aplicada no startup; falha de schema impede a aplicação de iniciar, evitando execução parcialmente configurada.
