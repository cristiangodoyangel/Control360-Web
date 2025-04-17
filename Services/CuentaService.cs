using Inventario360.Web.Models;
using System.Threading.Tasks;
using System.Web.Security;

namespace Inventario360.Web.Services
{
    public class CuentaService : ICuentaService
    {
        public async Task<SignInResult> LoginAsync(Cuenta model)
        {
            // Aquí puedes consultar tu DB si usas una tabla de usuarios propia
            bool esValido = Membership.ValidateUser(model.Email, model.Password);

            if (esValido)
            {
                FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);
                return SignInResult.Success;
            }

            return SignInResult.Failed;
        }
    }
}
