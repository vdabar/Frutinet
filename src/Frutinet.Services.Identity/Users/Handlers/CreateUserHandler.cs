using FluentValidation;
using Frutinet.Common.Commands;
using Frutinet.Common.Events;
using Frutinet.Services.Identity.Users.Commands;
using Frutinet.Services.Identity.Users.Events;
using Frutinet.Services.Identity.Users.Repositories;
using RawRabbit;
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
        private readonly IBusClient _bus;

        public CreateUserHandler(IUserRepository userRepository, IValidator<CreateUser> validator, IBusClient bus)
        {
            _userRepository = userRepository;
            _validator = validator;
            _bus = bus;
        }

        public async Task<IEnumerable<IEvent>> HandleAsync(CreateUser command)
        {
            try
            {
                var user = User.CreateNew(command, _validator);
                await _userRepository.AddAsync(user);
                return user.Events;
            }
            catch (Exception ex)
            {
                await _bus.PublishAsync(
                   new CreateUserRejected(command.Id, "a", "error", ex.GetBaseException().Message),
                   command.Id,
                   cfg => cfg.WithExchange(e => e.WithName("Frutinet.Services.Identity")).WithRoutingKey("user.rejected"));
            }
            return null;
        }
    }
}