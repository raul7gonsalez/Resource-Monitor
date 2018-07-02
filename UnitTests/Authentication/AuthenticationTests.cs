namespace UnitTests.Authentication
{
    using System.Web.Mvc;

    using AutoFixture;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using ResourceMonitor.Authentification.Interfaces;
    using ResourceMonitor.Controllers;
    using ResourceMonitor.Models;

    /// <summary>ТЕсты аутентификации</summary>
    [TestClass]
    public class AuthenticationTests
    {
        /// <summary>Успешная аутентификаци</summary>
        [TestMethod]
        public void SuccessAuthenticate()
        {
            const string Login = "admin";
            const string Password = "admin";
            const string ReturnedUrl = "/Admin/List";

            var mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate(Login, Password)).Returns(true);

            var fixture = new Fixture();

            var model = fixture.Build<LoginModel>()
                               .With(m => m.UserName, Login)
                               .With(m => m.Password, Password)
                               .Create();

            var controller = new AccountController
                             {
                                 AuthProvider = mock.Object
                             };

            var result = controller.Login(model, ReturnedUrl);

            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual(ReturnedUrl, ((RedirectResult)result).Url);
        }
    }
}