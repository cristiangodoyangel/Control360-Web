using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Inventario360.Web.Data;
using Inventario360.Web.Models;

namespace Inventario360.Web.Services
{
    public class FichaCamionetaService : IFichaCamionetaService
    {
        private readonly InventarioDbContext _context;

        public FichaCamionetaService(InventarioDbContext context)
        {
            _context = context;
        }

        public IEnumerable<FichaCamioneta> ObtenerFichas()
        {
            return _context.FichaCamionetas.ToList();
        }

        public FichaCamioneta ObtenerFichaPorId(int id)
        {
            return _context.FichaCamionetas.Find(id);
        }

        public void CrearFicha(FichaCamioneta ficha)
        {
            _context.FichaCamionetas.Add(ficha);
            _context.SaveChanges();
        }

        public bool ActualizarFicha(FichaCamioneta ficha)
        {
            var fichaExistente = _context.FichaCamionetas.Find(ficha.ID);
            if (fichaExistente == null) return false;

            var responsableExiste = _context.Empleado.Any(e => e.ID == ficha.ResponsableID);
            if (!responsableExiste)
            {
                return false;
            }

            _context.Entry(fichaExistente).CurrentValues.SetValues(ficha);
            _context.SaveChanges();
            return true;
        }

        public bool EliminarFicha(int id)
        {
            var ficha = _context.FichaCamionetas.Find(id);
            if (ficha == null) return false;

            _context.FichaCamionetas.Remove(ficha);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<FichaCamioneta> ObtenerTodas()
        {
            return _context.FichaCamionetas.ToList();
        }

        public FichaCamioneta ObtenerPorId(int id)
        {
            return _context.FichaCamionetas.Find(id);
        }

        public bool ExisteResponsable(int responsableID)
        {
            return _context.Empleado.Any(e => e.ID == responsableID);
        }

        public List<Empleado> ObtenerEmpleados()
        {
            return _context.Empleado.ToList();
        }

        public List<FichaCamioneta> ObtenerTodasConResponsable()
        {
            return _context.FichaCamionetas
                .Include(c => c.Responsable)
                .ToList();
        }

        public FichaCamioneta ObtenerDetalleConResponsable(int id)
        {
            return _context.FichaCamionetas
                .Include(c => c.Responsable)
                .FirstOrDefault(c => c.ID == id);
        }
    }
}
