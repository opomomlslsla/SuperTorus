using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using SuperTorus.Application.Services;
using SuperTorus.Application.DTO;
using FluentValidation;
using SuperTorus.Domain.Entities;
using Serilog;



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
        public async Task<IActionResult> AddTorus(RequestData requestData)
        {
            await _service.AddTorus(requestData);
            return Ok();
        }


        [HttpPost("TorusCalc")]
        public async Task<IActionResult> Calculate(RequestData requestData)
        {
            double res = 0;
            res = _service.CalculateTorus(requestData);
            return Ok(res);
        }

        [HttpPost("TorusChek")]
        public async Task<IActionResult> TorusChek(RequestData requestData)
        {
            var res = _service.ChekTorus(requestData);
            return Ok(res);
        }
    }
}
