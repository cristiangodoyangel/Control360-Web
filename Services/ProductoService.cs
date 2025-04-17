using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Inventario360.Web.Models;
using Inventario360.Web.Data;
using Inventario360.Web.Services;

namespace Inventario360.Web.Services
{
    public class ProductoService : IProductoService
    {
        private readonly InventarioDbContext _context;

        public ProductoService(InventarioDbContext context)
        {
            _context = context;
        }

        public async Task<List<Producto>> ObtenerTodos()
        {
            return await _context.Producto.ToListAsync();
        }

        public async Task<IEnumerable<Producto>> ObtenerTodosAsync()
        {
            return await _context.Producto.ToListAsync();
        }

        public async Task<Producto> ObtenerPorId(int id)
        {
            return await _context.Producto.FindAsync(id);
        }

        public async Task<Producto> GetProductoByIdAsync(int id)
        {
            return await _context.Producto.FindAsync(id);
        }

        public async Task Agregar(Producto producto)
        {
            _context.Producto.Add(producto);
            await _context.SaveChangesAsync();
        }

        public async Task Actualizar(Producto producto)
        {
            var existente = await _context.Producto.FindAsync(producto.ITEM);
            if (existente != null)
            {
                existente.Cantidad = producto.Cantidad;
                existente.NombreTecnico = producto.NombreTecnico;
                existente.Medida = producto.Medida;
                existente.UnidadMedida = producto.UnidadMedida;
                existente.Marca = producto.Marca;
                existente.Descripcion = producto.Descripcion;
                existente.Imagen = producto.Imagen;
                existente.Proveedor = producto.Proveedor;
                existente.Ubicacion = producto.Ubicacion;
                existente.Estado = producto.Estado;
                existente.Categoria = producto.Categoria;

                await _context.SaveChangesAsync();
            }
        }

        public async Task Eliminar(int id)
        {
            var producto = await _context.Producto.FindAsync(id);
            if (producto != null)
            {
                _context.Producto.Remove(producto);
                await _context.SaveChangesAsync();
            }
        }
    }
}
