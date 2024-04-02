using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using SuperTorus.Application.Services;
using SuperTorus.Application.DTO;
using FluentValidation;
using SuperTorus.Domain.Entities;



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
            try
            {
                res = await _service.CalculateTorus(requestData);
            }
            catch (ValidationException ex)
            {

            }
            catch (Exception ex)
            {
                //Logger.logError(ex.ToString());
                //return Ok(ex.Message);
            }
            return Ok(res);
        }

        [HttpPost("TorusChek")]
        public IActionResult TorusChek(RequestData requestData)
        {
            
            string res = "";
            try
            {
                res = _service.ChekTorus(requestData);
            }
            catch(ValidationException ex)
            {
                return Ok("Bad Data");
            }
            catch (Exception ex)
            {
                //Logger.logError(ex.ToString());
                //return Ok(ex.Message);
            }
            return Ok(res);
        }


        [HttpGet("GetString")]
        public string GetString()
        {
            return "sdsdsa";
        }
    }
}
