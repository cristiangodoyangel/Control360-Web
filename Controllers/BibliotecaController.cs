using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventario360.Web.Controllers
{
    public class BibliotecaController : Controller
    {
        private readonly string _documentosPath;

        public BibliotecaController()
        {
            _documentosPath = System.Web.Hosting.HostingEnvironment.MapPath("~/documentos");
        }

        public ActionResult Index()
        {
            var categorias = new List<string> { "Camionetas", "Proyectos", "Trabajadores", "HSEC" };
            var archivosPorCategoria = new Dictionary<string, List<string>>();

            foreach (var categoria in categorias)
            {
                string rutaCategoria = Path.Combine(_documentosPath, categoria);
                if (!Directory.Exists(rutaCategoria))
                {
                    Directory.CreateDirectory(rutaCategoria);
                }

                var archivos = Directory.GetFiles(rutaCategoria).Select(Path.GetFileName).ToList();
                archivosPorCategoria[categoria] = archivos;
            }

            ViewBag.ArchivosPorCategoria = archivosPorCategoria;
            return View();
        }

        public ActionResult Descargar(string categoria, string nombreArchivo)
        {
            string rutaArchivo = Path.Combine(_documentosPath, categoria, nombreArchivo);
            if (System.IO.File.Exists(rutaArchivo))
            {
                return File(rutaArchivo, "application/octet-stream", nombreArchivo);
            }
            return HttpNotFound();
        }

        public ActionResult Camionetas()
        {
            return CargarCategoria("Camionetas");
        }

        public ActionResult Trabajadores()
        {
            return CargarCategoria("Trabajadores");
        }

        public ActionResult HSEC()
        {
            return CargarCategoria("HSEC");
        }

        public ActionResult Proyectos()
        {
            return CargarCategoria("Proyectos");
        }

        private ActionResult CargarCategoria(string categoria)
        {
            string rutaCategoria = Path.Combine(_documentosPath, categoria);
            if (!Directory.Exists(rutaCategoria))
            {
                Directory.CreateDirectory(rutaCategoria);
            }

            var archivos = Directory.GetFiles(rutaCategoria).Select(Path.GetFileName).ToList();
            ViewBag.Categoria = categoria;
            ViewBag.Archivos = archivos;
            return View("Categoria");
        }

        [HttpPost]
        public JsonResult SubirDocumento(string categoria, HttpPostedFileBase archivo, string titulo)
        {
            if (archivo == null || archivo.ContentLength == 0 || string.IsNullOrEmpty(titulo))
            {
                return Json(new { success = false, message = "Debe proporcionar un archivo y un título." });
            }

            string carpetaDestino = Path.Combine(_documentosPath, categoria);
            if (!Directory.Exists(carpetaDestino))
            {
                Directory.CreateDirectory(carpetaDestino);
            }

            string nombreArchivo = $"{titulo}{Path.GetExtension(archivo.FileName)}";
            string rutaArchivo = Path.Combine(carpetaDestino, nombreArchivo);

            archivo.SaveAs(rutaArchivo);

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult EliminarDocumento(string categoria, string archivo)
        {
            if (string.IsNullOrEmpty(archivo))
            {
                return Json(new { success = false, message = "El nombre del archivo es inválido." });
            }

            string rutaArchivo = Path.Combine(_documentosPath, categoria, archivo);

            if (System.IO.File.Exists(rutaArchivo))
            {
                try
                {
                    System.IO.File.Delete(rutaArchivo);
                    return Json(new { success = true });
                }
                catch
                {
                    return Json(new { success = false, message = "Error al eliminar el archivo." });
                }
            }

            return Json(new { success = false, message = "El archivo no existe." });
        }
    }
}
