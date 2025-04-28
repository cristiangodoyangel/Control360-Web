using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Inventario360.Web.Models;
using Inventario360.Web.Services;
using Inventario360.Web.Data;
using Inventario360.Web.Models.ViewModels;

namespace Inventario360.Web.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly IEmpleadoService _empleadoService;
        private readonly IFichaEmpleadoService _fichaEmpleadoService;
        private readonly InventarioDbContext _context;

        public EmpleadosController(IEmpleadoService empleadoService, IFichaEmpleadoService fichaEmpleadoService, InventarioDbContext context)
        {
            _empleadoService = empleadoService;
            _fichaEmpleadoService = fichaEmpleadoService;
            _context = context;
        }

        public ActionResult Index()
        {
            var empleados = _empleadoService.ObtenerTodos();
            return View(empleados);
        }

        public ActionResult Detalle(int id)
        {
            var empleado = _empleadoService.ObtenerPorId(id);
            if (empleado == null) return HttpNotFound();
            return View(empleado);
        }

        public ActionResult Crear()
        {
            var viewModel = new EmpleadoConFichaViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(EmpleadoConFichaViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var empleado = new Empleado
            {
                Nombre = model.Nombre,
                Cargo = model.Cargo
            };

            _context.Empleado.Add(empleado);
            _context.SaveChanges();

            var ficha = new FichaEmpleado
            {
                EmpleadoID = empleado.ID,
                FechaIngreso = model.FechaIngreso,
                FechaTerminoContrato = model.FechaTerminoContrato,
                FechaVencimientoExamen = model.FechaVencimientoExamen,
                CursoAltura = model.CursoAltura,
                Acreditaciones = model.Acreditaciones
            };

            _context.FichaEmpleado.Add(ficha);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Editar(int id)
        {
            var empleado = _context.Empleado.Find(id);
            var ficha = _context.FichaEmpleado.FirstOrDefault(f => f.EmpleadoID == id);

            if (empleado == null || ficha == null)
                return HttpNotFound();

            var model = new EmpleadoConFichaViewModel
            {
                Nombre = empleado.Nombre,
                Cargo = empleado.Cargo,
                FechaIngreso = ficha.FechaIngreso,
                FechaTerminoContrato = ficha.FechaTerminoContrato,
                FechaVencimientoExamen = ficha.FechaVencimientoExamen,
                CursoAltura = ficha.CursoAltura,
                Acreditaciones = ficha.Acreditaciones
            };

            ViewBag.EmpleadoID = id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, EmpleadoConFichaViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var empleado = _context.Empleado.Find(id);
            var ficha = _context.FichaEmpleado.FirstOrDefault(f => f.EmpleadoID == id);

            if (empleado == null || ficha == null)
                return HttpNotFound();

            empleado.Nombre = model.Nombre;
            empleado.Cargo = model.Cargo;

            ficha.FechaIngreso = model.FechaIngreso;
            ficha.FechaTerminoContrato = model.FechaTerminoContrato;
            ficha.FechaVencimientoExamen = model.FechaVencimientoExamen;
            ficha.CursoAltura = model.CursoAltura;
            ficha.Acreditaciones = model.Acreditaciones;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Eliminar(int id)
        {
            var empleado = _empleadoService.ObtenerPorId(id);
            if (empleado == null)
                return HttpNotFound();

            return View(empleado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Eliminar")]
        public ActionResult ConfirmarEliminar(int id)
        {
            try
            {
                var empleado = _context.Empleado.Find(id);
                if (empleado == null)
                {
                    TempData["Error"] = "Empleado no encontrado.";
                    return RedirectToAction("Index");
                }

                var ficha = _context.FichaEmpleado.FirstOrDefault(f => f.EmpleadoID == id);
                if (ficha != null)
                {
                    _context.FichaEmpleado.Remove(ficha);
                }

                bool tieneSalidas = _context.SalidaDeBodega.Any(s => s.Solicitante == id || s.ResponsableEntrega == id);

                if (tieneSalidas)
                {
                    TempData["Error"] = "No se puede eliminar el empleado porque está asociado a una Salida de Bodega.";
                    return RedirectToAction("Index");
                }

                _context.Empleado.Remove(empleado);
                _context.SaveChanges();

                TempData["Success"] = "Empleado y ficha eliminados correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error interno: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
