using System.Collections.Generic;
using System.Linq;
using Inventario360.Web.Models;
using Inventario360.Web.Data;

namespace Inventario360.Web.Services
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly InventarioDbContext _context;

        public EmpleadoService(InventarioDbContext context)
        {
            _context = context;
        }

        public List<Empleado> ObtenerTodos()
        {
            return _context.Empleado.ToList();
        }

        public Empleado ObtenerPorId(int id)
        {
            return _context.Empleado.Find(id);
        }

        public void Agregar(Empleado empleado)
        {
            _context.Empleado.Add(empleado);
            _context.SaveChanges();
        }

        public void Actualizar(Empleado empleado)
        {
            _context.Entry(empleado).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Eliminar(int id)
        {
            var empleado = _context.Empleado.Find(id);
            if (empleado != null)
            {
                _context.Empleado.Remove(empleado);
                _context.SaveChanges();
            }
        }
    }
}
