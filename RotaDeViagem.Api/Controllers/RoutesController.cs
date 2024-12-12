using Microsoft.AspNetCore.Mvc;
using RotaDeViagem.Core.Models; // Namespace para o modelo Route
using RotaDeViagem.Core.Services;

[ApiController]
[Route("api/routes")]
public class RoutesController : ControllerBase
{
    private readonly RouteService _service;

    public RoutesController(RouteService service)
    {
        _service = service;
    }

    [HttpGet("{origin}/{destination}")]
    public IActionResult GetCheapestRoute(string origin, string destination)
    {
        var result = _service.FindCheapestRoute(origin, destination);
        return Ok(result);
    }

    [HttpPost]
    public IActionResult AddRoute([FromBody] RotaDeViagem.Core.Models.Route newRoute) // Especificando o namespace completo
    {
        try
        {
            _service.AddRoute(newRoute, "rotas.csv");
            return CreatedAtAction(nameof(AddRoute), newRoute);
        }
        catch (Exception ex)
        {
            return Conflict(new { message = "Rota já existente ou erro ao adicionar.", details = ex.Message });
        }
    }
}
