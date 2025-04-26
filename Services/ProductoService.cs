using System.Collections.Generic;
using System.Linq;
using Inventario360.Web.Models;
using Inventario360.Web.Data;

namespace Inventario360.Web.Services
{
    public class ProductoService : IProductoService
    {
        private readonly InventarioDbContext _context;

        public ProductoService()
        {
            _context = new InventarioDbContext();
        }

        public List<Producto> ObtenerTodos()
        {
            return _context.Producto.ToList();
        }

        public Producto ObtenerPorId(int id)
        {
            return _context.Database.SqlQuery<Producto>(
                @"SELECT ITEM, Cantidad, NombreTecnico, Medida, UnidadMedida, Marca,
                 Descripcion, Imagen, Proveedor, Ubicacion, Estado, Categoria
          FROM Producto
          WHERE ITEM = @p0", id).FirstOrDefault();
        }





        public void Agregar(Producto producto)
        {
            _context.Producto.Add(producto);
            _context.SaveChanges();
        }

        public void Actualizar(Producto producto)
        {
            var productoExistente = _context.Producto.FirstOrDefault(p => p.ITEM == producto.ITEM);
            if (productoExistente != null)
            {
                productoExistente.Cantidad = producto.Cantidad;
                productoExistente.NombreTecnico = producto.NombreTecnico;
                productoExistente.Medida = producto.Medida;
                productoExistente.UnidadMedida = producto.UnidadMedida;
                productoExistente.Marca = producto.Marca;
                productoExistente.Descripcion = producto.Descripcion;
                productoExistente.Imagen = producto.Imagen;
                productoExistente.Proveedor = producto.Proveedor;
                productoExistente.Ubicacion = producto.Ubicacion;
                productoExistente.Estado = producto.Estado;
                productoExistente.Categoria = producto.Categoria;

                _context.SaveChanges();
            }
        }





        public void Eliminar(int id)
        {
            var producto = _context.Producto.Find(id);
            if (producto != null)
            {
                _context.Producto.Remove(producto);
                _context.SaveChanges();
            }
        }
    }
}
