using System;
using System.Collections.Generic;
using Inventario360.Web.Models;

namespace Inventario360.Web.Services
{
    public class ReporteService : IReporteService
    {
        public IEnumerable<Reporte> ObtenerDatosReportes()
        {
            return new List<Reporte>
            {
                new Reporte { Id = 1, Nombre = "Reporte de Inventario", Fecha = DateTime.Now, Descripcion = "Resumen del stock disponible" },
                new Reporte { Id = 2, Nombre = "Reporte de Gastos", Fecha = DateTime.Now, Descripcion = "Gastos totales por proyecto" }
            };
        }

        public Reporte ObtenerDetalleReporte(int id)
        {
            return new Reporte
            {
                Id = id,
                Nombre = "Reporte Detallado",
                Fecha = DateTime.Now,
                Descripcion = "Detalle específico del reporte seleccionado"
            };
        }
    }
}
