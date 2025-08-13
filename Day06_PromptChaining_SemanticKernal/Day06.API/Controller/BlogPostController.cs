using Day06.API.CommandQueries.command;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Day06.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly IMediator mediator;

        public BlogPostController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        // ---------------------------
        // BlogPost Create
        // ---------------------------
        [HttpPost]
        [Route("Create-Blog")]
        public async Task<IActionResult> Create([FromBody] BlogPostCommand command)
        {

            var guid = await mediator.Send(command);
            return Ok(guid);
        }
        // ---------------------------
        // Outline Create
        // ---------------------------
        [HttpPost("outline")]
        public async Task<IActionResult> CreateOutline([FromBody] CreateOutlineCommand command)
        {
            var id = await mediator.Send(command);
            return Ok(id);
        }
        // ---------------------------
        // ContentBlock Create
        // ---------------------------
        [HttpPost("contentblock")]
        public async Task<IActionResult> CreateContentBlock([FromBody] CreateContentBlockCommand command)
        {
            var id = await mediator.Send(command);
            return Ok(id);
        }

        // ---------------------------
        // Review Create
        // ---------------------------
        [HttpPost("review")]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewCommand command)
        {
            var id = await mediator.Send(command);
            return Ok(id);
        }
    }
}
