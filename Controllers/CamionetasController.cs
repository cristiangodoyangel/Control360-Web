using System.Web.Mvc;
using System.Collections.Generic;
using Inventario360.Web.Models;
using Inventario360.Web.Services;

namespace Inventario360.Web.Controllers
{
    public class CamionetasController : Controller
    {
        private readonly IFichaCamionetaService _fichaCamionetaService;

        public CamionetasController(IFichaCamionetaService fichaCamionetaService)
        {
            _fichaCamionetaService = fichaCamionetaService;
        }

        public ActionResult Index()
        {
            var camionetas = _fichaCamionetaService.ObtenerTodasConResponsable();
            return View(camionetas);
        }

        public ActionResult Detalle(int id)
        {
            var ficha = _fichaCamionetaService.ObtenerFichaPorId(id);
            if (ficha == null) return HttpNotFound();
            return View(ficha);
        }

        public ActionResult Crear()
        {
            ViewBag.Empleados = new SelectList(_fichaCamionetaService.ObtenerEmpleados(), "ID", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(FichaCamioneta ficha)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Empleados = new SelectList(_fichaCamionetaService.ObtenerEmpleados(), "ID", "Nombre");
                return View(ficha);
            }

            _fichaCamionetaService.CrearFicha(ficha);
            return RedirectToAction("Index");
        }

        public ActionResult Editar(int id)
        {
            var ficha = _fichaCamionetaService.ObtenerFichaPorId(id);
            if (ficha == null) return HttpNotFound();

            ViewBag.Empleados = new SelectList(_fichaCamionetaService.ObtenerEmpleados(), "ID", "Nombre", ficha.ResponsableID);
            return View(ficha);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(FichaCamioneta ficha)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Empleados = new SelectList(_fichaCamionetaService.ObtenerEmpleados(), "ID", "Nombre", ficha.ResponsableID);
                return View(ficha);
            }

            if (!_fichaCamionetaService.ExisteResponsable(ficha.ResponsableID))
            {
                ModelState.AddModelError("ResponsableID", "El Responsable seleccionado no es válido.");
                ViewBag.Empleados = new SelectList(_fichaCamionetaService.ObtenerEmpleados(), "ID", "Nombre", ficha.ResponsableID);
                return View(ficha);
            }

            bool actualizado = _fichaCamionetaService.ActualizarFicha(ficha);
            if (!actualizado)
            {
                ModelState.AddModelError("", "No se pudo actualizar la ficha. Verifica los datos ingresados.");
                ViewBag.Empleados = new SelectList(_fichaCamionetaService.ObtenerEmpleados(), "ID", "Nombre", ficha.ResponsableID);
                return View(ficha);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Eliminar(int id)
        {
            var ficha = _fichaCamionetaService.ObtenerFichaPorId(id);
            if (ficha == null) return HttpNotFound();
            return View(ficha);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmarEliminar(int id)
        {
            bool eliminado = _fichaCamionetaService.EliminarFicha(id);
            if (!eliminado) return HttpNotFound();

            return RedirectToAction("Index");
        }
    }
}
