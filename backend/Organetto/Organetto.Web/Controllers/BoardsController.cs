using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<IEnumerable<BoardDto>>> GetAll(long userId)
        {
            // Assume we extract Firebase UID from JWT and then resolve internal userId.
            // Здесь предполагается, что из JWT мы получаем FirebaseUid и затем резолвим внутренний userId.
            //var firebaseUid = User.FindFirst("uid")?.Value;
            //if (string.IsNullOrEmpty(firebaseUid))
            //{
            //    return Unauthorized();
            //}

            // Resolve internal userId from Firebase UID – you need a lookup in Users table.
            // (Резолвим внутренний userId по Firebase UID – нужен запрос в таблицу Users.)
            // Example:
            // var userId = await _userService.GetInternalUserId(firebaseUid);

            // For demonstration, assume a helper method:
            //long userId = await ResolveInternalUserIdAsync(firebaseUid);

            // Send the MediatR query
            var query = new GetAllUserBoardsQuery(userId);
            var boards = await _mediator.Send(query);

            return Ok(boards);
        }

        private Task<long> ResolveInternalUserIdAsync(string firebaseUid)
        {
            // TODO: implement lookup, e.g., via IUserRepository or a cached dictionary.
            throw new NotImplementedException();
        }
    }
}
