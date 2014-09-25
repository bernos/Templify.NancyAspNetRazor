using System;
using System.Threading.Tasks;
using Bernos.Security;
using FluentValidation;
using MediatR;

namespace Templify.NancyAspNetRazor.Data.Commands
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(c => c.Username).NotEmpty();
            RuleFor(c => c.Password).NotEmpty();
        }
    }

    public class LoginCommandValidatorAsync : AbstractValidator<LoginCommandAsync>
    {
        public LoginCommandValidatorAsync()
        {
            RuleFor(c => c.Username).NotEmpty();
            RuleFor(c => c.Password).NotEmpty();
        }
    }

    public class LoginCommand : IRequest<LoginCommandResult>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginCommandAsync : IAsyncRequest<LoginCommandResult>
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

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginCommandResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public LoginCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public LoginCommandResult Handle(LoginCommand message)
        {
            var user = _userRepository.GetUser(message.Username);

            if (user != null)
            {
                if (_passwordHasher.ValidatePassword(message.Password, user.Password))
                {
                    return new LoginCommandResult(true, user.UserId);
                }
            }

            return new LoginCommandResult(false, null);
        }
    }

    public class LoginCommandHandlerAsync : IAsyncRequestHandler<LoginCommandAsync, LoginCommandResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public LoginCommandHandlerAsync(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<LoginCommandResult> Handle(LoginCommandAsync message)
        {
            return await Task.Run(() =>
            {
                var user = _userRepository.GetUser(message.Username);

                if (user != null)
                {
                    if (_passwordHasher.ValidatePassword(message.Password, user.Password))
                    {
                        return new LoginCommandResult(true, user.UserId);
                    }
                }

                return new LoginCommandResult(false, null);
            });
        }
    }
}