namespace ResourceMonitor.Authentification.Interfaces
{
    /// <summary>Интерфейс сервиса аутентификации</summary>
    public interface IAuthProvider
    {
        /// <summary>Пройти аутентификацию</summary>
        bool Authenticate(string username, string password);

        /// <summary>Выйти</summary>
        void Logout();
    }
}