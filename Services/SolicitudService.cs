using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using Inventario360.Web.Data;
using Inventario360.Web.Models;

namespace Inventario360.Web.Services
{
    public class SolicitudService : ISolicitudService
    {
        private readonly InventarioDbContext _context;
        private readonly string _uploadsFolder;

        public SolicitudService(InventarioDbContext context)
        {
            _context = context;
            _uploadsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content/Images");

            if (!Directory.Exists(_uploadsFolder))
            {
                Directory.CreateDirectory(_uploadsFolder);
            }
        }

        public IEnumerable<SolicitudDeMaterial> GetAllSolicitudes()
        {
            return _context.SolicitudDeMaterial.ToList();
        }

        public IEnumerable<SolicitudDeMaterial> ObtenerTodas()
        {
            return _context.SolicitudDeMaterial.ToList();
        }

        public SolicitudDeMaterial GetSolicitudById(int id)
        {
            return _context.SolicitudDeMaterial.Find(id);
        }

        public void AddSolicitud(SolicitudDeMaterial solicitud)
        {
            _context.SolicitudDeMaterial.Add(solicitud);
            _context.SaveChanges();
        }

        public void UpdateSolicitud(SolicitudDeMaterial solicitud)
        {
            var existingSolicitud = _context.SolicitudDeMaterial.Find(solicitud.ID);
            if (existingSolicitud != null)
            {
                existingSolicitud.Cantidad = solicitud.Cantidad;
                existingSolicitud.Medida = solicitud.Medida;
                existingSolicitud.UnidadMedida = solicitud.UnidadMedida;
                existingSolicitud.Marca = solicitud.Marca;
                existingSolicitud.PosibleProveedor = solicitud.PosibleProveedor;
                _context.SaveChanges();
            }
        }

        public void DeleteSolicitud(int id)
        {
            var solicitud = _context.SolicitudDeMaterial.Find(id);
            if (solicitud != null)
            {
                _context.SolicitudDeMaterial.Remove(solicitud);
                _context.SaveChanges();
            }
        }

        public string SubirImagen(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
            {
                throw new ArgumentException("No se ha proporcionado ninguna imagen.");
            }

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(_uploadsFolder, fileName);

            file.SaveAs(filePath);

            return fileName;
        }
    }
}
