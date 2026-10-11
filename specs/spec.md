# Gerador de Senhas Fortes

## Problema
Usuários precisam gerar uma senha aleatória para fins de estudo e recuperá-la posteriormente por um identificador único.

## Objetivo
Disponibilizar uma interface HTTP pequena que gere, armazene e consulte senhas válidas, deixando explícito o comportamento esperado sem prescrever tecnologia.

## Usuários
Desenvolvedores e estudantes que precisam exercitar desenvolvimento orientado a especificação.

## Histórias
- Como usuário, quero solicitar uma senha forte e receber um identificador para consultá-la depois.
- Como usuário, quero consultar uma senha pelo identificador recebido.

## Requisitos funcionais
- RF-01: `POST /passwords` deve gerar uma senha, armazená-la e retornar seu identificador e valor.
- RF-02: `GET /passwords/{id}` deve retornar a senha associada ao identificador.
- RF-03: A senha gerada deve ser persistida antes da resposta de criação.
- RF-04: O identificador deve ser único.

## Regras de negócio
- RN-01: Toda senha deve possuir no mínimo 16 caracteres.
- RN-02: Toda senha deve conter ao menos um caractere especial.
- RN-03: Toda senha deve ser livre de espaços em branco.
- RN-04: Um identificador malformado é erro de requisição.
- RN-05: Um identificador válido sem registro corresponde a recurso inexistente.

## Casos de borda
- Identificador que não representa um GUID: retornar `400` com ProblemDetails.
- GUID válido sem senha associada: retornar `404`.
- A criação deve continuar válida sem corpo de requisição.

## Fora de escopo
- Autenticação e autorização.
- Criptografia ou armazenamento seguro de senhas.
- Expiração, exclusão, edição ou listagem de senhas.
- Frontend e qualquer recurso além dos dois endpoints.

## Critérios de aceite
- Uma criação retorna `201` com GUID e senha persistida.
- Consulta do GUID retornado retorna `200` com os mesmos dados.
- As regras RN-01 a RN-03 são cobertas por testes unitários.
- Os casos de borda retornam os status definidos.
- O comportamento não depende de tecnologia específica nesta especificação.
