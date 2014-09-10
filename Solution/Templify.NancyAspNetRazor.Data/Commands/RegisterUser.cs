using System;
using Bernos.Security;
using Templify.NancyAspNetRazor.Data.Models;

namespace Templify.NancyAspNetRazor.Data.Commands
{
    public class RegisterUserCommand : ICommand<RegisterUserCommandResult>
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

    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, RegisterUserCommandResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public RegisterUserCommandResult Execute(RegisterUserCommand command)
        {
            // First, assert that username does not already exist
            if (_userRepository.GetUser(command.Username) != null)
            {
                return new RegisterUserCommandResult(string.Format("Username {0} is not available", command.Username));                
            }

            // Create the user
            var user = new User(command.Username, _passwordHasher.CreateHash(command.Password));
            
            // TODO: switch to unit of work here
            _userRepository.AddUser(user);
            _userRepository.Save();

            return new RegisterUserCommandResult(user.UserId);
        }
    }
}