using Bookworm.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookworm.Models;

namespace Bookworm.Contracts.Services
{
    public interface ISignUpService
    {
         bool AddUser(UserSignUpViewModel userDetails);
         User GetUserDetails(UserSignUpViewModel userSignUpDetails);
    }
}
