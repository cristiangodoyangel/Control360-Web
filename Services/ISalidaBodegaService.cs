using System.Collections.Generic;
using Inventario360.Web.Models;

namespace Inventario360.Web.Services
{
    public interface ISalidaBodegaService
    {
        bool RegistrarSalidaConProductos(SalidaDeBodega salida, List<DetalleSalidaDeBodega> productos);
        IEnumerable<SalidaDeBodega> ObtenerTodas();
        SalidaDeBodega ObtenerPorId(int id);
        List<DetalleSalidaDeBodega> ObtenerDetallesPorSalida(int id);
        void Eliminar(int id);
        void EliminarDetalles(int salidaId);
        void RevertirStock(int salidaId);
        object ObtenerDatosResumenSalidas(int mes, int año);
    }
}
