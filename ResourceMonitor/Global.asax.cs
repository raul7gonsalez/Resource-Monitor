namespace ResourceMonitor
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using ResourceMonitor.Binders;
    using ResourceMonitor.ControllerFactory;
    using ResourceMonitor.Entities;
    using ResourceMonitor.Jobs;

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ControllerBuilder.Current.SetControllerFactory(new CaslteConrollerFactory());
            ModelBinders.Binders.Add(typeof(Resource), new ResourceBinder());
            ResourceAvailabilityCheckScheduler.Start();
        }
    }
}