using FluentValidation;
using Frutinet.Common.Commands;
using Frutinet.Common.Events;
using Frutinet.Services.Identity.Users.Commands;
using Frutinet.Services.Identity.Users.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frutinet.Services.Identity.Users.Handlers
{
    public class CreateUserHandler : ICommandHandlerAsync<CreateUser>
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<CreateUser> _validator;

        public CreateUserHandler(IUserRepository userRepository, IValidator<CreateUser> validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public async Task<IEnumerable<IEvent>> HandleAsync(CreateUser command)
        {
            var user = User.CreateNew(command, _validator);
            await _userRepository.AddAsync(user);
            return user.Events;
        }
    }
}