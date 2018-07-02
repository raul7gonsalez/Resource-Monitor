namespace ResourceMonitor.Authentification
{
    using System.Web.Security;

    using ResourceMonitor.Authentification.Interfaces;

    /// <inheritdoc />
    public class FormsAuthProvider : IAuthProvider
    {
        public bool Authenticate(string username, string password)
        {
            var result = FormsAuthentication.Authenticate(username, password);
            if (result)
            {
                FormsAuthentication.SetAuthCookie(username, false);
            }

            return result;
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
        }
    }
}