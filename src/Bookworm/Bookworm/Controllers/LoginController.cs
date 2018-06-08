using Bookworm.Contracts.Services;
using Bookworm.ViewModels.Home;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bookworm.Models;

namespace Bookworm.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            this._loginService = loginService;
        }


        public ActionResult LoginPage()
        {
            ViewBag.Message = "Login Page";
            return View();
        }


        [HttpPost]
        public ActionResult AttemptLogin(UserLoginViewModel userLoginDetails)
        {
            // Check in DB for existing 
            // return either profile page or error (separate error based on whether
            ValidationResult results = new ValidationResult();
            User userDetails = _loginService.GetUserDetails(userLoginDetails);

            if (userDetails != null)
            {
                Session["userId"] = userDetails.UserId;                 // set the session informa
                return RedirectToAction( "MyProfile", "Profiles");
            }
            else
            {
                ValidationFailure loginFailure = new ValidationFailure("Password", "Incorrect Login Details Please try Again");

                ViewBag.Message = "Error Occured";

                results.Errors.Add(loginFailure);
                foreach (ValidationFailure failure in results.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }

                return RedirectToAction( "LoginPage", "Login", userLoginDetails);
            }
          
        }
       
    }
}