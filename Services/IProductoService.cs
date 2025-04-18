﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Inventario360.Web.Models;

namespace Inventario360.Web.Services
{
    public interface IProductoService
    {
        Task<List<Producto>> ObtenerTodos();
        Task<Producto> ObtenerPorId(int id);
        Task<Producto> GetProductoByIdAsync(int id);
        Task<IEnumerable<Producto>> ObtenerTodosAsync();
        Task Agregar(Producto producto);
        Task Actualizar(Producto producto);
        Task Eliminar(int id);
    }
}
