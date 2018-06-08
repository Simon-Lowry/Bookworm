using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bookworm.Contracts;
using Bookworm.Contracts.Services;
using NUnit.Framework;
using FakeItEasy;
using Bookworm.Controllers;
using FluentValidation;
using FluentValidation.Results;
using Bookworm.Validators;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bookworm.Data;
using Bookworm.Db_Config;
using Bookworm.Models;
using Bookworm.Repository;
using Bookworm.Validators;
using Bookworm.ViewModels.Home;
using FakeItEasy;
using NUnit.Framework;


namespace BookwormTests.Unit_Tests.Controllers
{
    [TestFixture]
    public class SignUpControllerUnitTests
    { 
       private SignUpController _signUpController;
       private ISignUpService _fakeSignUpService;
       private ILoginService _fakeLoginService;
       private IRepository<User> _fakeRepository;
       private SignUpValidator _validator;

        [SetUp]
       public void SetUp()
       {
           _fakeSignUpService = A.Fake<ISignUpService>();
           _fakeLoginService = A.Fake<ILoginService>();
           var _fakeBookwormDbContext = A.Fake<BookwormDbContext>();
           _signUpController = new SignUpController(_fakeSignUpService);
           _fakeRepository = new Repository<User>(_fakeBookwormDbContext);
           _validator = A.Fake<SignUpValidator>();

       }

        [Test]
        public void When_SigningUpCalled_ReturnSignUpView()
        {
            // Act
            ViewResult result = _signUpController.SigningUp() as ViewResult;

            // Assert
            Assert.That(result.ViewBag.Message, Is.EqualTo("SignUp Page"));

        }

      

    }
 }