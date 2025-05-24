using MediatR;
using Microsoft.AspNetCore.Mvc;
using Organetto.UseCases.Authentication.Commands;
using Organetto.Web.Authentication.Data;

namespace Organetto.Web.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AccountController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Creates a new account in Firebase Authentication.
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Register([FromBody] RegisterUserData dto, CancellationToken ct)
        {
            var uid = await _mediator.Send(new RegisterUserCommand(dto.Email, dto.Password, dto.DisplayName), ct);
            return Created(string.Empty, new { uid });
        }
    }
}
