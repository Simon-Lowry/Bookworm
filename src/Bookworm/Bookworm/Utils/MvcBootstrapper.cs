using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Integration.Mvc;
using System.Web.Mvc;
using Bookworm.Controllers;
using Bookworm.Validators;
using Bookworm.Contracts;
using FluentValidation;
using Bookworm.Contracts.Services;
using Bookworm.Services;
using Bookworm.Repository;
using Bookworm.Data;
using Bookworm.Models;
using System.Data.Entity;

namespace Bookworm.Utils
{
    public static class MvcBootstrapper
    {
        public static ContainerBuilder Builder { get; private set; }
        public static IContainer Container { get; private set; }

        public static void Init()
        {
            Builder = new ContainerBuilder();

            // DB registration
            Builder.RegisterType<BookwormDbContext>().AsSelf().As<IBookwormDbContext>();

            // Unit of Work registration            
            Builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerDependency();
       
            // Services registration
            Builder.RegisterType<SignUpService>().As<ISignUpService>().InstancePerRequest();
            Builder.RegisterType<ProfileService>().As<IProfileService>().InstancePerRequest();
            Builder.RegisterType<LoginService>().As<ILoginService>().InstancePerRequest();
            Builder.RegisterType<SearchService>().As<ISearchService>().InstancePerRequest();
            Builder.RegisterType<BookService>().As<IBookService>().InstancePerRequest();
            Builder.RegisterType<RecommenderService>().As<IRecommenderService>().InstancePerRequest();

            Builder.RegisterType<SignUpService>().UsingConstructor(typeof(IRepository<User>));
            Builder.RegisterType<BookService>().UsingConstructor(typeof(IRepository<Book>),
                 typeof(IRepository<UserBookReview>), typeof(IRepository<ToRead>));
            Builder.RegisterType<ProfileService>().UsingConstructor(typeof(IRepository<User>),
                typeof(IRepository<Connection>));
            Builder.RegisterType<BookwormDbContext>().As<IBookwormDbContext>();
            Builder.RegisterControllers(typeof(MvcApplication).Assembly).InstancePerRequest();
            Container = Builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));
        }
    }
}