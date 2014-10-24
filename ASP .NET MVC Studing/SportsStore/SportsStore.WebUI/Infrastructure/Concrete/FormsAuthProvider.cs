using System.Web.Security;
using SportsStore.WebUI.Infrastructure.Abstract;

namespace SportsStore.WebUI.Infrastructure.Concrete
{
    public class FormsAuthProvider : IAuthProvider
    {
        #region IAuthProvider Implementation

        public bool Authenticate(string username, string password)
        {
            bool result = FormsAuthentication.Authenticate(username, password);
            if(result)
                FormsAuthentication.SetAuthCookie(username, false);

            return result;
        }

        #endregion
    }
}