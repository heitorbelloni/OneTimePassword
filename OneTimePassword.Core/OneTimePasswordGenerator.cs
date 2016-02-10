using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace OneTimePassword.Core {
    public class OneTimePasswordGenerator : IOneTimePasswordGenerator {
        private readonly string _secretKey;
        private readonly DateTime _epoch;
        private readonly int _intervalInSeconds;
        private readonly int _tokenLenght;

        public OneTimePasswordGenerator() : this("5e3dc25b-9a7f-4b28-80a8-973f95ae17b1", DateTime.MinValue, 30, 6) { }

        public OneTimePasswordGenerator(string secretKey, DateTime epoch, int intervalInSeconds, int tokenLenght) {
            _secretKey = secretKey;
            _epoch = epoch;
            _intervalInSeconds = intervalInSeconds;
            _tokenLenght = tokenLenght;
        }

        public string Generate(string userId) {
            var hmac = GetHmac(userId);
            var message = CalculateMessage();
            var hash = hmac.ComputeHash(message);
            var oneTimePassword = CalculateOneTimePassword(hash);
            return oneTimePassword.PadLeft(_tokenLenght, '0');
        }

        private HMAC GetHmac(string userId) {
            var salted = String.Concat(userId, _secretKey);
            var key = Encoding.UTF8.GetBytes(salted);
            return new HMACSHA1(key);
        }

        private byte[] CalculateMessage() {
            var timeCounter = (int) (SystemTime.UtcNow().Subtract(_epoch).TotalSeconds / _intervalInSeconds);
            return Encoding.UTF8.GetBytes(timeCounter.ToString(CultureInfo.InvariantCulture));
        }

        private string CalculateOneTimePassword(byte[] hash) {
            var offset = hash[19] & 0xf;
            var binaryCode = (hash[offset] & 0x7f) << 24
                | (hash[offset + 1] & 0xff) << 16
                | (hash[offset + 2] & 0xff) << 8
                | (hash[offset + 3] & 0xff);

            var oneTimePassword = binaryCode % Math.Pow(10, _tokenLenght);
            return oneTimePassword.ToString(CultureInfo.InvariantCulture);
        }
    }
}