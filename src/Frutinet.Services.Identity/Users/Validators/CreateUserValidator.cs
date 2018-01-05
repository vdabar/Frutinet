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
            RuleFor(c => c.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .Length(1, 250).WithMessage("Email maximum length is 250 characters.");
            RuleFor(c => c.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .Length(1, 250).WithMessage("Email maximum length is 250 characters.");
            RuleFor(c => c.Username)
                .NotEmpty().WithMessage("Username is required.")
                .Length(1, 250).WithMessage("Username maximum length is 250 characters.")
                .MustAsync(HaveUniqueUserNameAsync).WithMessage("Username already exists.");
            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("Password is required.");
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