using System.Collections.Generic;
using Inventario360.Web.Models;

namespace Inventario360.Web.Services
{
    public interface IFichaEmpleadoService
    {
        List<FichaEmpleado> ObtenerFichas();
        FichaEmpleado ObtenerFichaPorEmpleado(int empleadoId);
        FichaEmpleado CrearFicha(FichaEmpleado ficha);
        FichaEmpleado ActualizarFicha(FichaEmpleado ficha);
        bool EliminarFicha(int id);
        void Agregar(FichaEmpleado ficha);
    }
}
