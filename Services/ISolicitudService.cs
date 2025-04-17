using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Inventario360.Web.Models;

namespace Inventario360.Web.Services
{
    public interface ISolicitudService
    {
        Task<IEnumerable<SolicitudDeMaterial>> GetAllSolicitudesAsync();
        Task<IEnumerable<SolicitudDeMaterial>> ObtenerTodas();
        Task<SolicitudDeMaterial> GetSolicitudByIdAsync(int id);
        Task AddSolicitudAsync(SolicitudDeMaterial solicitud);
        Task UpdateSolicitudAsync(SolicitudDeMaterial solicitud);
        Task DeleteSolicitudAsync(int id);
        Task<string> SubirImagenAsync(HttpPostedFileBase file); // Ya no usa IFormFile
    }
}
