using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Inventario360.Web.Models;
using System.Web;

namespace Inventario360.Web.Controllers
{
    public class DebugController : Controller
    {
        public ActionResult ResetPassword()
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            string userId = "77a83b5b-01bc-4d4b-b274-d4af386b81ee"; // <-- tu ID real
            string newPassword = "Password123!"; // <-- nueva contraseña segura

            var token = userManager.GeneratePasswordResetToken(userId);
            var result = userManager.ResetPassword(userId, token, newPassword);

            if (result.Succeeded)
            {
                return Content("✅ Contraseña restablecida correctamente.");
            }
            else
            {
                return Content("❌ Error: " + string.Join(", ", result.Errors));
            }
        }
    }
}
