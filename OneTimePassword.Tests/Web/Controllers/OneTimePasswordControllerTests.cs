using System.Net;
using System.Web;
using Moq;
using NUnit.Framework;
using OneTimePassword.Core;
using OneTimePassword.Web.Controllers;

namespace OneTimePassword.Tests.Web.Controllers {
    public class OneTimePasswordControllerTests {
        private IOneTimePasswordGenerator _generator;
        private OneTimePasswordController _controller;

        [SetUp]
        public void Setup() {
            _generator = Mock.Of<IOneTimePasswordGenerator>();
            _controller = new OneTimePasswordController(_generator);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void Generate_should_throw_http_exception_with_bad_request_status_code_when_user_id_is_null_or_white_space(string userId) {
            var result = Assert.Throws<HttpException>(() => _controller.Generate(userId));
            Assert.AreEqual((int) HttpStatusCode.BadRequest, result.GetHttpCode());
            Assert.AreEqual("\"User ID\" is required.", result.Message);
        }

        [Test]
        public void Generate_should_call_generator_to_get_a_token() {
            _controller.Generate("userId");
            Mock.Get(_generator).Verify(g => g.Generate(It.Is<string>(userId => userId.Equals("userId"))), Times.Once);
        }

        [Test]
        public void Generate_should_return_a_token() {
            Mock.Get(_generator).Setup(g => g.Generate(It.IsAny<string>())).Returns("token");
            var result = _controller.Generate("userId");
            Assert.AreEqual("token", result.Data);
        }
    }
}