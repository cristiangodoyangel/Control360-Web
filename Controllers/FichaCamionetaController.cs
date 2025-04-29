using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Inventario360.Web.Models;
using Inventario360.Web.Services;

namespace Inventario360.Web.Controllers
{
    public class FichaCamionetaController : Controller
    {
        private readonly IFichaCamionetaService _fichaCamionetaService;

        public FichaCamionetaController(IFichaCamionetaService fichaCamionetaService)
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
            var ficha = _fichaCamionetaService.ObtenerDetalleConResponsable(id);
            if (ficha == null)
                return HttpNotFound();

            return View(ficha);
        }

        public ActionResult Crear()
        {
            var empleados = _fichaCamionetaService.ObtenerEmpleados();
            ViewBag.Empleados = new SelectList(empleados, "ID", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear(FichaCamioneta ficha)
        {
            if (!ModelState.IsValid)
            {
                var empleados =  _fichaCamionetaService.ObtenerEmpleados();
                ViewBag.Empleados = new SelectList(empleados, "ID", "Nombre");
                return View(ficha);
            }

             _fichaCamionetaService.CrearFicha(ficha);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Editar(int id)
        {
            var ficha =  _fichaCamionetaService.ObtenerFichaPorId(id);
            if (ficha == null)
                return HttpNotFound();

            var empleados =  _fichaCamionetaService.ObtenerEmpleados();
            ViewBag.Empleados = new SelectList(empleados, "ID", "Nombre", ficha.ResponsableID);
            return View(ficha);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(FichaCamioneta ficha)
        {
            if (!ModelState.IsValid)
            {
                var empleados =  _fichaCamionetaService.ObtenerEmpleados();
                ViewBag.Empleados = new SelectList(empleados, "ID", "Nombre", ficha.ResponsableID);
                return View(ficha);
            }

            var existeResponsable =  _fichaCamionetaService.ExisteResponsable(ficha.ResponsableID);
            if (!existeResponsable)
            {
                ModelState.AddModelError("ResponsableID", "El Responsable seleccionado no es válido.");
                var empleados =  _fichaCamionetaService.ObtenerEmpleados();
                ViewBag.Empleados = new SelectList(empleados, "ID", "Nombre", ficha.ResponsableID);
                return View(ficha);
            }

            bool actualizado =  _fichaCamionetaService.ActualizarFicha(ficha);
            if (!actualizado)
            {
                ModelState.AddModelError("", "No se pudo actualizar la ficha. Verifica los datos ingresados.");
                var empleados =  _fichaCamionetaService.ObtenerEmpleados();
                ViewBag.Empleados = new SelectList(empleados, "ID", "Nombre", ficha.ResponsableID);
                return View(ficha);
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Eliminar(int id)
        {
            var ficha =  _fichaCamionetaService.ObtenerFichaPorId(id);
            if (ficha == null)
                return HttpNotFound();

            return View(ficha);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EliminarConfirmed(int id)
        {
            bool eliminado =  _fichaCamionetaService.EliminarFicha(id);
            if (!eliminado)
                return HttpNotFound();

            return RedirectToAction("Index");
        }
    }
}
