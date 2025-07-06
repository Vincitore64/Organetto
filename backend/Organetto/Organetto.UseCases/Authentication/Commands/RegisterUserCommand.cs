using MediatR;
using Organetto.Core.Authentication.Ports.Services;
using Organetto.Core.Shared.Databases;
using Organetto.Core.Users.Services;

namespace Organetto.UseCases.Authentication.Commands
{
    /// <summary>
    /// Command to create a new Firebase user.
    /// </summary>
    public sealed record RegisterUserCommand(string Email, string Password, string? DisplayName) : IRequest<string>;

    internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly IAuthenticationService _auth;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public RegisterUserCommandHandler(IAuthenticationService auth, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _auth = auth;
            this._unitOfWork = unitOfWork;
            this._userRepository = userRepository;
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var uuid = await _auth.RegisterUserAsync(request.Email, request.Password, request.DisplayName, cancellationToken);

            if (string.IsNullOrEmpty(uuid))
            {
                throw new InvalidOperationException("Failed to register user. UUID is empty.");
            }

            var user = await _userRepository.CreateAsync(new Core.Users.Data.User
            {
                FirebaseUid = uuid,
                Email = request.Email,
                Name = request.DisplayName ?? string.Empty,
            });
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return uuid;
        }
            
    }
}
