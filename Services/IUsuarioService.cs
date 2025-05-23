﻿using Inventario360.Web.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventario360.Web.Services
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> ObtenerTodos();
        Task<Usuario> ObtenerPorId(string id);
        Task<IdentityResult> Crear(Usuario usuario, string contraseña);
        Task<IdentityResult> Actualizar(Usuario usuario);
        Task<IdentityResult> Eliminar(string id);
        Task<IList<string>> ObtenerRoles(Usuario usuario);
        Task<IList<string>> ObtenerRolesPorId(string id);
        Task AsignarRoles(string userId, List<string> roles);
        Task<List<string>> ObtenerTodosLosRoles();
    }
}
