using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Inventario360.Web.Models;
using Inventario360.Web.Services;
using Inventario360.Web.Data;
using System.Data.Entity;


namespace Inventario360.Web.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly InventarioDbContext _context;

        public ProductosController(IProductoService productoService, InventarioDbContext context)
        {
            _productoService = productoService;
            _context = context;
        }

        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var productos = await _productoService.ObtenerTodos();
            return View(productos);
        }

        [AllowAnonymous]
        public async Task<ActionResult> Detalle(int id)
        {
            var producto = await _productoService.ObtenerPorId(id);
            if (producto == null) return HttpNotFound();

            var proveedor = await _context.Proveedor.FindAsync(producto.Proveedor);
            ViewBag.NombreProveedor = proveedor != null ? proveedor.Nombre : "Proveedor no definido";

            return View(producto);
        }

        [Authorize(Roles = "Administrador,Proyectos")]
        public async Task<ActionResult> Crear()
        {
            ViewBag.Proveedores = new SelectList(await _context.Proveedor.ToListAsync(), "ID", "Nombre");
            return View();
        }

        [Authorize(Roles = "Administrador,Proyectos")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear(Producto producto, HttpPostedFileBase ImagenArchivo)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Proveedores = new SelectList(await _context.Proveedor.ToListAsync(), "ID", "Nombre");
                return View(producto);
            }

            if (ImagenArchivo != null && ImagenArchivo.ContentLength > 0)
            {
                var folder = Server.MapPath("~/images");
                var fileName = $"{producto.ITEM}_{Path.GetFileName(ImagenArchivo.FileName)}";
                var filePath = Path.Combine(folder, fileName);
                ImagenArchivo.SaveAs(filePath);
                producto.Imagen = fileName;
            }

            await _productoService.Agregar(producto);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrador,Proyectos")]
        public async Task<ActionResult> Editar(int id)
        {
            var producto = await _productoService.ObtenerPorId(id);
            if (producto == null) return HttpNotFound();

            ViewBag.Proveedores = new SelectList(await _context.Proveedor.ToListAsync(), "ID", "Nombre");
            return View(producto);
        }

        [Authorize(Roles = "Administrador,Proyectos")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(int id, Producto producto, HttpPostedFileBase ImagenArchivo)
        {
            if (id != producto.ITEM) return HttpNotFound();

            var productoExistente = await _context.Producto.FindAsync(id);
            if (productoExistente == null) return HttpNotFound();

            if (ImagenArchivo != null && ImagenArchivo.ContentLength > 0)
            {
                var uploadsPath = Server.MapPath("~/images");
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImagenArchivo.FileName);
                var filePath = Path.Combine(uploadsPath, fileName);
                ImagenArchivo.SaveAs(filePath);

                if (!string.IsNullOrEmpty(productoExistente.Imagen))
                {
                    var oldImagePath = Path.Combine(uploadsPath, productoExistente.Imagen);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                productoExistente.Imagen = fileName;
            }

            productoExistente.NombreTecnico = producto.NombreTecnico;
            productoExistente.Medida = producto.Medida;
            productoExistente.UnidadMedida = producto.UnidadMedida;
            productoExistente.Marca = producto.Marca;
            productoExistente.Cantidad = producto.Cantidad;
            productoExistente.Descripcion = producto.Descripcion;
            productoExistente.Ubicacion = producto.Ubicacion;
            productoExistente.Estado = producto.Estado;
            productoExistente.Proveedor = producto.Proveedor;
            productoExistente.Categoria = producto.Categoria;

            _context.Entry(productoExistente).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<JsonResult> Eliminar(int id)
        {
            try
            {
                var producto = await _context.Producto.FindAsync(id);
                if (producto == null)
                {
                    return Json(new { success = false, message = "No se encontró el producto." });
                }

                var tieneDependencias = await _context.DetalleSalidaDeBodega.AnyAsync(d => d.ProductoID == id);
                if (tieneDependencias)
                {
                    return Json(new { success = false, message = "No se puede eliminar el producto porque está asociado a una Entrega de Materiales." });
                }

                _context.Producto.Remove(producto);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error interno: " + ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<JsonResult> ObtenerProductos()
        {
            try
            {
                var productos = await _productoService.ObtenerTodos();
                if (productos == null || !productos.Any())
                {
                    return Json(new { error = "No se encontraron productos." }, JsonRequestBehavior.AllowGet);
                }

                return Json(productos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Error al obtener productos", detalle = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
