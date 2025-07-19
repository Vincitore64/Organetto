using MediatR;
using Microsoft.AspNetCore.Mvc;
using Organetto.UseCases.Boards.Columns.Commands;
using Organetto.UseCases.Boards.Columns.Data;
using Organetto.UseCases.Boards.Columns.Queries;

namespace Organetto.Web.Controllers
{
    [ApiController]
    [Route("api/boards/{boardId:long}/columns")]
    public class ColumnsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ColumnsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// List all columns for a given board.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(BoardListDto[]), 200)]
        public async Task<IActionResult> GetColumns(
            [FromRoute] long boardId,
            CancellationToken cancellationToken)
        {
            var query = new GetColumnsByBoardQuery(boardId);
            var columns = await _mediator.Send(query, cancellationToken);
            return Ok(columns);
        }

        /// <summary>
        /// Create a new column in the board.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(BoardListDto), 201)]
        public async Task<IActionResult> CreateColumn(
            [FromRoute] long boardId,
            [FromBody] CreateColumnCommand command,
            CancellationToken cancellationToken)
        {
            if (command.BoardId != boardId)
                return BadRequest("BoardId in route and body must match.");

            var column = await _mediator.Send(command, cancellationToken);
            return Ok(column);
        }

        /// <summary>
        /// Rename or reorder an existing column.
        /// </summary>
        [HttpPatch]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateColumn(
            [FromRoute] long boardId,
            [FromBody] UpdateColumnMetadataCommand command,
            CancellationToken cancellationToken)
        {
            if (command.BoardId != boardId)
                return BadRequest("Route and body identifiers must match.");

            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Delete a column from the board.
        /// </summary>
        [HttpDelete("{columnId:long}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteColumn(
            [FromRoute] long boardId,
            [FromRoute] long columnId,
            CancellationToken cancellationToken)
        {
            var command = new DeleteColumnCommand(boardId, columnId);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
