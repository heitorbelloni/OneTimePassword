using System;
using NUnit.Framework;
using OneTimePassword.Core;

namespace OneTimePassword.Tests.Core {
    public class OneTimePasswordGeneratorTests {
        private DateTime _utcNow;
        private OneTimePasswordGenerator _defaultGenerator;
        private OneTimePasswordGenerator _customGenerator;

        [SetUp]
        public void Setup() {
            _utcNow = new DateTime(2016, 2, 10, 15, 0, 0);
            SystemTime.UtcNow = () => _utcNow;

            _defaultGenerator = new OneTimePasswordGenerator();
            _customGenerator = new OneTimePasswordGenerator("secretKey", DateTime.MinValue, 40, 8);
        }

        [Test]
        public void Should_generate_same_token_when_within_default_interval_of_30_seconds() {
            var formerToken = _defaultGenerator.Generate("userId");

            SystemTime.UtcNow = () => _utcNow.AddSeconds(15);
            var laterToken = _defaultGenerator.Generate("userId");

            Assert.AreEqual(formerToken, laterToken);
        }

        [Test]
        public void Should_generate_same_token_when_within_custom_interval() {
            var formerToken = _customGenerator.Generate("userId");

            SystemTime.UtcNow = () => _utcNow.AddSeconds(35);
            var laterToken = _customGenerator.Generate("userId");

            Assert.AreEqual(formerToken, laterToken);
        }

        [Test]
        public void Should_generate_different_token_when_after_default_interval_of_30_seconds() {
            var formerToken = _defaultGenerator.Generate("userId");

            SystemTime.UtcNow = () => _utcNow.AddSeconds(35);
            var laterToken = _defaultGenerator.Generate("userId");

            Assert.AreNotEqual(formerToken, laterToken);
        }

        [Test]
        public void Should_generate_different_token_when_after_custom_interval() {
            var formerToken = _customGenerator.Generate("userId");

            SystemTime.UtcNow = () => _utcNow.AddSeconds(45);
            var laterToken = _customGenerator.Generate("userId");

            Assert.AreNotEqual(formerToken, laterToken);
        }

        [Test]
        public void Should_generate_token_with_default_lenght_of_6_characters() {
            var token = _defaultGenerator.Generate("userId");
            Assert.AreEqual(6, token.Length);
        }

        [Test]
        public void Should_generate_token_with_custom_lenght() {
            var token = _customGenerator.Generate("userId");
            Assert.AreEqual(8, token.Length);
        }

        [Test]
        public void Should_generate_different_tokens_for_different_user_ids() {
            var token = _defaultGenerator.Generate("userId");
            var otherToken = _defaultGenerator.Generate("otherUserId");
            Assert.AreNotEqual(token, otherToken);
        }
    }
}
