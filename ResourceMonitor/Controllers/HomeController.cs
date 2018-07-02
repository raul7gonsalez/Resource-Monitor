namespace ResourceMonitor.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Sockets;
    using System.Web.Mvc;

    using ResourceMonitor.Interfaces;

    /// <summary>Контроллер стартовой страницы</summary>
    public class HomeController : Controller
    {
        public IResourceRepository ResourceRepository { get; set; }

        /// <summary>Вывести данные о доступности ресурсов</summary>
        public ActionResult ListResources()
        {
            var resources = ResourceRepository.Resources.ToArray();

            foreach (var resource in resources)
            {
                var client = new HttpClient();
                try
                {
                    var httpTask = client.GetAsync(resource.HostAddress);
                    var result = httpTask.Result;
                    resource.Availability = result.IsSuccessStatusCode
                                                ? "Yes"
                                                : "No";
                    resource.StatusCode = result.StatusCode;
                    ResourceRepository.Update(resource);
                }
                catch (AggregateException e)
                {
                    if (e.InnerExceptions != null)
                    {
                        foreach (var innerExc in e.InnerExceptions)
                        {
                            if (innerExc is HttpRequestException || innerExc is WebException)
                            {
                                resource.StatusCode = HttpStatusCode.ServiceUnavailable;
                                resource.Availability = "False";
                                ResourceRepository.Update(resource);
                            }
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    resource.StatusCode = HttpStatusCode.ServiceUnavailable;
                    resource.Availability = "False";
                    ResourceRepository.Update(resource);
                }
                catch (WebException e)
                {
                    resource.StatusCode = HttpStatusCode.ServiceUnavailable;
                    resource.Availability = "False";
                    ResourceRepository.Update(resource);
                }
                finally
                {
                    client.Dispose();
                }
            }

            return View(resources);
        }
    }
}