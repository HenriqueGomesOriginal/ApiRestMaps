using ApiRestMaps.Data.GoogleObj;
using ApiRestMaps.Data.VO;
using ApiRestMaps.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiRestMaps.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MapsController : ControllerBase
    {
        // Local variables
        private readonly ILogger<MapsController> _logger;
        private readonly IMapsService _service;

        public MapsController(ILogger<MapsController> logger, IMapsService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost]
        [Route("CalculateDistance")]
        [ProducesResponseType(typeof(List<MapsResult>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CalculateDistance(MapsRequest _address)
        {
            var ret = await _service.calculateDistance(_address.AddressArray);
            return Ok(ret);
        }
    }
}
