using Inventario360.Web.Models;
using Inventario360.Web.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Inventario360.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsuarioService()
        {
            var dbContext = new IdentityDbContext("DefaultConnection"); // Usa tu cadena de conexión real
            var userStore = new UserStore<Usuario>(dbContext);
            var roleStore = new RoleStore<IdentityRole>(dbContext);

            _userManager = new UserManager<Usuario>(userStore);
            _roleManager = new RoleManager<IdentityRole>(roleStore);
        }

        public async Task<List<Usuario>> ObtenerTodos()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<Usuario> ObtenerPorId(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IdentityResult> Crear(Usuario usuario, string contraseña)
        {
            return await _userManager.CreateAsync(usuario, contraseña);
        }

        public async Task<IdentityResult> Actualizar(Usuario usuario)
        {
            return await _userManager.UpdateAsync(usuario);
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

        public async Task<IList<string>> ObtenerRoles(Usuario usuario)
        {
            return await _userManager.GetRolesAsync(usuario.Id);
        }

        public async Task<IList<string>> ObtenerRolesPorId(string id)
        {
            return await _userManager.GetRolesAsync(id);
        }

        public async Task AsignarRoles(string userId, List<string> roles)
        {
            var actuales = await _userManager.GetRolesAsync(userId);
            await _userManager.RemoveFromRolesAsync(userId, actuales.ToArray());
            await _userManager.AddToRolesAsync(userId, roles.ToArray());
        }

        public async Task<List<string>> ObtenerTodosLosRoles()
        {
            return await _roleManager.Roles.Select(r => r.Name).ToListAsync();
        }
    }
}
