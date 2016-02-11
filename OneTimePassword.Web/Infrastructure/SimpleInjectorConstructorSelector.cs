using System;
using System.Linq;
using System.Reflection;

namespace OneTimePassword.Web {
    //https://www.cuttingedge.it/blogs/steven/pivot/entry.php?id=88
    public interface IConstructorSelector {
        ConstructorInfo GetConstructor(Type type);
    }

    public class SimpleInjectorConstructorSelector : IConstructorSelector {
        public static readonly IConstructorSelector LeastParameters =
            new SimpleInjectorConstructorSelector(type => type.GetConstructors().OrderBy(c => c.GetParameters().Length).First());

        private readonly Func<Type, ConstructorInfo> _selector;

        public SimpleInjectorConstructorSelector(Func<Type, ConstructorInfo> selector) {
            _selector = selector;
        }

        public ConstructorInfo GetConstructor(Type type) {
            return _selector(type);
        }
    }
}