# Tarefas

| ID | Tarefa | Origem | Depende de | Concluída quando |
|---|---|---|---|---|
| T-01 | Criar solução e projetos Domain, Service, Api, Infra.Data e Infra.Database | Constituição, plan Arquitetura | | Projetos referenciados e build inicial |
| T-02 | Modelar entidade e contrato de repositório | RF-03, RF-04, plan Modelo de dados | T-01 | Entidade e contrato compilam |
| T-03 | Implementar geração da senha e casos de uso | RN-01, RN-02, RN-03, RF-01, RF-02 | T-02 | Testes unitários cobrem formato e persistência via contrato |
| T-04 | Implementar DbContext, mapeamento e repositório EF Core | RF-03, plan Arquitetura | T-02 | Repositório grava e consulta registro |
| T-05 | Configurar SQLite e migration inicial | RF-03, plan Decisões | T-04 | Migration cria tabela e é aplicada no startup |
| T-06 | Expor POST e GET com contratos HTTP | RF-01, RF-02, RN-04, RN-05 | T-03, T-05 | Endpoints retornam status e payload definidos |
| T-07 | Criar testes unitários com enforcement de cobertura e testes de integração com WebApplicationFactory | Critérios de aceite, constituição Qualidade | T-06 | Regras unitárias cobertas, limite de 80% aplicado e POST, GET, 400 e 404 cobertos |
| T-08 | Atualizar README e revisar rastreabilidade | Governança | T-07 | Execução, testes, endpoints e aviso educacional documentados |
