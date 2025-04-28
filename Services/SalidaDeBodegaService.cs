using System;
using System.Collections.Generic;
using System.Linq;
using Inventario360.Web.Data;
using Inventario360.Web.Models;
using Inventario360.Web.Services;

namespace Inventario360.Web.Services.Implementaciones
{
    public class SalidaBodegaService : ISalidaBodegaService
    {
        private readonly InventarioDbContext _context;

        public SalidaBodegaService(InventarioDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SalidaDeBodega> ObtenerTodas()
        {
            return _context.SalidaDeBodega
                .Include("SolicitanteObj")
                .Include("ResponsableEntregaObj")
                .Include("ProyectoObj")
                .ToList();
        }

        public SalidaDeBodega ObtenerPorId(int id)
        {
            return _context.SalidaDeBodega
                .Include("SolicitanteObj")
                .Include("ResponsableEntregaObj")
                .Include("ProyectoObj")
                .Include("Detalles.Producto")
                .FirstOrDefault(s => s.ID == id);
        }

        public List<DetalleSalidaDeBodega> ObtenerDetallesPorSalida(int id)
        {
            return _context.DetalleSalidaDeBodega
                .Include("Producto")
                .Where(d => d.SalidaDeBodegaID == id)
                .ToList();
        }

        public bool RegistrarSalidaConProductos(SalidaDeBodega salida, List<DetalleSalidaDeBodega> productos)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (productos == null || productos.Count == 0)
                        return false;

                    salida.Fecha = DateTime.Now;
                    _context.SalidaDeBodega.Add(salida);
                    _context.SaveChanges();

                    foreach (var detalle in productos)
                    {
                        var producto = _context.Producto.Find(detalle.ProductoID);
                        if (producto == null || producto.Cantidad < detalle.Cantidad)
                        {
                            transaction.Rollback();
                            return false;
                        }

                        producto.Cantidad -= detalle.Cantidad;
                        _context.Entry(producto).State = System.Data.Entity.EntityState.Modified;

                        detalle.SalidaDeBodegaID = salida.ID;
                        _context.DetalleSalidaDeBodega.Add(detalle);
                    }

                    _context.SaveChanges();
                    transaction.Commit();

                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public void Eliminar(int id)
        {
            var salida = _context.SalidaDeBodega.Find(id);
            if (salida != null)
            {
                _context.SalidaDeBodega.Remove(salida);
                _context.SaveChanges();
            }
        }

        public void EliminarDetalles(int salidaId)
        {
            var detalles = _context.DetalleSalidaDeBodega.Where(d => d.SalidaDeBodegaID == salidaId);
            _context.DetalleSalidaDeBodega.RemoveRange(detalles);
            _context.SaveChanges();
        }

        public void RevertirStock(int salidaId)
        {
            var detalles = _context.DetalleSalidaDeBodega
                .Include("Producto")
                .Where(d => d.SalidaDeBodegaID == salidaId)
                .ToList();

            if (detalles == null || detalles.Count == 0)
                return;

            foreach (var detalle in detalles)
            {
                if (detalle.Producto != null)
                {
                    detalle.Producto.Cantidad += detalle.Cantidad;
                    _context.Entry(detalle.Producto).State = System.Data.Entity.EntityState.Modified;
                }
            }

            _context.SaveChanges();
        }

        public object ObtenerDatosResumenSalidas(int mes, int año)
        {
            var proyectosMasSolicitados = _context.SalidaDeBodega
                .Where(s => s.Fecha.HasValue && s.Fecha.Value.Month == mes && s.Fecha.Value.Year == año)
                .GroupBy(s => s.ProyectoAsignado)
                .OrderByDescending(g => g.Count())
                .Select(g => new { Proyecto = g.Key, Total = g.Count() })
                .FirstOrDefault();

            var empleadosMasSolicitantes = _context.SalidaDeBodega
                .Where(s => s.Fecha.HasValue && s.Fecha.Value.Month == mes && s.Fecha.Value.Year == año)
                .GroupBy(s => s.Solicitante)
                .OrderByDescending(g => g.Count())
                .Select(g => new { Empleado = g.Key, Total = g.Count() })
                .FirstOrDefault();

            var materialesMasSolicitados = _context.DetalleSalidaDeBodega
                .Where(d => d.SalidaDeBodega.Fecha.HasValue && d.SalidaDeBodega.Fecha.Value.Month == mes && d.SalidaDeBodega.Fecha.Value.Year == año)
                .GroupBy(d => d.Producto.NombreTecnico)
                .OrderByDescending(g => g.Sum(d => d.Cantidad))
                .Select(g => new { Material = g.Key, TotalCantidad = g.Sum(d => d.Cantidad) })
                .Take(3)
                .ToList();

            return new
            {
                proyectosMasSolicitados,
                empleadosMasSolicitantes,
                materialesMasSolicitados
            };
        }
    }
}
