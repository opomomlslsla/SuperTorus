using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using SuperTorus.Application.Services;
using SuperTorus.Application.DTO;



namespace SuperTorus.Server.Controllers
{
    [Route("Amin/[controller]")]
    [ApiController]
    public class TorusController(TorusService service) : ControllerBase
    {
        private readonly TorusService _service = service;

        [HttpGet("GetById/{id}")]
        public IActionResult Get(Guid id)
        {
            return Ok(_service.GetTorus(id));
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAllTorus());
        }

        [HttpPost("AddingToruses")]
        public IActionResult AddTorus(RequestData requestData)
        {
            _service.AddTorus(requestData);
            return Ok();
        }

        [HttpGet("GetString")]
        public string GetString()
        {
            return "sdsdsa";
        }
    }
}
