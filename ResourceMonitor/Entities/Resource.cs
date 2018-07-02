namespace ResourceMonitor.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using System.Web.Mvc;

    /// <summary> Ресурс </summary>
    public class Resource
    {
        /// <summary> Идентификатор </summary>
        public long Id { get; set; }

        /// <summary> Наименование </summary>
        [Required(ErrorMessage = "Введите имя")]
        [StringLength(200, ErrorMessage = "Максимальная длина 200 символов")]
        public string Name { get; set; }

        /// <summary> Адрес </summary>
        [Required(ErrorMessage = "Введите адрес")]
        [StringLength(200, ErrorMessage = "Максимальная длина 200 символов")]
        [RegularExpression(@"(https?:\/\/)([a-zA-Z0-9-_]+)(\.[a-zA-Z0-9-_]+)*\.([a-z]{2,6})", ErrorMessage = "Некорректный адрес")]
        public string HostAddress { get; set; }

        /// <summary> Код ответа на запрос </summary>
        [HiddenInput(DisplayValue = false)]
        public HttpStatusCode? StatusCode { get; set; }

        /// <summary> Доступность </summary>
        [HiddenInput(DisplayValue = false)]
        public string Availability { get; set; }
    }
}