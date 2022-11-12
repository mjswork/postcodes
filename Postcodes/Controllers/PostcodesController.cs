using Microsoft.AspNetCore.Mvc;
using Postcodes.Models.Request;
using Postcodes.Models.Response;
using Postcodes.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Postcodes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostcodesController : ControllerBase
    {

        private readonly ILogger<PostcodesController> _logger;
        private readonly IPostcodeApiService _postcodeApiService;


        public PostcodesController(ILogger<PostcodesController> logger, IPostcodeApiService postcodeApiService)
        {
            _logger = logger;
            this._postcodeApiService = postcodeApiService;
        }

        [HttpGet(Name = "GetSinglePostcode")]
        [SwaggerResponse(200, "Returns a detailed response for a given postcode", typeof(PostcodeResponse))]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 86400)]
        public async Task<IActionResult> GetSinglePostcode(string postcode)
        {
            return Ok(await _postcodeApiService.GetPostcodeDetailsAsync(postcode));
        }

        [HttpPost(Name = "GetMultiplePostcodes")]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 86400)]
        public async Task<IActionResult> GetMultiplePostcodes([FromBody] PostcodesRequest postcodesRequest)
        {
            return Ok(await _postcodeApiService.GetPostcodeDetailsAsync(postcodesRequest));
        }
    }
}