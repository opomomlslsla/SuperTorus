
using Microsoft.AspNetCore.Mvc;
using SuperTorus.Application.Services;
using SuperTorus.Application.DTO;

namespace SuperTorus.Server.Controllers
{
    [Route("Amin/[controller]")]
    [ApiController]
    public class TorusController(TorusService service, TorusDBService torusDBService) : ControllerBase
    {
        private readonly TorusService _service = service;
        private readonly TorusDBService _torusDbService = torusDBService;



        [HttpGet("GetById/{id}")]
        public IActionResult Get(Guid id)
        {
            return Ok(_torusDbService.GetTorus(id));
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_torusDbService.GetAllTorus());
        }

        [HttpPost("AddingToruses")]
        public async Task<IActionResult> AddTorus(RequestData requestData)
        {
            await _torusDbService.AddTorus(requestData);
            return Ok();
        }


        [HttpPost("TorusCalcAsync")]
        public async Task<IActionResult> CalculateAsync(RequestData requestData)
        {
            double res = 0;
            res = await _service.CalculateTorusAsync(requestData);
            return Ok(res);
        }

        [HttpPost("TorusCalcParallel")]
        public IActionResult CalculateParralell(RequestData requestData)
        {
            double res = 0;
            res = _service.CalculateTorusParallel(requestData);
            return Ok(res);
        }

        [HttpPost("TorusChek")]
        public async Task<IActionResult> TorusChek(RequestData requestData)
        {
            var res = await _service.ChekTorus(requestData);
            return Ok(res);
        }
    }
}
