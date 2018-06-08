using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Autofac.Integration.Mvc;
using Bookworm;
using Bookworm.Contracts;
using Bookworm.Contracts.Services;
using Bookworm.Controllers;
using Bookworm.Data;
using Bookworm.Models;
using Bookworm.Repository;
using Bookworm.Services;
using Bookworm.ViewModels.Recommendations;
using BookwormTests.MockData;
using NUnit.Framework;
using FakeItEasy;
using NReco.CF;
using NReco.CF.Taste.Eval;
using NReco.CF.Taste.Impl.Eval;
using NReco.CF.Taste.Impl.Model;
using NReco.CF.Taste.Impl.Neighborhood;
using NReco.CF.Taste.Impl.Recommender;
using NReco.CF.Taste.Impl.Similarity;
using NReco.CF.Taste.Model;
using NReco.CF.Taste.Recommender;
using NReco.CF.Taste.Similarity;
using NReco.CF.Taste.Neighborhood;

namespace BookwormTests.Component_Tests
{
    [TestFixture]
    public class RecommenderSystemComponentTesting
    {
        private const string connectionString = @"Data Source=(LocalDb)/MSSQLLocalDB; AttachDbFilename=C:\Dev\BookWorm\2018-ca400-lowrys5\src\Bookworm\Bookworm\App_Data\IBookwormDbContext.mdf; User Instance=True; Integrated Security=True";
        private const int NUM_RECOMMENDATIONS = 10;
        private IRepository<Book> _bookRepository;
        private IRecommenderService _recommenderService;
        private string queryString =
            "SELECT UserId, BookId, Rating FROM dbo.UserBookReviews;";

        private RecommenderController _recommenderController;
        private IProfileService _profileService;
        private IRepository<User> _userRepository;
        private IRepository<Connection> _connectionRepository;
        private String controllerName = "Profiles";
        private String actionCalledOtherUsersProfile = "OtherUsersProfile";
        private static ContainerBuilder _builder { get; set; }
        private static Autofac.IContainer Container { get; set; }



        [SetUp]
        public void SetUp()
        {

            _builder = new ContainerBuilder();
            {
                _builder.RegisterType<ProfileService>().As<IProfileService>();
                _builder.RegisterControllers(typeof(MvcApplication).Assembly);

                // DB registration
                _builder.RegisterType<BookwormDbContext>().AsSelf().As<IBookwormDbContext>();

                // Unit of Work registration            
                _builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));

                // Services registration
                _builder.RegisterType<SignUpService>().As<ISignUpService>();
                _builder.RegisterType<LoginService>().As<ILoginService>();
                _builder.RegisterType<SearchService>().As<ISearchService>();
                _builder.RegisterType<BookService>().As<IBookService>();
                _builder.RegisterType<RecommenderService>().As<IRecommenderService>();

                _builder.RegisterType<SignUpService>().UsingConstructor(typeof(IRepository<User>));
                _builder.RegisterType<BookService>().UsingConstructor(typeof(IRepository<Book>),
                    typeof(IRepository<UserBookReview>), typeof(IRepository<ToRead>));
                _builder.RegisterType<ProfileService>().UsingConstructor(typeof(IRepository<User>),
                    typeof(IRepository<Connection>));
                _builder.RegisterType<RecommenderService>().UsingConstructor(typeof(IRepository<Book>),
                    typeof(IRepository<UserBookReview>));

                _builder.RegisterType<BookwormDbContext>().As<IBookwormDbContext>();
            }

            Container = _builder.Build();
        }


        [Test]
        public void NRecoTest()
        {

            using (var scope = Container.BeginLifetimeScope())
            {
                _recommenderService = scope.Resolve<RecommenderService>();

                var results = _recommenderService.GetNNearestNeighborsUsersRecommendations(7, 23612);
            }
        }
      
        [Test]
        public void GivenTwoUsersWithIdenticalReviews_When_NRecoNNearestNeighbourCalled_ReturnTheSameNeighbours()
        {

            using (var scope = Container.BeginLifetimeScope())
            {
                _recommenderService = scope.Resolve<RecommenderService>();

                var results = _recommenderService.GetNNearestNeighborsUsersRecommendations(5, 53428);
                var results2 = _recommenderService.GetNNearestNeighborsUsersRecommendations(5, 53429);

                Assert.AreEqual(results, results2);
            }
        } // 173, 535, 2029, 2171, 3567


        [Test]
        public void GivenTwoUsersWithDifferentReviews_When_NRecoNNearestNeighbourCalled_ReturnDifferentNeighbours()
        {

            using (var scope = Container.BeginLifetimeScope())
            {
                _recommenderService = scope.Resolve<RecommenderService>();


                var results = _recommenderService.GetNNearestNeighborsUsersRecommendations(5, 53428);
                var results2 = _recommenderService.GetNNearestNeighborsUsersRecommendations(5, 4536);

                Assert.AreNotEqual(results, results2);
            }
        }


        [Test]
        public void GivenSameUser_When_NRecoNNearestNeighbourCalled_5Times_WithSameId_ReturnTheSameNeighbours()
        {

            using (var scope = Container.BeginLifetimeScope())
            {
                _recommenderService = scope.Resolve<RecommenderService>();

                var results = _recommenderService.GetNNearestNeighborsUsersRecommendations(5, 53428);
                var results2 = _recommenderService.GetNNearestNeighborsUsersRecommendations(5, 53428);
                var results3 = _recommenderService.GetNNearestNeighborsUsersRecommendations(5, 53428);

                Assert.AreEqual(results, results2);
                Assert.AreEqual(results, results3);
            }
        }


        [Test]
        public void NNearestNeighbour_Evaluation_Using_AverageAbsoluteDifferenceMetric()
        {
            RandomUtils.useTestSeed();
            AverageAbsoluteDifferenceRecommenderEvaluator evaluator = new AverageAbsoluteDifferenceRecommenderEvaluator();

            IRecommenderBuilder pearsonRecommenderBuilder = new TestRecommenderBuilderPearson();
            IRecommenderBuilder euclideanRecommenderBuilder = new TestRecommenderBuilderEuclidean();
            IRecommenderBuilder logLikelyRecommenderBuilder = new TestRecommenderBuilderLogLikelihood();
            IRecommenderBuilder spearmanRecommenderBuilder = new TestRecommenderBuilderSpearman();
            IRecommenderBuilder tanimotoRecommenderBuilder = new TestRecommenderBuilderTanimoto();

            // Average Absolute Difference For the recommendations based on metric applied
            // Common metric used for assessing the accuracy of the recommendations.
            // The lower the result the better the algorithm.
            // Training Data: 70% 
            // Test Data: 30%
            using (var scope = Container.BeginLifetimeScope())
            {
                _recommenderService = scope.Resolve<RecommenderService>();
                GenericDataModel model = _recommenderService.GetUserBasedDataModel();

                double score = evaluator.Evaluate(pearsonRecommenderBuilder
                    , null, model, 0.7, 1.0);
                double scoreEuclid = evaluator.Evaluate(euclideanRecommenderBuilder
                    , null, model, 0.7, 1.0);
                double scoreLoglikely = evaluator.Evaluate(logLikelyRecommenderBuilder
                    , null, model, 0.7, 1.0);
                double scoreSpearman = evaluator.Evaluate(spearmanRecommenderBuilder
                    , null, model, 0.7, 1.0);
                double scoreTanimoto = evaluator.Evaluate(tanimotoRecommenderBuilder
                    , null, model, 0.7, 1.0);
            }
        }


        [Test]
        public void NNearestNeighbour_Evaluation_Using_RootMeanSquaredMetric()
        {
            RandomUtils.useTestSeed();
            RMSRecommenderEvaluator evaluator = new RMSRecommenderEvaluator();

            IRecommenderBuilder pearsonRecommenderBuilder = new TestRecommenderBuilderPearson();
            IRecommenderBuilder euclideanRecommenderBuilder = new TestRecommenderBuilderEuclidean();
            IRecommenderBuilder logLikelyRecommenderBuilder = new TestRecommenderBuilderLogLikelihood();
            IRecommenderBuilder spearmanRecommenderBuilder = new TestRecommenderBuilderSpearman();
            IRecommenderBuilder tanimotoRecommenderBuilder = new TestRecommenderBuilderTanimoto();

            // Root mean square metric applied to the recommendations based on the algorithms below.
            // Common metric used for assessing the accuracy of the recommendations.
            // The lower the result the better the algorithm.
            // Training Data: 70% 
            // Test Data: 30%
            using (var scope = Container.BeginLifetimeScope())
            {
                _recommenderService = scope.Resolve<RecommenderService>();
                GenericDataModel model = _recommenderService.GetUserBasedDataModel();

                double score = evaluator.Evaluate(pearsonRecommenderBuilder
                    , null, model, 0.7, 1.0);
                double scoreEuclid = evaluator.Evaluate(euclideanRecommenderBuilder
                    , null, model, 0.7, 1.0);
                double scoreLoglikely = evaluator.Evaluate(logLikelyRecommenderBuilder
                    , null, model, 0.7, 1.0);
                double scoreSpearman = evaluator.Evaluate(spearmanRecommenderBuilder
                    , null, model, 0.7, 1.0);
                double scoreTanimoto = evaluator.Evaluate(tanimotoRecommenderBuilder
                    , null, model, 0.7, 1.0);
            }
        }


        public class TestRecommenderBuilderEuclidean : IRecommenderBuilder
        {
            public IRecommender BuildRecommender(IDataModel model)
            {
                IUserSimilarity similarity = new EuclideanDistanceSimilarity(model);
                IUserNeighborhood neighborhood =
                    new NearestNUserNeighborhood(10, similarity, model);
                return
                    new GenericUserBasedRecommender(model, neighborhood, similarity);
            }
        };


        public class TestRecommenderBuilderPearson : IRecommenderBuilder
        {
            public IRecommender BuildRecommender(IDataModel model)
            {
                IUserSimilarity similarity = new PearsonCorrelationSimilarity(model);
                IUserNeighborhood neighborhood =
                    new NearestNUserNeighborhood(10, similarity, model);
                return
                    new GenericUserBasedRecommender(model, neighborhood, similarity);
            }
        };


        public class TestRecommenderBuilderSpearman : IRecommenderBuilder
        {
            public IRecommender BuildRecommender(IDataModel model)
            {
                IUserSimilarity similarity = new SpearmanCorrelationSimilarity(model);
                IUserNeighborhood neighborhood =
                    new NearestNUserNeighborhood(10, similarity, model);
                return
                    new GenericUserBasedRecommender(model, neighborhood, similarity);
            }
        };


        public class TestRecommenderBuilderLogLikelihood : IRecommenderBuilder
        {
            public IRecommender BuildRecommender(IDataModel model)
            {
                IUserSimilarity similarity = new LogLikelihoodSimilarity(model);
                IUserNeighborhood neighborhood =
                    new NearestNUserNeighborhood(10, similarity, model);
                return
                    new GenericUserBasedRecommender(model, neighborhood, similarity);
            }
        };


        public class TestRecommenderBuilderTanimoto : IRecommenderBuilder
        {
            public IRecommender BuildRecommender(IDataModel model)
            {
                IUserSimilarity similarity = new TanimotoCoefficientSimilarity(model);
                IUserNeighborhood neighborhood =
                    new NearestNUserNeighborhood(10, similarity, model);
                return
                    new GenericUserBasedRecommender(model, neighborhood, similarity);
            }
        };
    }
}