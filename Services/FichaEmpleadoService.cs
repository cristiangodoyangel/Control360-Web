using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Inventario360.Web.Data;
using Inventario360.Web.Models;

namespace Inventario360.Web.Services
{
    public class FichaEmpleadoService : IFichaEmpleadoService
    {
        private readonly InventarioDbContext _context;

        public FichaEmpleadoService(InventarioDbContext context)
        {
            _context = context;
        }

        public List<FichaEmpleado> ObtenerFichas()
        {
            return _context.FichaEmpleado.Include(f => f.Empleado).ToList();
        }

        public FichaEmpleado ObtenerFichaPorEmpleado(int empleadoId)
        {
            return _context.FichaEmpleado
                .Include(f => f.Empleado)
                .FirstOrDefault(f => f.EmpleadoID == empleadoId);
        }

        public FichaEmpleado CrearFicha(FichaEmpleado ficha)
        {
            _context.FichaEmpleado.Add(ficha);
            _context.SaveChanges();
            return ficha;
        }

        public FichaEmpleado ActualizarFicha(FichaEmpleado ficha)
        {
            _context.Entry(ficha).State = EntityState.Modified;
            _context.SaveChanges();
            return ficha;
        }

        public bool EliminarFicha(int id)
        {
            var ficha = _context.FichaEmpleado.Find(id);
            if (ficha == null) return false;

            _context.FichaEmpleado.Remove(ficha);
            _context.SaveChanges();
            return true;
        }

        public void Agregar(FichaEmpleado ficha)
        {
            _context.FichaEmpleado.Add(ficha);
            _context.SaveChanges();
        }
    }
}
