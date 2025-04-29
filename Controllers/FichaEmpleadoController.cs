using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Inventario360.Web.Models;
using Inventario360.Web.Services;
using Inventario360.Web.Data;

namespace Inventario360.Web.Controllers
{
    public class FichaEmpleadoController : Controller
    {
        private readonly IFichaEmpleadoService _fichaEmpleadoService;
        private readonly InventarioDbContext _context;

        public FichaEmpleadoController(IFichaEmpleadoService fichaEmpleadoService, InventarioDbContext context)
        {
            _fichaEmpleadoService = fichaEmpleadoService;
            _context = context;
        }

        public ActionResult Index()
        {
            var fichas = _fichaEmpleadoService.ObtenerFichas();
            return View(fichas);
        }

        public ActionResult Detalle(int id)
        {
            var ficha = _fichaEmpleadoService.ObtenerFichaPorEmpleado(id);
            if (ficha == null) return HttpNotFound();
            return View(ficha);
        }

        public ActionResult Crear()
        {
            ViewBag.Empleados = new SelectList(_context.Empleado.ToList(), "ID", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(FichaEmpleado ficha)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Empleados = new SelectList(_context.Empleado.ToList(), "ID", "Nombre", ficha.EmpleadoID);
                return View(ficha);
            }

            _fichaEmpleadoService.CrearFicha(ficha);
            return RedirectToAction("Index");
        }

        public ActionResult Editar(int id)
        {
            var ficha = _context.FichaEmpleado.Include("Empleado")
                .FirstOrDefault(f => f.ID == id);

            if (ficha == null) return HttpNotFound();

            return View(ficha);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, FichaEmpleado fichaEmpleado)
        {
            if (id != fichaEmpleado.ID) return HttpNotFound();

            if (ModelState.IsValid)
            {
                var fichaExistente = _context.FichaEmpleado.Include("Empleado")
                    .FirstOrDefault(f => f.ID == id);

                if (fichaExistente == null) return HttpNotFound();

                fichaExistente.Empleado.Nombre = fichaEmpleado.Empleado?.Nombre;
                fichaExistente.FechaIngreso = fichaEmpleado.FechaIngreso;
                fichaExistente.FechaTerminoContrato = fichaEmpleado.FechaTerminoContrato;
                fichaExistente.FechaVencimientoExamen = fichaEmpleado.FechaVencimientoExamen;
                fichaExistente.CursoAltura = fichaEmpleado.CursoAltura;
                fichaExistente.Acreditaciones = fichaEmpleado.Acreditaciones;

                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fichaEmpleado);
        }

        public ActionResult Eliminar(int id)
        {
            var eliminado = _fichaEmpleadoService.EliminarFicha(id);
            if (!eliminado) return HttpNotFound();

            return RedirectToAction("Index");
        }

        private bool FichaEmpleadoExists(int id)
        {
            return _context.FichaEmpleado.Any(e => e.ID == id);
        }
    }
}
