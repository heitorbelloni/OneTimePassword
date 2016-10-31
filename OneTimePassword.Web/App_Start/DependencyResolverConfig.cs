using System.Reflection;
using System.Web.Mvc;
using OneTimePassword.Core;
using OneTimePassword.Web.Infrastructure;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

namespace OneTimePassword.Web {
    public class DependencyResolverConfig {
        public static void SetResolver() {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            container.RegisterMvcIntegratedFilterProvider();

            RegisterDependencies(container);

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static void RegisterDependencies(Container container) {
            container.Register<IOneTimePasswordConfiguration, OneTimePasswordConfiguration>();
            container.Register<IOneTimePasswordGenerator, OneTimePasswordGenerator>(SimpleInjectorConstructorSelector.LeastParameters);
        }
    }
}