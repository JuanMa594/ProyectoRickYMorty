using System.Text.Json.Serialization;

namespace ProyectoRickYMorty.Models;
public class ApiResponse<T>
{
    [JsonPropertyName("info")]
    public Info Info { get; set; }
    
    [JsonPropertyName("results")]
    public List<T> Results { get; set; }
}

public class Info
{
    [JsonPropertyName("count")]
    public int Count { get; set; }
    
    [JsonPropertyName("pages")]
    public int Pages { get; set; }
    
    [JsonPropertyName("next")]
    public string? Next { get; set; }
    
    [JsonPropertyName("prev")]
    public string? Prev { get; set; }
}