using RotaDeViagem.Core.Models;
using RotaDeViagem.Core.Services;

// Verificar se foi passado um argumento (o caminho do arquivo)
if (args.Length == 0)
{
    Console.WriteLine("Erro: É necessário especificar o arquivo de entrada.");
    Console.WriteLine("Uso: executavel <caminho_do_arquivo.csv>");
    return;
}

string filePath = args[0];

if (!File.Exists(filePath))
{
    Console.WriteLine($"Erro: O arquivo '{filePath}' não foi encontrado.");
    return;
}

try
{
    // Inicializar o serviço com o arquivo especificado
    var service = new RouteService(filePath);

    Console.WriteLine($"Arquivo de rotas carregado com sucesso: {filePath}");

    while (true)
    {
        Console.Write("Digite a rota (DE-PARA) ou 'sair' para encerrar: ");
        var input = Console.ReadLine();

        // Verificando se o input é nulo ou "sair"
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Entrada inválida. Tente novamente.");
            continue;
        }

        if (input.ToLower() == "sair")
            break;

        var parts = input.Split('-');
        if (parts.Length != 2)
        {
            Console.WriteLine("Formato inválido. Use 'DE-PARA'.");
            continue;
        }

        var origin = parts[0].Trim();
        var destination = parts[1].Trim();

        try
        {  
            var result = service.FindCheapestRoute(origin, destination);
            Console.WriteLine($"Melhor Rota: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao buscar rota: {ex.Message}");
        }
    } 
}
catch (FileNotFoundException ex)
{
    Console.WriteLine($"Erro: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"Erro inesperado: {ex.Message}");
}