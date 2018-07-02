namespace ResourceAvailibilityWindowsService
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.ServiceProcess;
    using System.Timers;

    using ResourceMonitor.Interfaces;
    using ResourceMonitor.Repositories;

    public partial class ResourceAvailibility : ServiceBase
    {
        private readonly IResourceRepository _resourceRepository;

        private readonly Timer _timer;

        public ResourceAvailibility()
        {
            InitializeComponent();
            _resourceRepository = new ResourceRepository();
            _timer = new Timer();
        }

        public void TimerElapsed(object sender, ElapsedEventArgs eventArgs)
        {
            var resources = _resourceRepository.Resources.ToArray();

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

        protected override void OnStart(string[] args)
        {
            _timer.Interval = 10000;
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }

        protected override void OnStop()
        {
            _timer.Stop();
        }
    }
}