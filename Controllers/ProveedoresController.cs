using System.Web.Mvc;
using Inventario360.Web.Models;
using Inventario360.Web.Services;

namespace Inventario360.Web.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly IProveedorService _proveedorService;

        public ProveedoresController()
        {
            _proveedorService = new Services.Implementaciones.ProveedorService();
        }

        public ActionResult Index()
        {
            var proveedores = _proveedorService.ObtenerTodos();
            return View(proveedores);
        }

        public ActionResult Detalle(int id)
        {
            var proveedor = _proveedorService.ObtenerPorId(id);
            if (proveedor == null) return HttpNotFound();
            return View(proveedor);
        }

        [HttpGet]
        public ActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Proveedor proveedor)
        {
            if (!ModelState.IsValid)
                return View(proveedor);

            _proveedorService.Agregar(proveedor);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            var proveedor = _proveedorService.ObtenerPorId(id);
            if (proveedor == null) return HttpNotFound();
            return View(proveedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Proveedor proveedor)
        {
            if (!ModelState.IsValid)
                return View(proveedor);

            _proveedorService.Actualizar(proveedor);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Eliminar(int id)
        {
            var proveedor = _proveedorService.ObtenerPorId(id);
            if (proveedor == null) return HttpNotFound();
            return View(proveedor);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmarEliminar(int id)
        {
            _proveedorService.Eliminar(id);
            return RedirectToAction("Index");
        }
    }
}
