using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Inventario360.Web.Models;
using Inventario360.Web.Data;
using Inventario360.Web.Services;
using System.IO;
using System.Web;
using System;

namespace Inventario360.Web.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly IProveedorService _proveedorService;
        private readonly InventarioDbContext _context;

        public ProductosController(
            IProductoService productoService,
            IProveedorService proveedorService,
            InventarioDbContext context)
        {
            _productoService = productoService;
            _proveedorService = proveedorService;
            _context = context;
        }

        public ActionResult Index()
        {
            var productos = _context.Database.SqlQuery<Producto>(
                @"SELECT ITEM, Cantidad, NombreTecnico, Medida, UnidadMedida, Marca,
                         Descripcion, Imagen, Proveedor, Ubicacion, Estado, Categoria
                  FROM Producto").ToList();

            return View(productos);
        }

        public ActionResult Detalle(int id)
        {
            try
            {
                var producto = _productoService.ObtenerPorId(id);
                if (producto == null)
                {
                    ViewBag.Mensaje = "No se encontró el producto con ID " + id;
                    return View("Error");
                }

                ViewBag.NombreProveedor = _proveedorService.ObtenerNombreProveedor(producto.Proveedor);
                return View(producto);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = "Error inesperado: " + ex.Message;
                return View("Error");
            }
        }




        [HttpGet]
        public ActionResult Editar(int id)
        {
            var producto = _context.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }

            ViewBag.Proveedores = new SelectList(
                _context.Proveedor.Select(p => new { Value = p.ID.ToString(), Text = p.Nombre }),
                "Value",
                "Text"
            );

            return View(producto);
        }

        [HttpPost]
        public ActionResult Editar(int id, Producto producto, HttpPostedFileBase ImagenArchivo)
        {
            if (id != producto.ITEM)
            {
                return HttpNotFound();
            }

            var productoExistente = _context.Producto.Find(id);
            if (productoExistente == null)
            {
                return HttpNotFound();
            }

            if (ImagenArchivo != null && ImagenArchivo.ContentLength > 0)
            {
                var uploadsPath = Server.MapPath("~/images");
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImagenArchivo.FileName);
                var filePath = Path.Combine(uploadsPath, fileName);

                ImagenArchivo.SaveAs(filePath);

                // Eliminar imagen anterior
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

            _context.Entry(productoExistente).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
