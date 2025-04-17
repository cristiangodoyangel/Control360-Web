using Inventario360.Web.Models;
using System.Threading.Tasks;

namespace Inventario360.Web.Services
{
    public interface ICuentaService
    {
        Task<SignInResult> LoginAsync(Cuenta model);
    }
}
