using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Inventario360.Web.Models;
using Inventario360.Web.Services;
using Inventario360.Web.Data;

namespace Inventario360.Web.Controllers
{
    public class ProyectosController : Controller
    {
        private readonly IProyectoService _proyectoService;
        private readonly InventarioDbContext _context;

        public ProyectosController()
        {
            _context = new InventarioDbContext();
            _proyectoService = new Services.Implementaciones.ProyectoService(_context);
        }

        public async Task<ActionResult> Index()
        {
            var proyectos = await _proyectoService.ObtenerTodos();
            return View(proyectos);
        }

        public async Task<ActionResult> Detalle(int id)
        {
            var proyecto = await _proyectoService.ObtenerPorId(id);
            if (proyecto == null) return HttpNotFound();
            return View(proyecto);
        }

        public ActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear(Proyecto proyecto)
        {
            if (!ModelState.IsValid) return View(proyecto);

            await _proyectoService.Agregar(proyecto);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Editar(int? id)
        {
            if (id == null) return RedirectToAction("Index");

            var proyecto = await _proyectoService.ObtenerPorId(id.Value);
            if (proyecto == null) return HttpNotFound();

            return View(proyecto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(Proyecto proyecto)
        {
            if (!ModelState.IsValid) return View(proyecto);

            await _proyectoService.Actualizar(proyecto);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Eliminar(int id)
        {
            var proyecto = await _proyectoService.ObtenerPorId(id);
            if (proyecto == null) return HttpNotFound();
            return View(proyecto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmarEliminar(int id)
        {
            try
            {
                var proyecto = await _proyectoService.ObtenerPorId(id);
                if (proyecto == null)
                {
                    TempData["Error"] = "No se encontró el proyecto.";
                    return RedirectToAction("Index");
                }

                var tieneSalidas = _context.SalidaDeBodega.Any(s => s.ProyectoAsignado == id);
                if (tieneSalidas)
                {
                    TempData["Error"] = "No se puede eliminar el proyecto porque está asociado a una salida de bodega.";
                    return RedirectToAction("Index");
                }

                _context.Proyecto.Remove(proyecto);
                _context.SaveChanges();

                TempData["Success"] = "Proyecto eliminado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error interno: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
