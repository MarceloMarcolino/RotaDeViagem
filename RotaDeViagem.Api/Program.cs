using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RotaDeViagem.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Adicionando o RouteService ao container de serviços
builder.Services.AddSingleton<RouteService>(provider =>
{
    // Aqui você pode especificar o caminho para o arquivo CSV (se necessário)
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "rotas.csv");
    return new RouteService(filePath);
});

builder.Services.AddControllers(); // Adicionando suporte a controladores

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Rota de Viagem API",
        Version = "v1",
        Description = "API para gerenciar rotas e calcular rotas mais baratas."
    });
});
builder.Services.AddSingleton<RouteService>(_ => new RouteService("rotas.csv"));


var app = builder.Build();

// Configuração do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();

app.MapControllers(); // Mapeia os controladores automaticamente

app.Run();