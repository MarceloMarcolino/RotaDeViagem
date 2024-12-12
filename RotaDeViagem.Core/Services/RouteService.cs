using System.Collections.Generic;
using System.IO;
using System.Linq;
using RotaDeViagem.Core.Models;

namespace RotaDeViagem.Core.Services;

public class RouteService
{
    private readonly List<Route> _routes;

    public RouteService(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("O caminho do arquivo não pode ser nulo ou vazio.");
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"O arquivo de rotas não foi encontrado no caminho: {filePath}");
        }

        // Lê as linhas do arquivo CSV e cria as rotas
        _routes = File.ReadAllLines(filePath)
            .Where(line => !string.IsNullOrWhiteSpace(line)) // Ignora linhas vazias
            .Select(line =>
            {
                var parts = line.Split(',');
                if (parts.Length != 3 || !int.TryParse(parts[2], out int cost) || string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
                {
                    throw new InvalidDataException($"Formato inválido de linha no arquivo: {line}");
                }
                return new Route(parts[0].Trim(), parts[1].Trim(), cost);
            })
            .ToList();
    }

    public string FindCheapestRoute(string origin, string destination)
    {
        var routes = FindAllPaths(origin, destination);

        // Obter a rota mais barata (caso haja alguma)
        var cheapestRoute = routes.OrderBy(route => route.Cost).FirstOrDefault();

        // Verificar se nenhuma rota foi encontrada (tupla com lista de caminhos vazia)
        if (cheapestRoute.Path == null || cheapestRoute.Path.Count == 0)
        {
            return "Nenhuma rota encontrada.";
        }

        // Verificar se existe uma rota direta e se ela é mais barata
        var directRoute = _routes.FirstOrDefault(r => r.Origin == origin && r.Destination == destination);
        if (directRoute != null && (cheapestRoute.Cost >= directRoute.Cost))
        {
            // Se a rota direta for mais barata ou igual, atualizamos a rota mais barata
            cheapestRoute = new (new List<string> { origin, destination }, directRoute.Cost);
        }

        // Formatar a saída corretamente, com " - " como separador
        var routePath = string.Join(" - ", cheapestRoute.Path);

        // Retornar a melhor rota encontrada
        return $"{routePath} ao custo de ${cheapestRoute.Cost}";
    }

    private List<(List<string> Path, int Cost)> FindAllPaths(string origin, string destination)
    {
        var paths = new List<(List<string>, int)>();
        FindPathsRecursive(origin, destination, new List<string>(), 0, paths);
        return paths;
    }

    private void FindPathsRecursive(string current, string destination, List<string> visited, int cost, List<(List<string>, int)> paths)
    {
        if (visited.Contains(current)) return;

        visited.Add(current);

        if (current == destination)
        {
            paths.Add((new List<string>(visited), cost));
        }
        else
        {
            var nextSteps = _routes.Where(r => r.Origin == current);
            foreach (var step in nextSteps)
            {
                FindPathsRecursive(step.Destination, destination, visited, cost + step.Cost, paths);
            }
        }

        visited.RemoveAt(visited.Count - 1);
    }

    public void AddRoute(Route newRoute, string filePath)
    {
        if (_routes.Any(r => r.Origin == newRoute.Origin && r.Destination == newRoute.Destination))
        {
            throw new InvalidOperationException("Essa rota já existe.");
        }
        
        _routes.Add(newRoute);
        File.AppendAllLines(filePath, new[] { $"{newRoute.Origin},{newRoute.Destination},{newRoute.Cost}" });
    }
}
