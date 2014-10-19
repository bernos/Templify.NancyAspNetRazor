using System;
using Bernos.Security;
using FluentValidation;
using MediatR;

namespace Templify.NancyAspNetRazor.Data.Auth.Commands
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
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
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;
        private readonly IPasswordHasher _passwordHasher;

        public LoginCommandHandler(Func<IUnitOfWork> unitOfWorkFactory, IPasswordHasher passwordHasher)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _passwordHasher = passwordHasher;
        }

        public LoginCommandResult Handle(LoginCommand message)
        {
            using (var unitOfWork = _unitOfWorkFactory())
            {
                var respository = unitOfWork.UserRepository;
                var user = respository.GetUser(message.Username);

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
    }
}