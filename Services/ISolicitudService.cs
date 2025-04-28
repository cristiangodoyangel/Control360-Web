using System.Collections.Generic;
using Inventario360.Web.Models;
using System.Web;

namespace Inventario360.Web.Services
{
    public interface ISolicitudService
    {
        IEnumerable<SolicitudDeMaterial> GetAllSolicitudes();
        IEnumerable<SolicitudDeMaterial> ObtenerTodas();
        SolicitudDeMaterial GetSolicitudById(int id);
        void AddSolicitud(SolicitudDeMaterial solicitud);
        void UpdateSolicitud(SolicitudDeMaterial solicitud);
        void DeleteSolicitud(int id);
        string SubirImagen(HttpPostedFileBase file);
    }
}
