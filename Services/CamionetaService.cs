using System;
using System.Collections.Generic;
using System.Linq;
using Inventario360.Web.Data;
using Inventario360.Web.Models;

namespace Inventario360.Web.Services
{
    public class CamionetaService : ICamionetaService
    {
        private readonly InventarioDbContext _context;

        public CamionetaService(InventarioDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Camioneta> ObtenerCamionetas()
        {
            return _context.Camionetas.Include("Responsable").ToList();
        }

        public Camioneta ObtenerCamionetaPorId(int id)
        {
            return _context.Camionetas.Include("Responsable").FirstOrDefault(c => c.ID == id);
        }

        public bool CrearCamioneta(Camioneta camioneta)
        {
            try
            {
                _context.Camionetas.Add(camioneta);
                var resultado = _context.SaveChanges();
                Console.WriteLine("✅ Camioneta guardada en BD");
                return resultado > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al guardar camioneta: {ex.Message}");
                return false;
            }
        }

        public bool EditarCamioneta(Camioneta camioneta)
        {
            _context.Entry(camioneta).State = System.Data.Entity.EntityState.Modified;
            return _context.SaveChanges() > 0;
        }

        public bool EliminarCamioneta(int id)
        {
            var camioneta = _context.Camionetas.Find(id);
            if (camioneta == null)
                return false;

            _context.Camionetas.Remove(camioneta);
            return _context.SaveChanges() > 0;
        }
    }
}
