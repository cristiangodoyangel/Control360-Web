using Inventario360.Web.Models;
using Inventario360.Web.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Inventario360.Web.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsuariosController()
        {
            var db = new Data.InventarioDbContext();
            _userManager = new UserManager<Usuario>(new UserStore<Usuario>(db));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
        }

        public async Task<ActionResult> Index()
        {
            var usuarios = _userManager.Users.ToList();
            var modelo = new List<UsuarioConRolesViewModel>();

            foreach (var usuario in usuarios)
            {
                var roles = await _userManager.GetRolesAsync(usuario.Id);
                modelo.Add(new UsuarioConRolesViewModel
                {
                    Id = usuario.Id,
                    NombreCompleto = usuario.NombreCompleto,
                    Email = usuario.Email,
                    Roles = roles.ToList()
                });
            }

            return View(modelo);
        }

        [HttpGet]
        public ActionResult Crear()
        {
            ViewBag.Roles = _roleManager.Roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            }).ToList();

            return View(new CrearUsuarioViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear(CrearUsuarioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _roleManager.Roles.Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                }).ToList();
                return View(model);
            }

            var usuario = new Usuario
            {
                UserName = model.Email,
                Email = model.Email,
                NombreCompleto = model.NombreCompleto,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(usuario, model.Password);

            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(usuario.Id, model.Rol);
                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError("", "Error al asignar rol: " + error);
                    }

                    await _userManager.DeleteAsync(usuario);
                    ViewBag.Roles = _roleManager.Roles.Select(r => new SelectListItem
                    {
                        Value = r.Name,
                        Text = r.Name
                    }).ToList();

                    return View(model);
                }

                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }

            ViewBag.Roles = _roleManager.Roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            }).ToList();

            return View(model);
        }

        [HttpGet]
        public ActionResult Editar(string id)
        {
            var usuario = _userManager.FindById(id);
            if (usuario == null) return HttpNotFound();

            var rolesUsuario = _userManager.GetRoles(usuario.Id);
            var modelo = new EditarUsuarioViewModel
            {
                Id = usuario.Id,
                NombreCompleto = usuario.NombreCompleto,
                Email = usuario.Email,
                Rol = rolesUsuario.FirstOrDefault()
            };

            ViewBag.Roles = _roleManager.Roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name,
                Selected = rolesUsuario.Contains(r.Name)
            }).ToList();

            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(string id, EditarUsuarioViewModel model)
        {
            if (id != model.Id) return HttpNotFound();

            var usuario = _userManager.FindById(id);
            if (usuario == null) return HttpNotFound();

            usuario.NombreCompleto = model.NombreCompleto;

            var rolesActuales = _userManager.GetRoles(usuario.Id);
            _userManager.RemoveFromRoles(usuario.Id, rolesActuales.ToArray());

            if (!string.IsNullOrEmpty(model.Rol))
            {
                _userManager.AddToRole(usuario.Id, model.Rol);
            }

            _userManager.Update(usuario);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Eliminar(EliminarUsuarioViewModel model)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                return Json(new { success = false, message = "ID inválido." });
            }

            var usuario = _userManager.FindById(model.Id);
            if (usuario == null)
            {
                return Json(new { success = false, message = "Usuario no encontrado" });
            }

            var result = _userManager.Delete(usuario);
            if (result.Succeeded)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "No se pudo eliminar el usuario" });
        }

        [HttpGet]
        public ActionResult Roles(string userId)
        {
            var usuario = _userManager.FindById(userId);
            if (usuario == null) return HttpNotFound();

            var rolesTodos = _roleManager.Roles.Select(r => r.Name).ToList();
            var rolesAsignados = _userManager.GetRoles(usuario.Id);

            var modelo = new RolesViewModel
            {
                UserId = usuario.Id,
                NombreUsuario = usuario.NombreCompleto,
                TodosLosRoles = rolesTodos,
                RolesAsignados = rolesAsignados.ToList()
            };

            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Roles(string userId, string[] RolesSeleccionados)
        {
            var usuario = _userManager.FindById(userId);
            if (usuario == null) return HttpNotFound();

            var rolesActuales = _userManager.GetRoles(usuario.Id);
            _userManager.RemoveFromRoles(usuario.Id, rolesActuales.ToArray());

            if (RolesSeleccionados != null)
            {
                foreach (var rol in RolesSeleccionados)
                {
                    _userManager.AddToRole(usuario.Id, rol);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
