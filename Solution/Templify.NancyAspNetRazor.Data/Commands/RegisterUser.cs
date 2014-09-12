using System;
using Bernos.Security;
using MediatR;
using Templify.NancyAspNetRazor.Data.Models;

namespace Templify.NancyAspNetRazor.Data.Commands
{
    public class RegisterUserCommand : IRequest<RegisterUserCommandResult>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterUserCommandResult
    {
        public Guid UserId { get; private set; }
        public bool HasError { get; private set; }
        public string Error { get; private set; }

        public RegisterUserCommandResult(Guid userId)
        {
            UserId = userId;
        }

        public RegisterUserCommandResult(string error)
        {
            HasError = true;
            Error = error;
        }
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserCommandResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public RegisterUserCommandResult Handle(RegisterUserCommand message)
        {
            // First, assert that username does not already existd
            if (_userRepository.GetUser(message.Username) != null)
            {
                throw new Exception(string.Format("User {0} already exists", message.Username));
                return new RegisterUserCommandResult(string.Format("Username {0} is not available", message.Username));                
            }

            // Create the user
            var user = new User(message.Username, _passwordHasher.CreateHash(message.Password));
            
            // TODO: switch to unit of work here
            _userRepository.AddUser(user);
            _userRepository.Save();

            return new RegisterUserCommandResult(user.UserId);
        }
    }
}