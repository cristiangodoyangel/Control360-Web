using System.Collections.Generic;
using Inventario360.Web.Models;

namespace Inventario360.Web.Services
{
    public interface IProveedorService
    {
        List<Proveedor> ObtenerTodos();         // Obtener todos los proveedores
        Proveedor ObtenerPorId(int id);         // Obtener proveedor por ID
        void Agregar(Proveedor proveedor);      // Agregar nuevo proveedor
        void Actualizar(Proveedor proveedor);   // Actualizar proveedor existente
        void Eliminar(int id);                  // Eliminar proveedor por ID
    }
}
