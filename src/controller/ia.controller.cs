using System.Text.Json;
using appsy.src.dtos;
using appsy.src.service;
using Microsoft.AspNetCore.Mvc;

namespace appsy.src.controller;
[ApiController]
[Route("api/ia")]
public class IAController : ControllerBase
{
    private readonly IAService _service;
    public IAController(IAService service)
    {
        _service = service;
    }

    [HttpPost("{id}")]
    public Task<object> Consult(string id, [FromBody] IADto dto)
    {
        return _service.Consult(id,dto.Msg);
    }
    
}