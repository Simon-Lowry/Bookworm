# Blog: Bookworm

**Simon Lowry**


## Semester 1

## My First Blog Entry

I tried to come up with ideas that would be appropriate for my 4th year project. I wanted to do something with web applications as I'd done a lot of work with web applications on the INTRA work placement, I really enjoyed it and I want to work more with them in the future after graduation. I've taken note of a lot of the tools that were used in the development of the web applications from my INTRA placement and want to incorporate them into whatever system I end up developing. Some of these tools I learned about and used were: entity framework, autofixture, FluentValidator, NUnit, FakeItEasy. I want to also make use of some of the techniques i've learned about as well using the MVC (Model, View, Controller) structure to make a maintainable system of good quality and has a separation of concerns which comes as part of the MVC structure. I really like this way of constructing web applications, it simplifies the creation of the web application and it makes it very clear as to the responsibilities of each part of the system providing the developer with the ability to focus on part almost completely independent of another's functionality. I also plan on incorporating the testing knowledge I acquired and make use of the Triple A process of writing tests as well as making use of number of techniques to create quality automated testing.


## My Second Blog Entry

During the summer I made the bulk of a program which takes the highlights file from a kindle device and parses all of the highlights into separate books. This is something I'd like to incorporate into my system. My thinking is to have this as part of a book reviewing web application. I've taken a lot of time to understand the most popular book review website out there, goodreads.com and in terms of the UI for starters, I believe I can do a better job than they have. It suffers from a lack of distinction between sections, links are all over the place. it's trying to do too much and it's not abundantly clear how to do what you want to do as a user of the system. It doesn't adhere to many of the good UI practices of Schneiderman or Neilsson heuristics that we've learned about in this course. I also planned on using a data set from the website Kaggle in order to assist in the development, testing and demonstration of the system. 

[Kaggle Data Set](https://www.kaggle.com/zygmunt/goodbooks-10k/data)


## My Third Blog Entry

I met with Darragh O'Briain in order to discuss ideas for my 4th project and to ask him to be my supervisor. He accepted the request and told me. He questioned why and how I would implement integrate the kindle highlights part in the project. He wanted me to focus primarily on the recommender system as this is what would be better received in what I had suggested to build. He also gave me advice on what to put in the project proposal.  I took this on board and decided to make the web application Bookworm. 


## My Fourth Blog Entry

Project proposal evaluation: In order to have our projects given the green light to go ahead and make them, we have to first get approval from the project evaluation meeting with two of the lecturers in the college. I had this meeting recently and truthfully I presented the system poorly so that the lecturers were firstly unclear as to what the primary function of the system was and whether or not I was creating two big systems in one. They also wanted me to get a much better understanding of how I wanted to implement the recommender system and how this would be evaluated. They temporarily failed the green-lighting of the project so I could get a better understanding of what I wanted in the system and present this to my supervisor for further evaluation.


## My Fifth Blog Entry

After hearing this from the lecturers I beefed up my understanding of how I wanted to implement the recommender system in Bookworm and decided to only include the kindle highlight section as bonus features if time allowed for their creation . A core part of the system would be the recommender system but I also intended on putting a great emphasis on software practices and how the architecture was structured and what external tools could enhance the capabilities of the system itself. I sent a newly drafted project proposal to darragh and he green lit the project after some consultation with the module co-ordinator. 


## My Sixth Blog Entry

I intended on using a five layer system architecture which would have MVC (Model, View, Controller), the remaining two layers would be made up of the service layer and the repository layer, these would handle the database operations. The repository would have a set of generic operations which would be used in multiple parts of the system and it's inclusion would lead to a reduction in code duplication and allow for independent service usage of the repository with entity framework. With this mind, I began to initially work on the system using Visual Studio setting up the template MVC structure within the project. I also created a testing project as well which would contain all the automated testing (unit testing, integration testing and component testing.

At this point I spent a good bit of time setting up the repository layer and also creating all of the models and entities which would be mapped together in a code first entity framework approach to creating the database.


Here's the code for what the repository ended up like:
```C#
namespace Bookworm.Repository
{
    public class Repository<TEntity> : IRepository <TEntity> where TEntity : class
    {
        protected BookwormDbContext DbContext { get; set; }
        private DbSet<TEntity> Entities;
      
        public Repository(BookwormDbContext dbContext)
        {
            DbContext = dbContext;
            Entities = dbContext.Set<TEntity>();
        }


        public virtual TEntity Get(int id) 
        {
            return Entities.Find(id);
        }


        public virtual IEnumerable<TEntity> GetListOf()
        {
            return Entities.ToList();
        }


        public virtual bool Create(TEntity entity)  
        {
            Entities.Add(entity);
            DbContext.SaveChanges();
            return true;
        }


        public virtual void Update(TEntity entity)

        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

           
            Entities.Add(entity);
          //  DbContext.SaveChanges();
            
        }


        public virtual bool Delete(TEntity entity)
        {
            DbEntityEntry dbEntityEntry = this.DbContext.Entry<TEntity>(entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {

                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
               
                Entities.Attach(entity);
                Entities.Remove(entity);
                dbEntityEntry.State = EntityState.Deleted;
            }
            DbContext.SaveChanges();
            return true;
        }

       
        public virtual bool IsPresentInTable(TEntity entity)
        {
            return (DbContext.Set<TEntity>().Attach(entity) != null) ? true : false;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return Entities.AsEnumerable();
        }
    }
}
```

## My Seventh Blog Entry - Setting up the database 

This took almost a week to get right due to linking with sql server and my lack of understanding of this tool and having a number of other assignments to be working on. In order to use a database in I had to create a localdb instance first after installing a number of sql server components, I had to configure firewall settings, configure settings in those sql server components, create a local instance, ensure the services for sql server were running correctly, ensuring the connection string was pointing to correct instance of the local DB in visual studio. This was very time consuming, frustrating and took many crates of tea and visits to stackoverlow to get exactly right, eventually I got there. Ideally I would never like to do this again :) (It was something i hadn't encountered in my placement as these tools were configured for me) 


## My Eight Blog Entry - Getting the Kaggle Data Set into DB

This was another big problem as the kaggle data set couldn't be imported into Sql server management studio (SSMS) as it contained a number of characters which couldn't be read by SSMS. It wasn't a problem which would could be handedly manually (although i did try that) since there was thousands of books in the data set. This required some data cleaning and for me to write a script in Java which would read in the data (which were in csv files), filter out any characters which wouldn't be read by the system and tidy up any missing data and incorrect data type issues and present the results in separate csv files for the books csv, the reviews csv. The kaggle data set also only had user ids and not any information about those users for privacy reasons so i also generated fake users for each of these ids and created a users csv file for the database. All of these issues were eventually fixed and the database was setup with a combination of the modified kaggle dataset and entity framework based code to create the tables which would contain that data.


## My Ninth Blog Entry
At this point I started working on getting some basic mvc in place for a few of the main functions and their accompanying service which would take care of their database operations. This was done for the home screen, the sign up screen and for the login screen. Each of which would have separate controllers, views (the interface), and services which would also be unit tested and integration tested to ensure they are performing correctly. I made use of Fluent Validator to assess the sign up form data, including make using of it's password functionality and email functionality to create several rules which would make sure that the data was in the correct form entered by the user. It also would present the user with any errors that resulted from that validation. The UI (in the form of the views) for both the sign up screen and the login was completed but the home screen was partially completed and the navbar for all three of these views needed work.  A lot of my time at this point was spent between studying for exams and assignments over the christmas and in the exam period. Less work was done on the project during this period.


Sign up validator - ensuring the sign up form was entered correctly:
```C#
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
```



## My Tenth Blog Entry - The Recommender System

Our module in the first semester on data mining gave me some insight and allowed me the opportunity to create a basic one myself to get better understanding of how one would operate. However, I decided early on to go with MyMediaLite as opposed to going and creating my own which as my supervisor pointed out would not be what a developer would do in a production system while working. Having researched various recommender systems as well as some research papers which demonstrated results from their usage this showed to be the best option, although, in truth, in C# (the language I'm developing the recommender system in) there is a limited number of free open sourced options when it comes to recommender system tools. 

I then went on to develop some implementations using this tool and found it very difficult to use and validate. Even with the source code available, it was difficult to understand what each implementation of the recommender algorithms was doing due to the way the subclasses were extended in the hierarchical structure of the tool. I couldn't make sense of how they were implementing the various algorithms despite researching the algorithms themselves. There was also a shortage of documentation which made debugging the system very difficult, it was hard to trace where errors were coming from, how the system was evaluating the recommendations and also when validating it's output it failed to produce consistent output for users with the same profiles, as a result, something was going wrong but I couldn't figure out what. After spending a considerable amount of time on this I decided to explore different options.

I came across a tool called NReco which is an Apache Mahout port from Java. Apache Mahout is extensively used for recommender systems. It has a number of books which include it's implementation and how to use it, what works best with it, how to evaluate its results, research papers on it's effectiveness and the effectiveness of it's components in isolation. It's a highly valued tool which has the validity of all this extensive usage and research to backup it's quality. It's implementation in C# contained essentially the same information and algorithms. NReco's code is easy to understand and it's set-up isn't too complicated. There are a number of algorithms used in NReco and the one I chose to use was the User Based Nearest Neighbour algorithm. The  nearest neighbour collaborative filtering algorithms were among the earliest algorithms developed for collaborative filtering. The goal of collaborative filtering is to predict how well a user will like an item that he has not rated given a set of historical preference judgements for a group of users. 

The label user based is  inaccurate though, as any recommender algorithm is based on user- and item-related data. The defining characteristic of a user-based recommender algorithm is that it’s based upon some notion of similarities between users. These algorithms are based on the idea that similar users show similar patterns of rating behaviour.   

The algorithm goes as follows for the user based nearest neighbour algorithm:

```C#
for every item i that u has no preference for yet
	for every other user v that has a preference for i
		compute a similarity s between u and v
		incorporate v's preference for i, weighted by s, into a running average
return the top items, ranked by weighted average
```
The outer loop simply considers every known item that the user hasn’t already expressed a rating for as a possiblity for recommendation. The inner loop goes to any other user who has given a rating for this candidate item, and sees what their rating value for it was. In the end, the values are averaged to come up with an estimate—a weighted average, that is. Each rating value is weighted in the average by how similar that user is to the target user. The more similar a user, the more heavily their rating is weighted.

Another important part of user-based recommenders is the UserSimilarity implementation. A user-based recommender relies most of all on this component so as a result of this I spent a good lot of time evaluating the performance of this  using a number of methods available with the system:
	- pearson coefficient
	- euclidean distance
	- log likelihood 
	- spearman
	- tanimoto

Test evaluating the Average Absolute Difference Metric:
```C#
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
```


Test evaluating the Root Mean Squared Metric:
```C#
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
```


The results concluded the same thing that I've found in research papers that the euclidean distance performs the best. I've included a document with the testing results in the them in the link below:


[Recommender System Test Results](https://docs.google.com/document/d/1UYLX2rmVA0T_3jvJlDReDxY4PHTHUx_nzZG4Gu8Y5zw/edit)


## My Eleventh Blog Entry

Create partial views for accordions on the profiles. This required resign of a couple of the controller actions in the profiles controller. The system had no way of displaying book images for example with the book review accordion and this required me to tie the book objects with the profile view model. This was the same both for connections and to read accordions. It required the images to be also present in the view model. The book service was also updated accordingly to retrieve these sets of books based on the user book reviews. Also added to profiles services in order to retrieve all of a connections profile information for displaying on a user’s profile. 

Spent some time assessing how much of the unit tests needed repair given updated code. Added integration tests for LoginController and ProfilesController.


Added bootstrap alerts which fade in and fade out of the screen when a review has been created or edited. They notify the user as to the results of their action prior to the corresponding button being changed when the event has transpired, e.g. create review button changing to edit review after successful creation of review in database.

Made class diagrams for technical specifcation using Microsoft Visio. Made up Use Cases for technical specification. 

Add views all book reviews, all connections and all books to read.



## My Twelfth Blog Entry - Testing Results
My testing results for Bookworm are stored in the following file link:
[Bookworm Testing](https://drive.google.com/open?id=1vDJp6Zu-iK2mnVznSkVRUCELu7EWR5lLS-ATG9hud3k)

Overall there was 81 automated tests built for the system, these included unit tests, integration tests and component tests. The automated code for this is included in the testing project of the solution. The document in the link above shows the screenshots of those tests successfully passing in visual studio.

