using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

public class SetupController : Controller
{
    public ActionResult CrearAdmin()
    {
        using (var context = new Inventario360.Web.Data.InventarioDbContext())
        {
            var userStore = new UserStore<Inventario360.Web.Models.Usuario>(context);
            var userManager = new UserManager<Inventario360.Web.Models.Usuario>(userStore);

            var usuario = userManager.FindByEmail("admin@example.com");
            if (usuario == null)
            {
                var nuevoUsuario = new Inventario360.Web.Models.Usuario
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    NombreCompleto = "Administrador"
                };

                var resultado = userManager.Create(nuevoUsuario, "Password123!");
                if (resultado.Succeeded)
                {
                    return Content("✅ Usuario administrador creado.");
                }
                else
                {
                    return Content("❌ Error: " + string.Join(" | ", resultado.Errors));
                }
            }

            return Content("⚠️ El usuario ya existe.");
        }
    }
}
