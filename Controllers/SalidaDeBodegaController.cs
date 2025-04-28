using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Inventario360.Web.Services;
using Inventario360.Web.Models;
using Inventario360.Web.Data;
using Newtonsoft.Json;
using System.Data.Entity;

namespace Inventario360.Web.Controllers
{
    public class SalidaDeBodegaController : Controller
    {
        private readonly InventarioDbContext _context;
        private readonly ISalidaBodegaService _salidaBodegaService;
        private readonly IProductoService _productoService;
        private readonly IEmpleadoService _empleadoService;
        private readonly IProyectoService _proyectoService;

        public SalidaDeBodegaController(
            InventarioDbContext context,
            ISalidaBodegaService salidaBodegaService,
            IProductoService productoService,
            IEmpleadoService empleadoService,
            IProyectoService proyectoService)
        {
            _context = context;
            _salidaBodegaService = salidaBodegaService;
            _productoService = productoService;
            _empleadoService = empleadoService;
            _proyectoService = proyectoService;
        }

        public ActionResult Index()
        {
            var salidas = _salidaBodegaService.ObtenerTodas();
            return View(salidas);
        }

        public ActionResult Detalle(int id)
        {
            var salida = _salidaBodegaService.ObtenerPorId(id);
            if (salida == null)
                return HttpNotFound();

            ViewBag.Detalles = _salidaBodegaService.ObtenerDetallesPorSalida(id);
            return View(salida);
        }


        [HttpGet]
        public async Task<ActionResult> Crear()
        {
            ViewBag.Productos = _productoService.ObtenerTodos(); // No es async
            ViewBag.Empleados = await _context.FichaEmpleado
                .Include(f => f.Empleado)
                .Select(f => f.Empleado)
                .Distinct()
                .ToListAsync(); // Sí es async

            ViewBag.Proyectos = await _proyectoService.ObtenerTodos();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(SalidaDeBodega salida, string ProductosJson)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Datos inválidos." });

            if (string.IsNullOrEmpty(ProductosJson))
                return Json(new { success = false, message = "Debe agregar al menos un producto." });

            try
            {
                var productos = JsonConvert.DeserializeObject<List<DetalleSalidaDeBodega>>(ProductosJson);
                if (productos == null || productos.Count == 0)
                    return Json(new { success = false, message = "Error al procesar productos." });

                salida.Fecha = DateTime.Now;
                bool operacionExitosa = _salidaBodegaService.RegistrarSalidaConProductos(salida, productos);

                return Json(new { success = operacionExitosa });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error interno: " + ex.Message });
            }
        }


        public async Task<ActionResult> ObtenerResumenSalidas(int mes, int año)
        {
            var resumen = new
            {
                proyectosMasSolicitados = await _context.SalidaDeBodega
                    .Where(s => s.Fecha.HasValue && s.Fecha.Value.Month == mes && s.Fecha.Value.Year == año)
                    .GroupBy(s => s.ProyectoAsignado)
                    .OrderByDescending(g => g.Count())
                    .Select(g => new
                    {
                        nombreProyecto = _context.Proyecto
                            .Where(p => p.ID == g.Key)
                            .Select(p => p.Nombre)
                            .FirstOrDefault(),
                        total = g.Count()
                    })
                    .FirstOrDefaultAsync(),

                empleadosMasSolicitantes = await _context.SalidaDeBodega
                    .Where(s => s.Fecha.HasValue && s.Fecha.Value.Month == mes && s.Fecha.Value.Year == año)
                    .GroupBy(s => s.Solicitante)
                    .OrderByDescending(g => g.Count())
                    .Select(g => new
                    {
                        nombreEmpleado = _context.Empleado
                            .Where(e => e.ID == g.Key)
                            .Select(e => e.Nombre)
                            .FirstOrDefault(),
                        total = g.Count()
                    })
                    .FirstOrDefaultAsync(),

                materialesMasSolicitados = await _context.DetalleSalidaDeBodega
                    .Where(d => d.SalidaDeBodega.Fecha.HasValue && d.SalidaDeBodega.Fecha.Value.Month == mes && d.SalidaDeBodega.Fecha.Value.Year == año)
                    .Include(d => d.Producto)
                    .GroupBy(d => d.Producto.NombreTecnico)
                    .OrderByDescending(g => g.Sum(d => d.Cantidad))
                    .Select(g => new { material = g.Key, totalCantidad = g.Sum(d => d.Cantidad) })
                    .Take(3)
                    .ToListAsync()
            };

            return Json(resumen, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Eliminar(int id)
        {
            var salida = await _context.SalidaDeBodega
                .Include(s => s.Detalles.Select(d => d.Producto))
                .FirstOrDefaultAsync(s => s.ID == id);

            if (salida == null)
                return Json(new { success = false, message = "No se encontró la salida." });

            try
            {
                foreach (var detalle in salida.Detalles)
                {
                    var producto = await _context.Producto.FindAsync(detalle.ProductoID);
                    if (producto != null)
                    {
                        producto.Cantidad += detalle.Cantidad;
                        _context.Entry(producto).State = EntityState.Modified;
                    }
                }

                _context.SalidaDeBodega.Remove(salida);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarSalida(int id)
        {
            var salida = _salidaBodegaService.ObtenerPorId(id);
            if (salida == null)
                return Json(new { success = false, message = "No se encontró la salida." });

            try
            {
                _salidaBodegaService.RevertirStock(id);
                _salidaBodegaService.EliminarDetalles(id);
                _salidaBodegaService.Eliminar(id);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar: " + ex.Message });
            }
        } 

    }
}
