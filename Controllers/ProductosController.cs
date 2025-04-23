using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using Inventario360.Web.Models;
using Inventario360.Web.Data;

namespace Inventario360.Web.Controllers
{
    public class ProductosController : Controller
    {

        private readonly InventarioDbContext _context;

        public ProductosController()
        {
            _context = new InventarioDbContext();
        }

        public ActionResult Index()
        {
            var productos = _context.Database.SqlQuery<Producto>(
                @"SELECT ITEM, Cantidad, NombreTecnico, Medida, UnidadMedida, Marca,
                         Descripcion, Imagen, Proveedor, Ubicacion, Estado, Categoria
                  FROM Producto").ToList();

            return View(productos);
        }





    }
}
