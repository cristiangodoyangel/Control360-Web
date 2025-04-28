using System.Collections.Generic;
using Inventario360.Web.Models;

namespace Inventario360.Web.Services
{
    public interface ICamionetaService
    {
        IEnumerable<Camioneta> ObtenerCamionetas();
        Camioneta ObtenerCamionetaPorId(int id);
        bool CrearCamioneta(Camioneta camioneta);
        bool EditarCamioneta(Camioneta camioneta);
        bool EliminarCamioneta(int id);
    }
}
