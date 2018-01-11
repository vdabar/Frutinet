using FluentValidation;
using Frutinet.Common.Commands;
using Frutinet.Common.Domain;
using Frutinet.Common.Extensions;
using Frutinet.Common.Services;
using Frutinet.Services.Identity.Users.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frutinet.Services.Identity.Users
{
    public class User : AggregateRoot
    {
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public Provider Provider { get; set; }
        public Role Role { get; set; }
        public State State { get; set; }
        public string ExternalUserId { get; set; }
        public string Culture { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public User()
        {
        }

        public User(CreateUser cmd)
        {
            Email = cmd.Email;
            UserName = cmd.Username;
            Firstname = cmd.FirstName;
            Surname = cmd.LastName;
            State = State.Active;
            Role = Role.Associate;
            Provider = Provider.Frutinet;
        }

        public static User CreateNew(CreateUser cmd, IValidator<CreateUser> validator)
        {
            validator.ValidateCommand(cmd);
            return new User(cmd);
        }
    }
}