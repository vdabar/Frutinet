using FluentValidation;
using Frutinet.Services.Identity.Users.Commands;
using Frutinet.Services.Identity.Users.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Frutinet.Services.Identity.Users.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUser>
    {
        private readonly IUserRules _userRules;

        public CreateUserValidator(IUserRules userRules)
        {
            _userRules = userRules;
            RuleFor(c => c.Email)
               .NotEmpty().WithMessage("Email is required.")
               .EmailAddress().WithMessage("Email not valid.")
               .Length(1, 250).WithMessage("Email maximum length is 250 characters.")
               .MustAsync(HaveUniqueEmailAsync).WithMessage("Email already exists.");
            RuleFor(c => c.UserName)
                .NotEmpty().WithMessage("Name is required.")
                .Length(1, 250).WithMessage("Name maximum length is 250 characters.")
                .MustAsync(HaveUniqueUserNameAsync).WithMessage("Name already exists.");
            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("Password can not be empty!");
            RuleFor(c => Enum.Parse(typeof(Role), c.Role))
                .IsInEnum().WithMessage(c => $"Can not create a new account for user id: '{c.Id}', invalid role: '{c.Role}'.")
                .NotEqual(Role.Owner);
        }

        private async Task<bool> HaveUniqueUserNameAsync(string name, CancellationToken arg2)
        {
            return await _userRules.IsUserNameUnique(name);
        }

        private async Task<bool> HaveUniqueEmailAsync(string email, CancellationToken arg2)
        {
            return await _userRules.IsUserEmailUnique(email);
        }
    }
}