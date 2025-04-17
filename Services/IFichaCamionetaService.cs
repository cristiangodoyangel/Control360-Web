using System.Collections.Generic;
using System.Threading.Tasks;
using Inventario360.Web.Models;

namespace Inventario360.Web.Services
{
    public interface IFichaCamionetaService
    {
        Task<IEnumerable<FichaCamioneta>> ObtenerFichas();
        Task<FichaCamioneta> ObtenerFichaPorId(int id);
        Task CrearFicha(FichaCamioneta ficha);
        Task<bool> ActualizarFicha(FichaCamioneta ficha);
        Task<bool> EliminarFicha(int id);
        Task<IEnumerable<FichaCamioneta>> ObtenerTodas();
        Task<FichaCamioneta> ObtenerPorId(int id);
        Task<bool> ExisteResponsable(int responsableID);
        Task<List<Empleado>> ObtenerEmpleados();
    }
}
