using System;
using Bernos.Security;
using MediatR;
using Templify.NancyAspNetRazor.Data.Auth.Models;

namespace Templify.NancyAspNetRazor.Data.Auth.Commands
{
    public class RegisterUserCommand : IRequest<RegisterUserCommandResult>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterUserCommandResult
    {
        public Guid UserId { get; private set; }
        
        public RegisterUserCommandResult(Guid userId)
        {
            UserId = userId;
        }
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserCommandResult>
    {
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserCommandHandler(Func<IUnitOfWork> unitOfWorkFactory, IPasswordHasher passwordHasher)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _passwordHasher = passwordHasher;
        }
        
        public RegisterUserCommandResult Handle(RegisterUserCommand message)
        {
            using (var unitOfWork = _unitOfWorkFactory())
            {
                var repository = unitOfWork.UserRepository;

                // First, assert that username does not already existd
                if (repository.GetUser(message.Username) != null)
                {
                    throw new Exception(string.Format("User {0} already exists", message.Username));
                }

                // Create the user
                var user = new User(message.Username, _passwordHasher.CreateHash(message.Password));

                repository.Add(user);

                unitOfWork.Commit();

                return new RegisterUserCommandResult(user.UserId);
            }
        }
    }
}