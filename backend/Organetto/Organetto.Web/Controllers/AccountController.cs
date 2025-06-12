using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organetto.Core.Authentication.Ports.Data;
using Organetto.UseCases.Authentication.Commands;
using Organetto.UseCases.Authentication.Queries;
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
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest dto, CancellationToken ct)
        {
            var uid = await _mediator.Send(new RegisterUserCommand(dto.Email, dto.Password, dto.DisplayName), ct);
            return Created(string.Empty, new { uid });
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest dto)
        {
            var tokens = await _mediator.Send(new LoginUserQuery(dto.Email, dto.Password));
            return Ok(tokens);
        }

        [HttpPost("refresh")]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest dto)
        {
            var tokens = await _mediator.Send(new RefreshTokenCommand(dto.RefreshToken));
            return Ok(tokens);
        }
    }
}
