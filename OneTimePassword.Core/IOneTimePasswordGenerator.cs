namespace OneTimePassword.Core {
    public interface IOneTimePasswordGenerator {
        string Generate(string userId);
    }
}
