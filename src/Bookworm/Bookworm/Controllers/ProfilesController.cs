using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bookworm.Contracts.Services;
using Bookworm.Models;
using Bookworm.ViewModels.Profiles;
using Bookworm.Enums;
using Bookworm.ViewModels;
using Bookworm.ViewModels.Books;


namespace Bookworm.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly IProfileService _profileService;
        private readonly IBookService _bookService;


        public ProfilesController(IProfileService profileService, IBookService bookSerivce)
        {
            _profileService = profileService;
            _bookService = bookSerivce;
        }


        // GET: Profiles
        public ActionResult MyProfile()
        {
            int userId = Convert.ToInt32(Session["userId"]);

            User user = _profileService.GetUserDetails(userId);

            ViewBag.Message = "My Profile";

            MyBookReviewsDetails myBookReviews = _bookService.GetAllOfAUsersBookReviewsDetails(userId);
            MyConnectionDetails myConnections = _profileService.GetAllOfAUsersConnectionsDetails(userId);

            List<Book> myToReadShelf = _bookService.GetBooksOnUsersBookShelf(userId);

            MyProfileViewModel myProfile = new MyProfileViewModel()
            {
                MyDetails = user,
                MyBookReviews = myBookReviews,
                MyConnections = myConnections,
                MyToReadBookDetails = myToReadShelf
            };

            return View(myProfile);
        }


        public ActionResult Logout()
        {
            ViewBag.Message = "Redirect to Login Page after Logging out";
            return RedirectToAction("LoginPage", "Login");
        }


        public ActionResult OtherUsersProfile(int otherUserId)
        {
            int userId = Convert.ToInt32(Session["userId"]);

            // check are they connected
            Connection connection = new Connection()
            {
                UserId = userId,
                OtherUserId = otherUserId
            };

            bool usersAreConnected = _profileService.AreUsersConnected(connection);

            User myUserDetails = _profileService.GetUserDetails(userId);
            User otherUserDetails = _profileService.GetUserDetails(otherUserId);
           
            CombinedUserProfilesViewModel bothProfilesViewModel = new CombinedUserProfilesViewModel()
            {
                OtherUserDetails = otherUserDetails,
                MyUserDetails = myUserDetails,
                UsersAreConnected = usersAreConnected,
                Connection = connection
            };

            bothProfilesViewModel.OtherUsersConnections = _profileService.GetAllOfAUsersConnectionsDetails(otherUserId);
            bothProfilesViewModel.OtherUsersBookReviews = _bookService.GetAllOfAUsersBookReviewsDetails(otherUserId);
            bothProfilesViewModel.OtherUsersToReadBookDetails = new MyToReadDetails();
            bothProfilesViewModel.OtherUsersToReadBookDetails.ToReadBooksDetails = _bookService.GetBooksOnUsersBookShelf(otherUserId);

            ViewBag.Message = "Other User's Profile";
            return View(bothProfilesViewModel);
        }
    

        public ActionResult CreateConnectionBetweenUsers(CombinedUserProfilesViewModel bothUserProfileDetails )
        {
            bool isConnectionCreated = _profileService.AddConnection(bothUserProfileDetails.Connection);

            bothUserProfileDetails.UsersAreConnected = isConnectionCreated;

            bothUserProfileDetails.ConnectionWasCreated = (isConnectionCreated) ? CombinedUserProfilesViewModel.Success :
                   CombinedUserProfilesViewModel.Failed;
            
            return RedirectToAction("OtherUsersProfile", "Profiles", bothUserProfileDetails);
        }


        public ActionResult DeleteConnectionBetweenUsers(CombinedUserProfilesViewModel bothUserProfileDetails)
        {
            bool isConnectionDeleted = _profileService.DeleteConnection(bothUserProfileDetails.Connection);

            bothUserProfileDetails.UsersAreConnected = isConnectionDeleted;

            bothUserProfileDetails.ConnectionWasDeleted = (isConnectionDeleted) ? CombinedUserProfilesViewModel.Success :
                    CombinedUserProfilesViewModel.Failed;
          
            return RedirectToAction("OtherUsersProfile", "Profiles", bothUserProfileDetails);
        }


        public ActionResult MyBookReviews()
        {
            int userId = Convert.ToInt32(Session["userId"]);
            ViewBag.Message = "My Book Reviews";
            MyBookReviewsDetails myBookReviews = _bookService.GetAllOfAUsersBookReviewsDetails(userId);
            return View(myBookReviews);
        }


        public ActionResult MyConnections()
        {
            int userId = Convert.ToInt32(Session["userId"]);
            ViewBag.Message = "My Connections";
            MyConnectionDetails myConnections = _profileService.GetAllOfAUsersConnectionsDetails(userId);

            return View(myConnections);
        }


        public ActionResult MyBookShelf()
        {
            int userId = Convert.ToInt32(Session["userId"]);
            ViewBag.Message = "My Book Shelf";
            MyToReadDetails myToReadShelf = new MyToReadDetails();
            myToReadShelf.ToReadBooksDetails = _bookService.GetBooksOnUsersBookShelf(userId);
            return View(myToReadShelf);
        }


        public PartialViewResult ReadOnlyBookReviewModal(MyProfileViewModel bookViewModel)
        {
            return PartialView("~/Views/PartialViews/ReadOnlyBookReviewModal.cshtml", bookViewModel);
        }
    }
}