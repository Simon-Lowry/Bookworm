using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bookworm.Contracts.Services;
using Bookworm.Contracts;
using Bookworm.ViewModels.Home;
using Bookworm.Models;

namespace Bookworm.Services
{
    public class SignUpService : ISignUpService
    {
        private readonly IRepository<User> _userRepository;

        public SignUpService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public bool AddUser(UserSignUpViewModel userDetails)
        {
            DateTime Dob;
            DateTime.TryParse(userDetails.DOB, out Dob);

            var user = new User()
            {
                FirstName = userDetails.FirstName,
                LastName = userDetails.LastName,
                City = userDetails.City,
                Country = userDetails.Country,
                DOB = Dob,
                Email = userDetails.Email,
                Password = userDetails.Password        
            };

            bool result = _userRepository.Create(user);

            return result;
        }

        public User GetUserDetails(UserSignUpViewModel userSignUpDetails)
        {
            String passwordEntered = userSignUpDetails.Password;
            String emailEntered = userSignUpDetails.Email;

            var allUserDetails = from users in _userRepository.GetAll() select users;
            var userProfile =
                (allUserDetails.Where(u => u.Password.Equals(passwordEntered) && u.Email.Equals(emailEntered))).FirstOrDefault();

            return userProfile;
        }
    }
}