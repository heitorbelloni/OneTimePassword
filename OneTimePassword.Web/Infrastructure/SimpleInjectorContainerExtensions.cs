using System.Linq;
using System.Linq.Expressions;
using SimpleInjector;

namespace OneTimePassword.Web {
    //https://www.cuttingedge.it/blogs/steven/pivot/entry.php?id=88
    public static class SimpleInjectorContainerExtensions {
        public static void Register<TService, TImplementation>(this Container container, IConstructorSelector selector) where TService : class {
            container.Register<TService>(() => null);

            container.ExpressionBuilt += (sender, e) => {
                if (e.RegisteredServiceType == typeof(TService)) {
                    var ctor =
                        selector.GetConstructor(typeof(TImplementation));

                    var parameters = ctor.GetParameters()
                                         .Select(p => container.GetRegistration(p.ParameterType, true).BuildExpression());

                    e.Expression = Expression.New(ctor, parameters);
                }
            };
        }
    }
}
