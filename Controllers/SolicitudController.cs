using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Inventario360.Web.Data;
using Inventario360.Web.Models;
using Inventario360.Web.Services;
using OfficeOpenXml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing.Printing;
using System.Drawing;
using System.Xml.Linq;

namespace Inventario360.Web.Controllers
{
    public class SolicitudesController : Controller
    {
        private readonly ISolicitudService _solicitudService;
        private readonly IProductoService _productoService;
        private readonly InventarioDbContext _context;

        public SolicitudesController(ISolicitudService solicitudService, IProductoService productoService, InventarioDbContext context)
        {
            _solicitudService = solicitudService;
            _productoService = productoService;
            _context = context;
        }

        public ActionResult Index()
        {
            ViewBag.Proveedores = new SelectList(_context.Proveedor.ToList(), "ID", "Nombre");
            return View();
        }

        [HttpPost]
        public JsonResult AgregarDesdeInventario(int productoId, int cantidad, string medida, string unidadMedida, string marca, string posibleProveedor)
        {
            var producto = _productoService.ObtenerPorId(productoId);
            if (producto == null)
            {
                return Json(new { success = false, message = "Producto no encontrado" });
            }

            var solicitud = new SolicitudDeMaterial
            {
                ITEM = producto.ITEM,
                NombreTecnico = producto.NombreTecnico,
                Descripcion = producto.Descripcion,
                Imagen = producto.Imagen,
                Cantidad = cantidad,
                Medida = medida,
                UnidadMedida = unidadMedida,
                Marca = marca,
                PosibleProveedor = posibleProveedor,
                Fecha = DateTime.Now,
                Estado = "Pendiente"
            };

            _solicitudService.AddSolicitud(solicitud);


            return Json(new { success = true, solicitud });
        }

        [HttpPost]
        public ActionResult CrearNuevoProducto(SolicitudDeMaterial solicitud)
        {
            if (!ModelState.IsValid)
            {
                return View("Crear", solicitud);
            }

            solicitud.Fecha = DateTime.Now;
            solicitud.Estado = "Pendiente";

            _solicitudService.AddSolicitud(solicitud);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult ObtenerProductos()
        {
            var productos = _productoService.ObtenerTodos();

            var productosDTO = productos.Select(p => new
            {
                item = p.ITEM,
                nombreTecnico = p.NombreTecnico,
                descripcion = p.Descripcion ?? "",
                marca = p.Marca ?? "",
                imagen = p.Imagen ?? "".ToLowerInvariant(),
                posibleProveedor = p.Proveedor ?? 0
            }).ToList();

            return Json(productosDTO, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Detalle(int id)
        {
            var solicitud = _solicitudService.GetSolicitudById(id);

            if (solicitud == null)
            {
                return HttpNotFound();
            }
            return View(solicitud);
        }
        [HttpPost]
        public ActionResult Eliminar(int id)
        {
            var solicitud = _solicitudService.GetSolicitudById(id);

            if (solicitud == null)
            {
                return HttpNotFound();
            }

            _solicitudService.DeleteSolicitud(id);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Crear()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DescargarExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Solicitudes");

                worksheet.Cells[1, 1].Value = "Nombre del Material";
                worksheet.Cells[1, 2].Value = "Cantidad";
                worksheet.Cells[1, 3].Value = "Medida";
                worksheet.Cells[1, 4].Value = "Unidad de Medida";
                worksheet.Cells[1, 5].Value = "Marca";
                worksheet.Cells[1, 6].Value = "Descripción";
                worksheet.Cells[1, 7].Value = "Posible Proveedor";

                var solicitudes = _solicitudService.ObtenerTodas();

                if (solicitudes == null || !solicitudes.Any())
                {
                    return new HttpStatusCodeResult(400, "No hay datos para exportar.");
                }

                int fila = 2;
                foreach (var solicitud in solicitudes)
                {
                    worksheet.Cells[fila, 1].Value = solicitud.NombreTecnico;
                    worksheet.Cells[fila, 2].Value = solicitud.Cantidad;
                    worksheet.Cells[fila, 3].Value = solicitud.Medida;
                    worksheet.Cells[fila, 4].Value = solicitud.UnidadMedida;
                    worksheet.Cells[fila, 5].Value = solicitud.Marca;
                    worksheet.Cells[fila, 6].Value = solicitud.Descripcion;
                    worksheet.Cells[fila, 7].Value = solicitud.PosibleProveedor;
                    fila++;
                }

                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string nombreArchivo = $"Solicitudes_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
            }
        }
        [HttpPost]
        public ActionResult DescargarPDF(List<SolicitudDeMaterial> solicitudes)
        {
            if (solicitudes == null || !solicitudes.Any())
            {
                return new HttpStatusCodeResult(400, "No hay datos para exportar.");
            }

            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    Document document = new Document(PageSize.A4.Rotate());
                    PdfWriter.GetInstance(document, stream);
                    document.Open();

                    iTextSharp.text.Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                    iTextSharp.text.Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

                    Paragraph titulo = new Paragraph("Solicitud de Materiales", titleFont);
                    document.Add(titulo);

                    document.Add(new Paragraph("\n"));
                    Paragraph subtitulo = new Paragraph("Solicito estos materiales para esta semana:", normalFont);
                    document.Add(subtitulo);

                    document.Add(new Paragraph("\n"));

                    PdfPTable table = new PdfPTable(8);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 3, 1, 1, 1, 2, 3, 2, 2 });

                    string[] headers = { "Nombre", "Cantidad", "Medida", "Unidad", "Marca", "Descripción", "Proveedor", "Imagen" };
                    BaseColor headerColor = new BaseColor(255, 204, 153);

                    foreach (var header in headers)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(header, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
                        cell.BackgroundColor = headerColor;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Padding = 5f;
                        table.AddCell(cell);
                    }

                    foreach (var solicitud in solicitudes)
                    {
                        table.AddCell(new PdfPCell(new Phrase(solicitud.NombreTecnico ?? "")) { Padding = 5f });
                        table.AddCell(new PdfPCell(new Phrase(solicitud.Cantidad.ToString())) { Padding = 5f });
                        table.AddCell(new PdfPCell(new Phrase(solicitud.Medida ?? "")) { Padding = 5f });
                        table.AddCell(new PdfPCell(new Phrase(solicitud.UnidadMedida ?? "")) { Padding = 5f });
                        table.AddCell(new PdfPCell(new Phrase(solicitud.Marca ?? "")) { Padding = 5f });
                        table.AddCell(new PdfPCell(new Phrase(solicitud.Descripcion ?? "")) { Padding = 5f });

                        string proveedorFinal = !string.IsNullOrEmpty(solicitud.PosibleProveedor) ? solicitud.PosibleProveedor : "Sin proveedor";
                        table.AddCell(new PdfPCell(new Phrase(proveedorFinal, FontFactory.GetFont(FontFactory.HELVETICA, 10))) { Padding = 5f });

                        PdfPCell imgCell = new PdfPCell { Padding = 5f, HorizontalAlignment = Element.ALIGN_CENTER };
                        if (!string.IsNullOrEmpty(solicitud.Imagen))
                        {
                            iTextSharp.text.Image img = null;

                            if (solicitud.Imagen.StartsWith("data:image"))
                            {
                                // Imagen en Base64
                                var base64Data = solicitud.Imagen.Substring(solicitud.Imagen.IndexOf(",") + 1);
                                byte[] imageBytes = Convert.FromBase64String(base64Data);
                                img = iTextSharp.text.Image.GetInstance(imageBytes);
                            }
                            else
                            {
                                // Imagen desde archivo físico
                                var imagePath = Path.Combine(Server.MapPath("~/images"), solicitud.Imagen);
                                if (System.IO.File.Exists(imagePath))
                                {
                                    img = iTextSharp.text.Image.GetInstance(imagePath);
                                }
                            }

                            if (img != null)
                            {
                                img.ScaleToFit(50f, 50f);
                                imgCell.AddElement(img);
                            }
                            else
                            {
                                imgCell.AddElement(new Phrase("No disponible"));
                            }
                        }
                        else
                        {
                            imgCell.AddElement(new Phrase("No disponible"));
                        }


                        table.AddCell(imgCell);
                    }

                    document.Add(table);
                    document.Close();

                    return File(stream.ToArray(), "application/pdf", "Solicitud_Material.pdf");
                }
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, "Error al generar PDF: " + ex.Message);
            }
        }



        [HttpPost]
        public JsonResult SubirImagen(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
            {
                return Json(new { success = false, message = "No se ha proporcionado ninguna imagen." });
            }

            string uploadsFolder = Server.MapPath("~/images");
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploadsFolder, fileName);

            file.SaveAs(filePath);

            return Json(new { success = true, fileName });
        }
    }
}
