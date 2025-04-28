using System.Collections.Generic;
using Inventario360.Web.Models;

namespace Inventario360.Web.Services
{
    public interface IEmpleadoService
    {
        List<Empleado> ObtenerTodos(); // Obtener lista de empleados
        Empleado ObtenerPorId(int id); // Obtener un empleado por ID
        void Agregar(Empleado empleado); // Agregar un nuevo empleado
        void Actualizar(Empleado empleado); // Actualizar un empleado existente
        void Eliminar(int id); // Eliminar un empleado por ID
    }
}
