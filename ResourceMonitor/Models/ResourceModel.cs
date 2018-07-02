namespace ResourceMonitor.Models
{
    using System.Net;

    /// <summary> Модель ресурса </summary>
    public class ResourceModel
    {
        /// <summary> Наименование </summary>
        public string Name { get; set; }

        /// <summary> Адрес </summary>
        public string HostAddress { get; set; }

        /// <summary> Код ответа на запрос </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary> Доступность </summary>
        public string Availability { get; set; }
    }
}