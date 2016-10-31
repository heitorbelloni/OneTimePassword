using System;

namespace OneTimePassword.Core {
    public interface IOneTimePasswordConfiguration {
        string GetSecrectKey();
        DateTime GetEpoch();
        int GetIntervalInSeconds();
        int GetTokenLenght();
    }
}
