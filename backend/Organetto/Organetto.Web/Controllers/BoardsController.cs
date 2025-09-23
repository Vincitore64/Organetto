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
        /// Gets a board with its columns and cards by ID.
        /// </summary>
        [HttpGet("{boardId}")]
        [ProducesResponseType(typeof(BoardDetailDto), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<ActionResult<BoardDetailDto>> GetBoard(
            [FromRoute] long boardId,
            CancellationToken cancellationToken)
        {
            var query = new GetBoardQuery(boardId);
            var board = await _mediator.Send(query, cancellationToken);
            return Ok(board);
        }

        /// <summary>
        /// GET /api/boards – Retrieves all boards for the current user. (GET /api/boards – Получает все доски для текущего пользователя.)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoardDto>>> GetAllOfUser([FromQuery] long userId, CancellationToken cancellationToken)
        {
            // Send the MediatR query
            var query = new GetAllUserBoardsQuery(userId);
            var boards = await _mediator.Send(query, cancellationToken);

            return Ok(boards);
        }

        /// <summary>
        /// POST /api/boards
        /// Creates a new board for the current user.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<BoardDto>> Create([FromBody] CreateBoardCommand command, CancellationToken cancellationToken)
        {
            var created = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetAllOfUser),
                new { userId = created.OwnerId },
                created
            );
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateBoardMetadataCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id, [FromQuery] long userId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteBoardCommand(id, userId), cancellationToken);
            return NoContent();
        }
    }
}
