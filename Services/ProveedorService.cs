using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Inventario360.Web.Models;
using Inventario360.Web.Data;

namespace Inventario360.Web.Services
{
    public class ProveedorService : IProveedorService
    {
        private readonly InventarioDbContext _context;

        public ProveedorService(InventarioDbContext context)
        {
            _context = context;
        }

        public async Task<List<Proveedor>> ObtenerTodos()
        {
            return await _context.Proveedor.ToListAsync();
        }

        public async Task<Proveedor> ObtenerPorId(int id)
        {
            return await _context.Proveedor.FindAsync(id);
        }

        public async Task Agregar(Proveedor proveedor)
        {
            _context.Proveedor.Add(proveedor);
            await _context.SaveChangesAsync();
        }

        public async Task Actualizar(Proveedor proveedor)
        {
            _context.Entry(proveedor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Eliminar(int id)
        {
            var proveedor = await _context.Proveedor.FindAsync(id);
            if (proveedor != null)
            {
                _context.Proveedor.Remove(proveedor);
                await _context.SaveChangesAsync();
            }
        }
    }
}
