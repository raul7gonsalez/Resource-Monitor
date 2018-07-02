namespace ResourceMonitor.Jobs
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Quartz;

    using ResourceMonitor.Interfaces;

    public class ResourceAvailabilityCheckJob : IJob
    {
        private readonly IResourceRepository _resourceRepository;

        public ResourceAvailabilityCheckJob(IResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var resources = _resourceRepository.Resources.ToArray();

            foreach (var resource in resources)
            {
                var client = new HttpClient();
                try
                {
                    var result = await client.GetAsync(resource.HostAddress);

                    resource.Availability = result.IsSuccessStatusCode
                                                ? "Yes"
                                                : "No";
                    resource.StatusCode = result.StatusCode;
                    _resourceRepository.Update(resource);
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
                                _resourceRepository.Update(resource);
                            }
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    resource.StatusCode = HttpStatusCode.ServiceUnavailable;
                    resource.Availability = "False";
                    _resourceRepository.Update(resource);
                }
                catch (WebException e)
                {
                    resource.StatusCode = HttpStatusCode.ServiceUnavailable;
                    resource.Availability = "False";
                    _resourceRepository.Update(resource);
                }
                finally
                {
                    client.Dispose();
                }
            }
        }
    }
}