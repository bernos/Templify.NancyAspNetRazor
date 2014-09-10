using System;
using Bernos.Security;

namespace Templify.NancyAspNetRazor.Data.Commands
{
    public class LoginCommand : ICommand<LoginCommandResult>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginCommandResult
    {
        public Guid? UserId { get; private set; }
        public bool Success { get; private set; }

        public LoginCommandResult(bool success, Guid? userId)
        {
            Success = success;
            UserId = userId;
        }
    }

    public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginCommandResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public LoginCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public LoginCommandResult Execute(LoginCommand command)
        {
            var user = _userRepository.GetUser(command.Username);

            if (user != null)
            {
                if (_passwordHasher.ValidatePassword(command.Password, user.Password))
                {
                    return new LoginCommandResult(true, user.UserId);
                }
            }

            return new LoginCommandResult(false, null);
        }
    }
}