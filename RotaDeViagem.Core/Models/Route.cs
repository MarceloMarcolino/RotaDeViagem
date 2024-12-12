namespace RotaDeViagem.Core.Models;

public class Route
{
    public string Origin { get; set; }
    public string Destination { get; set; }
    public int Cost { get; set; }

    public Route(string origin, string destination, int cost)
    {
        Origin = origin;
        Destination = destination;
        Cost = cost;
    }
}