namespace ResourceMonitor.ControllerFactory
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    using ResourceMonitor.Authentification;
    using ResourceMonitor.Authentification.Interfaces;
    using ResourceMonitor.Interfaces;
    using ResourceMonitor.Repositories;

    public class CaslteConrollerFactory : DefaultControllerFactory
    {
        public CaslteConrollerFactory()
        {
            // создание контейнера
            Container = new WindsorContainer();
            AddBindings();
        }

        public IWindsorContainer Container { get; }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                return null;

            return Container.Resolve(controllerType) as IController;
        }

        public override void ReleaseController(IController controller)
        {
            var disposableController = controller as IDisposable;
            disposableController?.Dispose();

            Container.Release(controller);
        }

        private void AddBindings()
        {
            var controllers = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.BaseType == typeof(Controller))
                .ToList();

            foreach (var controller in controllers)
            {
                Container.Register(Component.For(controller).LifestylePerWebRequest());
            }

            Container.Register(Component.For<IResourceRepository>().ImplementedBy<ResourceRepository>());
            Container.Register(Component.For<IAuthProvider>().ImplementedBy<FormsAuthProvider>());
        }
    }
}