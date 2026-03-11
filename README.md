# Task Management API

API REST para gestao de tarefas, desenvolvida em .NET 8 com arquitetura em camadas (Api, Application, Domain e Infrastructure).

## Objetivo

Permitir que usuarios criem, consultem, atualizem, listem e removam tarefas.

## Stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core InMemory
- Swagger / OpenAPI
- Serilog
- xUnit + Moq

## Estrutura da Solucao

```text
taskmanagement
|- src/
|  |- TaskManagement.Api/            # Camada de entrada HTTP
|  |- TaskManagement.Application/    # Regras de negocio e validacoes
|  |- TaskManagement.Domain/         # Entidades, contratos e Result pattern
|  |- TaskManagement.Infrastructure/ # Persistencia (EF Core InMemory)
|- test/
|  |- TaskManagement.Api.UnitTest/   # Testes unitarios
```

## Arquitetura

- Api: controllers, pipeline, Swagger e logging.
- Application: servicos (`TaskService`), validadores (`TaskValidator`) e mapeamentos.
- Domain: entidades (`TaskEntity`), contratos (`ITaskRepository`) e objetos de resultado (`Result`, `Result<T>`, `Error`).
- Infrastructure: repositorio (`TaskRepository`) e contexto (`TaskDbContext`) com banco em memoria.

## Pre-requisitos

- SDK do .NET 8 instalado
- (Opcional) Visual Studio 2022 ou VS Code

Verificar instalacao:

```bash
dotnet --version
```

## Como executar

Na raiz do repositorio:

```bash
dotnet restore
dotnet build
dotnet run --project src/TaskManagement.Api/TaskManagement.Api.csproj
```

Por padrao em ambiente Development, os perfis de execucao expõem:

- HTTP: `http://localhost:5043`
- HTTPS: `https://localhost:7171`

Swagger:

- `https://localhost:7171/swagger`
- `http://localhost:5043/swagger`

## Persistencia

O projeto usa `UseInMemoryDatabase("TaskDB")`.

- Os dados sao mantidos apenas em memoria durante a execucao da aplicacao.
- Ao reiniciar a API, os dados sao perdidos.

## Endpoints

Base route: `api/Task`

1. Criar tarefa
- Metodo: `POST`
- Rota: `/api/Task/Create`

Exemplo de request:

```json
{
	"titulo": "Preparar apresentacao",
	"descricao": "Finalizar slides da reuniao semanal",
	"dataVencimento": "2026-03-20T18:00:00",
	"status": "Pendente"
}
```

2. Obter tarefa por id
- Metodo: `GET`
- Rota: `/api/Task/{id}`

3. Atualizar tarefa
- Metodo: `PUT`
- Rota: `/api/Task/{id}`

4. Remover tarefa
- Metodo: `DELETE`
- Rota: `/api/Task/{id}`

5. Listar tarefas com filtro e paginacao
- Metodo: `GET`
- Rota: `/api/Task/List`
- Query params opcionais:
	- `status` (string)
	- `dataVencimento` (DateTime)
	- `page` (int, padrao = 1)
	- `pageSize` (int, padrao = 10)

Exemplo:

```text
GET /api/Task/List?status=Pendente&dataVencimento=2026-03-31&page=1&pageSize=10
```

## Contrato de resposta

A API utiliza um envelope padrao:

```json
{
	"isSuccess": true,
	"error": {
		"message": "",
		"type": "None"
	},
	"data": {
		"id": 1,
		"titulo": "Preparar apresentacao",
		"descricao": "Finalizar slides da reuniao semanal",
		"dataVencimento": "2026-03-20T18:00:00",
		"status": "Pendente"
	}
}
```

Quando `isSuccess` for `false`, `data` nao e retornado e `error` contem o detalhe.

## Regras de validacao

- `titulo` e obrigatorio.
- `status` precisa ser um valor valido do enum:
	- `Pendente`
	- `EmProgresso`
	- `Concluida`

## Codigos HTTP (mapeados por tipo de erro)

- `200 OK`: sucesso geral
- `201 Created`: criacao bem-sucedida
- `204 No Content`: remocao bem-sucedida
- `400 Bad Request`: erro de validacao
- `404 Not Found`: recurso nao encontrado
- `500 Internal Server Error`: erro inesperado

## Logging

<!-- O projeto usa Serilog com saida em console e nivel minimo `Information`. -->

<!-- ## Testes -->

Executar todos os testes:

```bash
dotnet test
```

Executar apenas o projeto de testes:

```bash
dotnet test test/TaskManagement.Api.UnitTest/TaskManagement.Api.UnitTest.csproj
```