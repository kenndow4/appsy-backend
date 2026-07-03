using System.Text;
using System.Text.Json;
namespace appsy.src.service;

public class IAService(IConfiguration configuration, BoardService boardService)
{
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly BoardService _boardService = boardService;
    private readonly string _apiKey = configuration["Gemini:ApiKey"]
            ?? throw new InvalidOperationException("Gemini:ApiKey no encontrada en configuración");

    public async Task<object> Consult(string boardId,string userPrompt)
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
        
        using var document = JsonDocument.Parse(resultJson);
        var generatedText = document.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();


        var parsedJson = JsonDocument.Parse(generatedText ?? "{}");

var elements = parsedJson
    .RootElement
    .GetProperty("elements");

var res = await _boardService.UpdateBoard(
    boardId,
    elements
);
        return res;
    }
}
