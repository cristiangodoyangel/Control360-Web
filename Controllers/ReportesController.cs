using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Inventario360.Web.Data;
using Inventario360.Web.Services;

namespace Inventario360.Web.Controllers
{
    public class ReportesController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly InventarioDbContext _context;

        public ReportesController(IProductoService productoService, InventarioDbContext context)
        {
            _productoService = productoService;
            _context = context;
        }

        public ActionResult Index()
        {
            var salidas = _context.SalidaDeBodega
                .Include("Detalles.Producto")
                .Include("SolicitanteObj")
                .Include("ResponsableEntregaObj")
                .Include("ProyectoObj")
                .ToList();

            return View(salidas);
        }

        public ActionResult Inventario()
        {
            return View();
        }

        public ActionResult Salidas()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ObtenerDatosReportes()
        {
            var productos = _productoService.ObtenerTodos();

            var productosPorEstado = productos
                .GroupBy(p => p.Estado)
                .Select(g => new { estado = g.Key ?? "Sin Estado", cantidad = g.Count() })
                .ToList<object>();

            var productosPorCategoria = productos
                .GroupBy(p => p.Categoria)
                .Select(g => new { categoria = g.Key ?? "Sin Categoría", cantidad = g.Count() })
                .ToList<object>();

            int totalInventario = productos.Count();
            int productosStockBajo = productos.Count(p => p.Cantidad < 5);
            int productosOverstock = productos.Count(p => p.Cantidad > 100);

            DateTime fechaFiltro = DateTime.Now.AddMonths(-1);

            var materialesMasSolicitados = _context.SalidaDeBodega
                .Where(s => s.Fecha >= fechaFiltro && s.Detalles.Any())
                .SelectMany(s => s.Detalles)
                .Where(d => d.Producto != null)
                .GroupBy(d => d.Producto.NombreTecnico)
                .Select(g => new { material = g.Key, totalSolicitudes = g.Sum(d => d.Cantidad) })
                .OrderByDescending(g => g.totalSolicitudes)
                .Take(3)
                .ToList<object>();

            var empleadoMasSolicitante = _context.SalidaDeBodega
                .Where(s => s.Fecha >= fechaFiltro && s.SolicitanteObj != null)
                .GroupBy(s => s.SolicitanteObj.Nombre)
                .Select(g => new { empleado = g.Key, totalSolicitudes = g.Count() })
                .OrderByDescending(g => g.totalSolicitudes)
                .FirstOrDefault() ?? new { empleado = "Sin datos", totalSolicitudes = 0 };

            var proyectoMasSolicitado = _context.SalidaDeBodega
                .Where(s => s.Fecha >= fechaFiltro && s.ProyectoObj != null)
                .GroupBy(s => s.ProyectoObj.Nombre)
                .Select(g => new { proyecto = g.Key, totalSolicitudes = g.Count() })
                .OrderByDescending(g => g.totalSolicitudes)
                .FirstOrDefault() ?? new { proyecto = "Sin datos", totalSolicitudes = 0 };

            return Json(new
            {
                productosPorEstado,
                productosPorCategoria,
                totalInventario,
                productosStockBajo,
                productosOverstock,
                materialesMasSolicitados,
                empleadoMasSolicitante,
                proyectoMasSolicitado
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerInventario()
        {
            var productos = _productoService.ObtenerTodos();

            var inventarioCompleto = productos.ToList();
            var stockCritico = productos.Where(p => p.Cantidad < 5).ToList();
            var overstock = productos.Where(p => p.Cantidad > 100).ToList();

            return Json(new
            {
                inventarioCompleto,
                stockCritico,
                overstock
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
