using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Inventario360.Web.Data;
using Inventario360.Web.Models;

namespace Inventario360.Web.Services
{
    public class SolicitudService : ISolicitudService
    {
        private readonly InventarioDbContext _context;

        public SolicitudService(InventarioDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SolicitudDeMaterial>> ObtenerTodas()
        {
            return await _context.SolicitudDeMaterial.ToListAsync();
        }

        public async Task<SolicitudDeMaterial> ObtenerPorId(int id)
        {
            return await _context.SolicitudDeMaterial.FindAsync(id);
        }

        public async Task Agregar(SolicitudDeMaterial solicitud)
        {
            _context.SolicitudDeMaterial.Add(solicitud);
            await _context.SaveChangesAsync();
        }

        public async Task Actualizar(SolicitudDeMaterial solicitud)
        {
            var existente = await _context.SolicitudDeMaterial.FindAsync(solicitud.ID);
            if (existente != null)
            {
                existente.Cantidad = solicitud.Cantidad;
                existente.Medida = solicitud.Medida;
                existente.UnidadMedida = solicitud.UnidadMedida;
                existente.Marca = solicitud.Marca;
                existente.PosibleProveedor = solicitud.PosibleProveedor;

                _context.Entry(existente).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task Eliminar(int id)
        {
            var solicitud = await _context.SolicitudDeMaterial.FindAsync(id);
            if (solicitud != null)
            {
                _context.SolicitudDeMaterial.Remove(solicitud);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> SubirImagenAsync(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
            {
                throw new ArgumentException("No se ha proporcionado ninguna imagen.");
            }

            var folderPath = HttpContext.Current.Server.MapPath("~/images");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string fullPath = Path.Combine(folderPath, fileName);
            file.SaveAs(fullPath);

            return fileName;
        }
    }
}
