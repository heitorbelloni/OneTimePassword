using System;
using Moq;
using NUnit.Framework;
using OneTimePassword.Core;

namespace OneTimePassword.Tests.Core {
    public class OneTimePasswordGeneratorTests {
        private DateTime _utcNow;
        private string _secretKey;
        private DateTime _epoch;
        private int _intervalInSeconds;
        private int _tokenLenght;
        private string _userId;
        private Mock<IOneTimePasswordConfiguration> _configurationMock;
        private OneTimePasswordGenerator _generator;

        [SetUp]
        public void Setup() {
            _utcNow = new DateTime(2016, 2, 10, 15, 0, 0);
            SystemTime.UtcNow = () => _utcNow;

            _secretKey = "secretKey";
            _epoch = DateTime.MinValue;
            _intervalInSeconds = 30;
            _tokenLenght = 6;
            _userId = "userId";

            _configurationMock = new Mock<IOneTimePasswordConfiguration>();
            _configurationMock.Setup(c => c.GetSecrectKey()).Returns(_secretKey);
            _configurationMock.Setup(c => c.GetEpoch()).Returns(_epoch);
            _configurationMock.Setup(c => c.GetIntervalInSeconds()).Returns(_intervalInSeconds);
            _configurationMock.Setup(c => c.GetTokenLenght()).Returns(_tokenLenght);

            _generator = new OneTimePasswordGenerator(_configurationMock.Object);
        }

        [Test]
        public void Should_generate_same_token_when_within_set_interval() {
            var formerToken = _generator.Generate(_userId);

            SystemTime.UtcNow = () => _utcNow.AddSeconds(_intervalInSeconds);
            var laterToken = _generator.Generate(_userId);

            Assert.AreEqual(formerToken, laterToken);
        }



        [Test]
        public void Should_generate_different_token_when_after_set_interval() {
            var formerToken = _generator.Generate(_userId);

            SystemTime.UtcNow = () => _utcNow.AddSeconds(_intervalInSeconds + 1);
            var laterToken = _generator.Generate(_userId);

            Assert.AreNotEqual(formerToken, laterToken);
        }

        [Test]
        public void Should_generate_token_with_set_lenght() {
            var token = _generator.Generate(_userId);
            Assert.AreEqual(_tokenLenght, token.Length);
        }

        [Test]
        public void Should_generate_different_tokens_for_different_user_ids() {
            var token = _generator.Generate(_userId);
            var otherToken = _generator.Generate("otherUserId");
            Assert.AreNotEqual(token, otherToken);
        }
    }
}
