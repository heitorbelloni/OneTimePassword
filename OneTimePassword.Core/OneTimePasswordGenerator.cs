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
        
        public OneTimePasswordGenerator(IOneTimePasswordConfiguration oneTimePasswordConfiguration) {
            _secretKey = oneTimePasswordConfiguration.GetSecrectKey();
            _epoch = oneTimePasswordConfiguration.GetEpoch();
            _intervalInSeconds = oneTimePasswordConfiguration.GetIntervalInSeconds();
            _tokenLenght = oneTimePasswordConfiguration.GetTokenLenght();
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
            //http://tools.ietf.org/html/rfc4226#page-7
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