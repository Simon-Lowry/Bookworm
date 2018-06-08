using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using Bookworm.ViewModels.Home;
using System.Text.RegularExpressions;
using Bookworm.Contracts;

namespace Bookworm.Validators
{
    public class SignUpValidator : AbstractValidator<UserSignUpViewModel>, ISignUpValidator
    {
        public SignUpValidator()
        {
            RuleFor(userDetails => userDetails.FirstName).NotEmpty().WithMessage("Please specify a first name");
            RuleFor(userDetails => userDetails.LastName).NotEmpty().WithMessage("Please specify a last name");
            RuleFor(userDetails => userDetails.City).NotEmpty().WithMessage("Please specify a country");
            RuleFor(userDetails => userDetails.Country).NotEmpty().WithMessage("Please specify a country");
            RuleFor(userDetails => userDetails.Email).NotEmpty().EmailAddress().WithMessage("Please enter a valid email address");
            RuleFor(userDetails => userDetails.DOB).Must(BeAValidDate).WithMessage("Invalid date");
            RuleFor(userDetails => userDetails.Password).NotEmpty().NotNull().Must(BeAValidPassword).WithMessage("Password must contain at least 6 characters - " +
                "one upper case letter, one lower case letter and one digit");
            RuleFor(userDetails => userDetails.ConfirmPassword).Equal(userDetails => userDetails.Password, StringComparer.Ordinal).
                WithMessage("Passwords must match");
        }

        public bool BeAValidPassword(string password)
        {
            if (ContainsCorrectChars(password) && password.Length > 5)
                return true;
            else
                return false;
        }

        public bool BeAValidDate(string value)
        {
             
            DateTime date;
            return DateTime.TryParse(value, out date);
        }

        // contains an uppercase letter, lowercase letter and digit
        public bool ContainsCorrectChars(string password)
        {
            Regex regex = new Regex("[A-Z]+[a-z]+[0-9]+");
            if (password == null)
                return false;
            Match match = regex.Match(password);

            if (match.Success)
                return true;
            else
                return false;
             
        }

    }
}