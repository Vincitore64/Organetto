using MediatR;
using Microsoft.AspNetCore.Mvc;
using Organetto.UseCases.Boards.Commands;
using Organetto.UseCases.Boards.Data;
using Organetto.UseCases.Boards.Queries;

namespace Organetto.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BoardsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// GET /api/boards – Retrieves all boards for the current user. (GET /api/boards – Получает все доски для текущего пользователя.)
        /// </summary>
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<BoardDto>>> GetAllOfUser(long userId)
        {
            // Send the MediatR query
            var query = new GetAllUserBoardsQuery(userId);
            var boards = await _mediator.Send(query);

            return Ok(boards);
        }

        /// <summary>
        /// POST /api/boards
        /// Creates a new board for the current user.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<BoardDto>> Create([FromBody] CreateBoardCommand command)
        {
            var created = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(GetAllOfUser),
                new { userId = created.OwnerId },
                created
            );
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id, [FromQuery] long userId)
        {
            await _mediator.Send(new DeleteBoardCommand(id, userId));
            return NoContent();
        }
    }
}
