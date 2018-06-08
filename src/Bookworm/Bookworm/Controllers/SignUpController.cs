using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bookworm.Models;
using Bookworm.ViewModels.Home;
using Bookworm.Contracts;
using FluentValidation;
using Bookworm.Validators;
using FluentValidation.Results;
using Bookworm.Contracts.Services;

namespace Bookworm.Controllers
{
    public class SignUpController : Controller
    {
        private readonly ISignUpService _signUpService;
        private readonly SignUpValidator _validator;
   
        
        public SignUpController(ISignUpService signUp)
        {
            _signUpService = signUp;
            _validator = new SignUpValidator();
        }
        

        // GET: SignUp
        public ActionResult SigningUp()
        {
            ViewBag.Message = "SignUp Page";

            return View();
        }

       
        [HttpPost]
        public ActionResult SigningUp(UserSignUpViewModel userSignUpDetails)
        {
            ValidationResult results = _validator.Validate(userSignUpDetails);

            if (results.IsValid ) //  valid signup, update DB and redirect to profile page
            {
                // add new user to db
                _signUpService.AddUser(userSignUpDetails);

                // retrieve newly added user's details from db
                User userProfile = _signUpService.GetUserDetails(userSignUpDetails);
                Session["userId"] = userProfile.UserId;

                return RedirectToAction("MyProfile", "Profiles");
            }
            else
            {

                foreach (ValidationFailure failure in results.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }

                ViewBag.Message = "Error in SignUp Occured";
                return View(userSignUpDetails);
            }
        }
    }
}