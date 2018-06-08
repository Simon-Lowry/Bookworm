using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using Bookworm.Models;

namespace BookwormTests.MockData
{
    public class MockUsers
    {
        public User FakeUser1 =  new User()
        {
            FirstName = "Bill",
            LastName = "Boyd",
            Email = "abc@gmail.com",
            Password = "bill buck",
            Country = "MyCountry9637",
            City = "MyCity9637",
            
        };

        public User FakeUser2 = new User()
        {
            FirstName = "Simon",
            LastName = "Lowry",
            Email = "abc122@gmail.com",
            Password = "mypassword",
            UserId = 123455
        };

      

        public User RealUser1 = new User()
        {
            UserId = 10,
            FirstName = "UserFirstName9637",
            LastName = "UserLastName9637",
            City = "MyCity9637",
            Country = "MyCountry9637",
            Password = "Ur12355",
            Email = "abc9637@gmail.com",
           
         };
    }
}