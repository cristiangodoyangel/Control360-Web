using System.Collections.Generic;
using Inventario360.Web.Models;

namespace Inventario360.Web.Services
{
    public interface IFichaCamionetaService
    {
        IEnumerable<FichaCamioneta> ObtenerFichas();
        FichaCamioneta ObtenerFichaPorId(int id);
        void CrearFicha(FichaCamioneta ficha);
        bool ActualizarFicha(FichaCamioneta ficha);
        bool EliminarFicha(int id);
        IEnumerable<FichaCamioneta> ObtenerTodas();
        FichaCamioneta ObtenerPorId(int id);
        bool ExisteResponsable(int responsableID);
        List<Empleado> ObtenerEmpleados();
        List<FichaCamioneta> ObtenerTodasConResponsable();
        FichaCamioneta ObtenerDetalleConResponsable(int id);
    }
}
