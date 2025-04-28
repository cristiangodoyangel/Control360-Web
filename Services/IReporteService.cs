using System.Collections.Generic;
using Inventario360.Web.Models;

namespace Inventario360.Web.Services
{
    public interface IReporteService
    {
        IEnumerable<Reporte> ObtenerDatosReportes();
        Reporte ObtenerDetalleReporte(int id);
    }
}
