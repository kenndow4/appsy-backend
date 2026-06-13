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

    [HttpPost]
    public Task<string> Consult([FromBody] IADto dto)
    {
        return _service.Consult(dto.Msg);
    }
    
}