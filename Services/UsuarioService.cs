using Inventario360.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventario360.Web.Services.Implementaciones
{
    public class UsuarioService : IUsuarioService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsuarioService()
        {
            var context = new Data.InventarioDbContext();
            _userManager = new UserManager<Usuario>(new UserStore<Usuario>(context));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
        }

        public Task<List<Usuario>> ObtenerTodos()
        {
            return Task.FromResult(_userManager.Users.ToList());
        }

        public Task<Usuario> ObtenerPorId(string id)
        {
            return _userManager.FindByIdAsync(id);
        }

        public Task<IdentityResult> Crear(Usuario usuario, string contraseña)
        {
            return _userManager.CreateAsync(usuario, contraseña);
        }

        public Task<IdentityResult> Actualizar(Usuario usuario)
        {
            return _userManager.UpdateAsync(usuario);
        }

        public async Task<IdentityResult> Eliminar(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario != null)
            {
                return await _userManager.DeleteAsync(usuario);
            }
            return IdentityResult.Failed("Usuario no encontrado");
        }

        public Task<IList<string>> ObtenerRoles(Usuario usuario)
        {
            return _userManager.GetRolesAsync(usuario.Id);
        }

        public async Task<IList<string>> ObtenerRolesPorId(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            return usuario != null
                ? await _userManager.GetRolesAsync(usuario.Id)
                : new List<string>();
        }

        public async Task AsignarRoles(string userId, List<string> roles)
        {
            var usuario = await _userManager.FindByIdAsync(userId);
            if (usuario != null)
            {
                var rolesActuales = await _userManager.GetRolesAsync(usuario.Id);
                await _userManager.RemoveFromRolesAsync(usuario.Id, rolesActuales.ToArray());
                await _userManager.AddToRolesAsync(usuario.Id, roles.ToArray());
            }
        }

        public Task<List<string>> ObtenerTodosLosRoles()
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return Task.FromResult(roles);
        }
    }
}
