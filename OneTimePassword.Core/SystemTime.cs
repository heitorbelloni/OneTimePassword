using System;

namespace OneTimePassword.Core {
    //http://ayende.com/blog/3408/dealing-with-time-in-tests
    public class SystemTime {
        public static Func<DateTime> UtcNow = () => DateTime.UtcNow;
    }
}