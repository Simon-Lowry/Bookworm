using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bookworm.Contracts.Services;
using Bookworm.Models;
using Bookworm.ViewModels.Recommendations;

namespace Bookworm.Controllers
{
    public class RecommenderController : Controller
    {
        private readonly IRecommenderService _recommenderService;


        public RecommenderController(IRecommenderService recommenderService)
        {
            _recommenderService = recommenderService;
        }


        // GET: Recommender
        public ActionResult BookRecommendations()
        {
            int userId = Convert.ToInt32(Session["userId"]);
            List<int> recommendationIds = _recommenderService.GetRecommendations(userId);
           
            RecommendationsViewModel booksRecommended =  new RecommendationsViewModel();
            booksRecommended.BooksRecommended = _recommenderService.GetBooksRecommended(recommendationIds);
            booksRecommended.BooksByUsersFavouriteAuthors = _recommenderService.GetBooksFromAuthorsAUserLiked(userId);
            booksRecommended.UserId = userId;

            return View(booksRecommended);
        }

    }
}