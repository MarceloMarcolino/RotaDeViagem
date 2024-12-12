# Documentação da Aplicação: RotaDeViagem

## 1. Como Executar a Aplicação

### Pré-requisitos
- **.NET SDK** versão 9.0 ou superior instalado em seu computador.
- **Editor de código** recomendado: [Visual Studio](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/).

### Passos para Executar a Aplicação

1. **Baixar e extrair o projeto**:
2. Restaurar as dependências do projeto: Execute o seguinte comando na raiz do projeto para restaurar as dependências:
	dotnet restore
3. Executar a aplicação Console: Para rodar a aplicação de console, utilize o seguinte comando:
	cd RotaDeViagem.ConsoleApp\bin\Release\net9.0\win-x64\publish
	RotaDeViagem.ConsoleApp.exe RotaDeViagem.ConsoleApp\Input\rotas.csv
4. Executar os testes: Para rodar os testes unitários, execute:
	dotnet test --project RotaDeViagem.Tests
5. Executar a API: Para rodar a API, utilize:
	dotnet run --project RotaDeViagem.Api
	A aplicação estará disponível localmente em http://localhost:5000 para a API.
	
## 2. Estrutura dos Arquivos/Pacotes
A estrutura do projeto foi organizada de forma modular, com diferentes componentes e responsabilidades. Veja a estrutura detalhada abaixo:
└── 📁RotaDeViagem.Api                 # API REST da aplicação
    └── 📁bin                          # Arquivos compilados da API
    └── 📁Controllers                  # Controladores da API
    └── 📁obj                         # Arquivos temporários de compilação
    └── 📁Properties                   # Arquivos de configuração
    └── rotas.csv                      # Arquivo de dados
    └── Program.cs                     # Arquivo principal da API
    └── RotaDeViagem.Api.csproj         # Arquivo do projeto da API

└── 📁RotaDeViagem.ConsoleApp           # Aplicação de Console
    └── 📁bin                          # Arquivos compilados do ConsoleApp
    └── 📁Input                        # Arquivo de entrada (rotas.csv)
    └── 📁obj                         # Arquivos temporários de compilação
    └── Program.cs                     # Arquivo principal do ConsoleApp
    └── RotaDeViagem.ConsoleApp.csproj  # Arquivo do projeto do ConsoleApp

└── 📁RotaDeViagem.Core                 # Camada central de serviços e modelos
    └── 📁bin                          # Arquivos compilados do Core
    └── 📁Models                       # Modelos de dados (ex: Route.cs)
    └── 📁Services                     # Lógica de negócios (ex: RouteService.cs)
    └── Class1.cs                      # Classe auxiliar em desuso
    └── RotaDeViagem.Core.csproj        # Arquivo do projeto Core

└── 📁RotaDeViagem.Tests                # Projeto de testes unitários
    └── 📁bin                          # Arquivos compilados dos testes
    └── 📁obj                          # Arquivos temporários dos testes
    └── 📁Services                     # Testes para os serviços
    └── rotas_teste.csv                 # Arquivo de dados para os testes
    └── RotaDeViagem.Tests.csproj       # Arquivo do projeto dos testes
    └── UnitTest1.cs                    # Teste de exemplo
    └── RouteServiceTests.cs            # Testes para o RouteService

## 3. Decisões de Design Adotadas
1. Modularização
A aplicação foi dividida em três principais módulos:

API: Responsável pela exposição das funcionalidades da aplicação através de endpoints RESTful.
ConsoleApp: Permite que o usuário interaja com a aplicação via linha de comando, utilizando os mesmos serviços da API.
Core: Contém a lógica de negócios e os modelos da aplicação, como a definição das rotas.

2. Arquivos de Configuração
Arquivos como rotas.csv e rotas_teste.csv são utilizados para armazenar os dados das rotas. Esses arquivos são lidos e manipulados pela aplicação para buscar e adicionar rotas.
A configuração de execução da API é realizada pelo arquivo Program.cs, enquanto o RotaDeViagem.Api.csproj contém a configuração do projeto.

3. Testes Unitários
A aplicação adota xUnit como framework de testes, que são realizados no projeto RotaDeViagem.Tests. A classe RouteServiceTests.cs contém os testes para a lógica de negócios, como a adição e busca de rotas.

4. Uso de Camada de Serviço
Foi adotada a camada de serviços para centralizar a lógica de negócios, separando as responsabilidades de manipulação de dados (no Core) e a exposição de funcionalidades (na API e ConsoleApp).

5. Arquitetura Simples
A solução adota uma arquitetura simples de três camadas (API, ConsoleApp e Core) para facilitar a manutenção e testes da aplicação. A API e o ConsoleApp utilizam os mesmos serviços definidos no Core, o que garante a consistência da lógica de negócios entre as diferentes interfaces.

4. API REST - Descrição Simplificada
Base URL
A API está disponível em http://localhost:5000.

Endpoints
GET /routes
Retorna todas as rotas cadastradas no sistema.

Resposta: Lista de rotas (origem, destino, distância, preço).
GET /routes/{origin}/{destination}
Retorna a rota mais barata entre a origem e o destino.

Parâmetros: origin e destination são obrigatórios.
Resposta: A rota mais barata entre as duas cidades.
POST /routes
Adiciona uma nova rota ao sistema.

Corpo: JSON contendo os dados da rota (origem, destino, distância, preço).
Resposta: Mensagem de sucesso ou erro.
Exemplo de Requisição
Adicionar uma nova rota:
	POST /routes
	{
	  "origin": "GRU",
	  "destination": "CDG",
	  "cost": 400
	}
Resposta:
	{
	  "message": "Rota adicionada com sucesso."
	}
Segurança
A API não possui autenticação implementada, mas pode ser expandida para incluir autenticação com tokens (JWT) ou API keys, conforme necessário.

Essa documentação oferece uma visão geral da aplicação RotaDeViagem, incluindo como executar a aplicação, a estrutura de arquivos e pacotes, as decisões de design e uma descrição simplificada da API REST implementada.
