using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Inventario360.Services;
using Inventario360.Web.Data;
using Inventario360.Web.Models;

namespace Inventario360.Web.Services
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly InventarioDbContext _context;

        public EmpleadoService(InventarioDbContext context)
        {
            _context = context;
        }

        public async Task<List<Empleado>> ObtenerTodos()
        {
            return await _context.Empleado.ToListAsync();
        }

        public async Task<Empleado> ObtenerPorId(int id)
        {
            return await _context.Empleado.FindAsync(id);
        }

        public async Task Agregar(Empleado empleado)
        {
            _context.Empleado.Add(empleado);
            await _context.SaveChangesAsync();
        }

        public async Task Actualizar(Empleado empleado)
        {
            _context.Entry(empleado).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Eliminar(int id)
        {
            var empleado = await _context.Empleado.FindAsync(id);
            if (empleado != null)
            {
                _context.Empleado.Remove(empleado);
                await _context.SaveChangesAsync();
            }
        }
    }
}
