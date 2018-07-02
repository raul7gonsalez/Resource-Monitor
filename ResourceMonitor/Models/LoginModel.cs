namespace ResourceMonitor.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>Модель аутентификации</summary>
    public class LoginModel
    {
        /// <summary>Логин</summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>Пароль</summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}