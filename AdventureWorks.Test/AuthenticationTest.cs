using System;
using System.Collections.Generic;
using AdventureWorks.Business.Authentication;
using AdventureWorks.Business.UnitOfWork;
using AdventureWorks.Data;
using AdventureWorks.Data.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AdventureWorks.Test
{
    [TestClass]
    public class AuthenticationTest
    {
        private static AuthenticationService _service;

        [TestInitialize]
        public void Init()
        {
            int id = 1;
            var person =
                new Person
                {
                    BusinessEntityID = id,
                    rowguid = Guid.NewGuid(),
                    FirstName = "Jorge",
                    LastName = "Sanchez",
                    Password = new Password
                    {
                        BusinessEntityID = id,
                        PasswordSalt = "password@123"
                    },
                    EmailAddresses = new List<EmailAddress>
                    {
                        new EmailAddress
                        {
                            BusinessEntityID = id,
                            EmailAddress1 = "sanchez.franco@gmail.com"
                        }
                    }
                };

            var mockUnitOfWork = new Mock<IAdventureWorksUnitOfWork>();
            var mockUnitOfWorkProvider = new Mock<IUnitOfWorkProvider>();

            //We mock the return value instead of mocking the whole DBContext
            var mockPersonRepository = new Mock<IPersonRepository>();
            mockPersonRepository.Setup(r => r.GetPerson(It.IsAny<string>(), It.IsAny<string>())).Returns(person);

            mockUnitOfWork.Setup(r => r.PersonRepository).Returns(mockPersonRepository.Object);
            mockUnitOfWorkProvider.Setup(r => r.GetAdventureWorksUnitOfWork()).Returns(mockUnitOfWork.Object);
            _service = new AuthenticationService(mockUnitOfWorkProvider.Object);
        }

        [TestMethod]
        public void TestMethod1()
        {
            var validUser = _service.ValidateUserPassword("sanchez.franco@gmail.com", "password@123");
            Assert.IsNotNull(validUser);
        }
    }
}
