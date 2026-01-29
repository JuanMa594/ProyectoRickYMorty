using System.Net.Http.Json;
using System.Text.Json;
using ProyectoRickYMorty.Models;

namespace ProyectoRickYMorty.Services;

public interface IRickMortyService
{
    Task<ApiResponse<Character>> GetCharactersAsync(int page = 1);
    Task<Character?> GetByIdAsync(int id);
    Task<List<Character>> GetMultipleCharactersAsync(params int[] ids);
    Task<ApiResponse<Character>> FilterCharactersAsync(string? name = null, string? status = null, 
        string? species = null, string? type = null, string? gender = null);
}

public class RickMortyService : IRickMortyService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptions _jsonOptions;

    public RickMortyService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        
        // Configurar JSON options para deserialización consistente
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    private HttpClient CreateClient()
    {
        return _httpClientFactory.CreateClient("RickAndMortyApi");
    }

    public async Task<ApiResponse<Character>> GetCharactersAsync(int page = 1)
    {
        try
        {
            var client = CreateClient();
            var url = $"?page={page}";
            
            var response = await client.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error al obtener personajes: {response.StatusCode} - {error}");
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<Character>>(_jsonOptions);
            return result ?? new ApiResponse<Character>();
        }
        catch (HttpRequestException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener personajes: {ex.Message}", ex);
        }
    }

    public async Task<Character?> GetByIdAsync(int id)
    {
        try
        {
            var client = CreateClient();
            var response = await client.GetAsync($"/{id}");
            
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;
                
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error al obtener personaje: {response.StatusCode}");
            }

            var character = await response.Content.ReadFromJsonAsync<Character>(_jsonOptions);
            return character;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener personaje: {ex.Message}", ex);
        }
    }

    public async Task<List<Character>> GetMultipleCharactersAsync(params int[] ids)
    {
        try
        {
            if (ids == null || ids.Length == 0)
                return new List<Character>();

            var client = CreateClient();
            var idsString = string.Join(",", ids);
            var response = await client.GetAsync($"/{idsString}");
            
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error al obtener personajes: {response.StatusCode} - {error}");
            }

            var characters = await response.Content.ReadFromJsonAsync<List<Character>>(_jsonOptions);
            return characters ?? new List<Character>();
        }
        catch (HttpRequestException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener múltiples personajes: {ex.Message}", ex);
        }
    }

    public async Task<ApiResponse<Character>> FilterCharactersAsync(string? name = null, string? status = null, 
        string? species = null, string? type = null, string? gender = null)
    {
        try
        {
            var client = CreateClient();
            
            // Construir query string con filtros
            var queryParams = new List<string>();
            if (!string.IsNullOrEmpty(name)) 
                queryParams.Add($"name={Uri.EscapeDataString(name)}");
            if (!string.IsNullOrEmpty(status)) 
                queryParams.Add($"status={Uri.EscapeDataString(status)}");
            if (!string.IsNullOrEmpty(species)) 
                queryParams.Add($"species={Uri.EscapeDataString(species)}");
            if (!string.IsNullOrEmpty(type)) 
                queryParams.Add($"type={Uri.EscapeDataString(type)}");
            if (!string.IsNullOrEmpty(gender)) 
                queryParams.Add($"gender={Uri.EscapeDataString(gender)}");
            
            var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
            var url = $"{queryString}";
            
            var response = await client.GetAsync(url);
            
            // La API retorna 404 cuando no hay resultados para el filtro
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new ApiResponse<Character> 
                { 
                    Info = new Info { Count = 0, Pages = 0 },
                    Results = new List<Character>() 
                };
            }
            
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error al filtrar personajes: {response.StatusCode} - {error}");
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<Character>>(_jsonOptions);
            return result ?? new ApiResponse<Character>();
        }
        catch (HttpRequestException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al filtrar personajes: {ex.Message}", ex);
        }
    }
}