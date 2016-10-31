using System;

namespace OneTimePassword.Core {
    public class OneTimePasswordConfiguration : IOneTimePasswordConfiguration {
        public string GetSecrectKey() {
            throw new NotImplementedException();
        }

        public DateTime GetEpoch() {
            throw new NotImplementedException();
        }

        public int GetIntervalInSeconds() {
            throw new NotImplementedException();
        }

        public int GetTokenLenght() {
            throw new NotImplementedException();
        }
    }
}
