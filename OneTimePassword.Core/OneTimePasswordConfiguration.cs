using System;

namespace OneTimePassword.Core {
    public class OneTimePasswordConfiguration : IOneTimePasswordConfiguration {
        public string GetSecrectKey() {
            return "5e3dc25b-9a7f-4b28-80a8-973f95ae17b1";
        }

        public DateTime GetEpoch() {
            return DateTime.MinValue;
        }

        public int GetIntervalInSeconds() {
            return 30;
        }

        public int GetTokenLenght() {
            return 6;
        }
    }
}