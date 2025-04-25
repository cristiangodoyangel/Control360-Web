using System.Collections.Generic;
using System.Linq;
using Inventario360.Web.Models;
using Inventario360.Web.Data;

namespace Inventario360.Web.Services.Implementaciones
{
    public class ProveedorService : IProveedorService
    {
        private readonly InventarioDbContext _context;

        public ProveedorService()
        {
            _context = new InventarioDbContext();
        }

        public List<Proveedor> ObtenerTodos()
        {
            return _context.Proveedor.ToList();
        }

        public Proveedor ObtenerPorId(int id)
        {
            return _context.Proveedor.Find(id);
        }

        public void Agregar(Proveedor proveedor)
        {
            _context.Proveedor.Add(proveedor);
            _context.SaveChanges();
        }

        public void Actualizar(Proveedor proveedor)
        {
            var proveedorExistente = _context.Proveedor.Find(proveedor.ID);
            if (proveedorExistente != null)
            {
                proveedorExistente.Nombre = proveedor.Nombre;
                proveedorExistente.Contacto = proveedor.Contacto;
                _context.SaveChanges();
            }
        }

        public void Eliminar(int id)
        {
            var proveedor = _context.Proveedor.Find(id);
            if (proveedor != null)
            {
                _context.Proveedor.Remove(proveedor);
                _context.SaveChanges();
            }
        }

        public string ObtenerNombreProveedor(int? proveedorId)
        {
            if (proveedorId == null) return "Proveedor no definido";

            var proveedor = _context.Proveedor.Find(proveedorId);
            return proveedor != null ? proveedor.Nombre : "Proveedor no definido";
        }

    }
}
