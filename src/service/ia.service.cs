using System.Text;
using System.Text.Json;
namespace appsy.src.service;

public class IAService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public IAService(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _apiKey = configuration["Gemini:ApiKey"] 
            ?? throw new InvalidOperationException("Gemini:ApiKey no encontrada en configuración");
    }

    public async Task<string> Consult(string userPrompt)
    {
        var prompt = $@"
Eres un experto en UX/UI.

Tu tarea es generar únicamente JSON válido para Excalidraw.

REGLAS OBLIGATORIAS:

1. Devuelve SOLO JSON.
2. No uses markdown.
3. No uses ```json.
4. No expliques nada.
5. La respuesta debe ser parseable con JsonSerializer.
6. Siempre devolver:
{{
  ""elements"": []
}}

7. Todos los elementos deben tener:
- id
- type
- x
- y
- width
- height
- text

8. Si el usuario pide una aplicación compleja, crea todos los componentes necesarios.

Solicitud del usuario:

{userPrompt}
";

        var body = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
        };

        var json = JsonSerializer.Serialize(body);
        
        var requestUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-flash-latest:generateContent?key={_apiKey}";
        
        var response = await _httpClient.PostAsync(
            requestUrl,
            new StringContent(json, Encoding.UTF8, "application/json")
        );

        response.EnsureSuccessStatusCode();

        var resultJson = await response.Content.ReadAsStringAsync();
        
        // Parsear la respuesta de Gemini para extraer solo el texto generado
        using var document = JsonDocument.Parse(resultJson);
        var generatedText = document.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        return generatedText ?? string.Empty;
    }
}