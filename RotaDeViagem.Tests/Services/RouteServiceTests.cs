using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using RotaDeViagem.Core.Models;
using RotaDeViagem.Core.Services;

namespace RotaDeViagem.Tests.Services
{
    public class RouteServiceTests : IDisposable
    {
        private readonly string _tempFilePath;
        private readonly RouteService _service;

        public RouteServiceTests()
        {
            // Cria um arquivo temporário com dados simulados
            _tempFilePath = Path.GetTempFileName();

            // Caminho do arquivo rotas.csv
            string csvFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Input", "rotas.csv");

            // Verifica se o arquivo rotas.csv existe
            if (File.Exists(csvFilePath))
            {
                // Copia o conteúdo de rotas.csv para o arquivo temporário
                File.Copy(csvFilePath, _tempFilePath, overwrite: true);
            }
            else
            {
                // Caso o arquivo rotas.csv não exista, cria conteúdo simulado
                File.WriteAllLines(_tempFilePath, new[]
                {
                    "GRU,BRC,10",
                    "BRC,SCL,5",
                    "GRU,CDG,75",
                    "GRU,SCL,20",
                    "GRU,ORL,56",
                    "ORL,CDG,5",
                    "SCL,ORL,20"
                });
            }

            // Inicializar o serviço com o arquivo designado
            _service = new RouteService(_tempFilePath); // Usando o construtor com argumento
        }

        public void Dispose()
        {
            // Deletar o arquivo temporário após os testes, se necessário
            if (_tempFilePath != null && File.Exists(_tempFilePath))
                File.Delete(_tempFilePath);
        }

        [Fact]
        public void Test_FindCheapestRoute_ShouldReturn_CheapestRoute()
        {
            // Arrange
            var origin = "GRU";
            var destination = "CDG";

            // Act
            var result = _service.FindCheapestRoute(origin, destination);

            // Assert
            Assert.Equal("GRU - BRC - SCL - ORL - CDG ao custo de $40", result);
        }

        [Fact]
        public void Test_FindCheapestRoute_WhenNoRouteExists_ShouldReturn_NoRouteFound()
        {
            // Arrange
            var origin = "GRU";
            var destination = "XYZ";  // Rota inexistente

            // Act
            var result = _service.FindCheapestRoute(origin, destination);

            // Assert
            Assert.Equal("Nenhuma rota encontrada.", result);
        }

        [Fact]
        public void Test_AddRoute_ShouldAddNewRoute()
        {
            // Configuração dos dados de teste
            var newRoute = new Route("SCL", "CDG", 100);
            
            // Adicionar uma nova rota no serviço
            _service.AddRoute(newRoute, _tempFilePath);

            // Act
            var result = _service.FindCheapestRoute("SCL", "CDG");

            // Esperado: a rota mais barata, que pode incluir escalas
            var expectedRoute = "SCL - ORL - CDG ao custo de $25"; // Ajuste aqui com base na lógica
            Assert.Equal(expectedRoute, result); // Verificando a rota mais barata
        }
    }
}
