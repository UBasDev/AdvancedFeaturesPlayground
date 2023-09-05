using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookService.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        [HttpGet("[action]")]
        public IActionResult test1()
        {
            return Ok();
        }
    }
}
