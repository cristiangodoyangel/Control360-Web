using System.Collections.Generic;
using System.Threading.Tasks;
using Inventario360.Web.Models;

namespace Inventario360.Web.Services
{
    public interface ICamionetaService
    {
        Task<IEnumerable<Camioneta>> ObtenerCamionetas();
        Task<Camioneta> ObtenerCamionetaPorId(int id);
        Task<bool> CrearCamioneta(Camioneta camioneta);
        Task<bool> EditarCamioneta(Camioneta camioneta);
        Task<bool> EliminarCamioneta(int id);
    }
}
