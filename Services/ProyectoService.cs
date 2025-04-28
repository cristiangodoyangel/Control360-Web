using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Inventario360.Web.Data;
using Inventario360.Web.Models;

namespace Inventario360.Web.Services.Implementaciones
{
    public class ProyectoService : IProyectoService
    {
        private readonly InventarioDbContext _context;

        public ProyectoService(InventarioDbContext context)
        {
            _context = context;
        }

        public async Task<List<Proyecto>> ObtenerTodos()
        {
            return await _context.Proyecto.ToListAsync();
        }

        public async Task<Proyecto> ObtenerPorId(int id)
        {
            return await _context.Proyecto.FindAsync(id);
        }

        public async Task Agregar(Proyecto proyecto)
        {
            _context.Proyecto.Add(proyecto);
            await _context.SaveChangesAsync();
        }

        public async Task Actualizar(Proyecto proyecto)
        {
            _context.Entry(proyecto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Eliminar(int id)
        {
            var proyecto = await _context.Proyecto.FindAsync(id);
            if (proyecto != null)
            {
                _context.Proyecto.Remove(proyecto);
                await _context.SaveChangesAsync();
            }
        }
    }
}
