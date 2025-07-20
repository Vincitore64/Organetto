using MediatR;
using Microsoft.AspNetCore.Mvc;
using Organetto.UseCases.Boards.Columns.Cards.Commands;
using Organetto.UseCases.Boards.Columns.Cards.Queries;
using Organetto.UseCases.Boards.Data;

namespace Organetto.Web.Controllers
{
    [ApiController]
    [Route("api/columns/{columnId}/cards")]
    public class CardsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CardsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// GET api/columns/{columnId}/cards
        /// Retrieves all cards in the specified column.
        /// DueDate mapping isn't done here.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CardDto>>> GetForColumn(long columnId, CancellationToken cancellationToken)
        {
            var dtos = await _mediator.Send(new GetCardsQuery(columnId), cancellationToken);
            return Ok(dtos);
        }

        /// <summary>
        /// POST api/columns/{columnId}/cards
        /// Creates a new card in the specified column.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CardDto>> Create(
            long columnId,
            [FromBody] CreateCardCommand command,
            CancellationToken cancellationToken)
        {
            if (command.ColumnId != columnId)
                return BadRequest("ColumnId in route and payload must match.");

            var created = await _mediator.Send(command, cancellationToken);
            // Returns 201 with location header pointing to the list endpoint
            return CreatedAtAction(
                nameof(GetForColumn),
                new { columnId },
                created);
        }

        /// <summary>
        /// PUT api/columns/{columnId}/cards/{id}
        /// Updates metadata of an existing card.
        /// </summary>
        [HttpPut]
        public async Task<ActionResult> Update(
            long columnId,
            [FromBody] UpdateCardCommand command, CancellationToken cancellationToken)
        {
            if (command.ColumnId != columnId)
                return BadRequest("Route parameters and command payload must match.");

            var updated = await _mediator.Send(command, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// DELETE api/columns/{columnId}/cards/{id}
        /// Deletes the specified card.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            long columnId,
            long id,
            CancellationToken cancellationToken)
        {

            await _mediator.Send(new DeleteCardCommand(columnId, id), cancellationToken);
            return NoContent();
        }
    }
}
