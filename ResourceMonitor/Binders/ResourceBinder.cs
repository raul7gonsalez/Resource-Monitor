namespace ResourceMonitor.Binders
{
    using System.Web.Mvc;

    using ResourceMonitor.Entities;

    /// <summary>Binder для ресурса</summary>
    public class ResourceBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var entityIdResult = bindingContext.ValueProvider.GetValue("Id");
            var name = (string)bindingContext.ValueProvider.GetValue("Name").ConvertTo(typeof(string));
            var hostAddress = (string)bindingContext.ValueProvider.GetValue("HostAddress").ConvertTo(typeof(string));

            var resource = new Resource
                           {
                               Name = name,
                               HostAddress = hostAddress
                           };

            if (entityIdResult != null)
            {
                resource.Id = (int)entityIdResult.ConvertTo(typeof(int));
            }
            return resource;
        }
    }
}