using Bookworm.Contracts.Services;
using Bookworm.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bookworm.Contracts;
using Bookworm.Models;
using Microsoft.Ajax.Utilities;

namespace Bookworm.Services
{
    public class LoginService : ILoginService
    {
        private readonly IRepository<User> _userRepository;


        public LoginService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }


        public User GetUserDetails(UserLoginViewModel userLoginDetails)
        {
            String passwordEntered = userLoginDetails.Password;
            String emailEntered = userLoginDetails.Email;

            var allUserDetails = from users in _userRepository.GetListOf() select users;
            var userProfile =
                (allUserDetails.Where(u => u.Password.Equals(passwordEntered) && u.Email.Equals(emailEntered))).FirstOrDefault();

            return userProfile;
        }

    }
}