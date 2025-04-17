using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Inventario360.Services;
using Inventario360.Web.Data;
using Inventario360.Web.Models;

namespace Inventario360.Web.Services
{
    public class SalidaBodegaService : ISalidaBodegaService
    {
        private readonly InventarioDbContext _context;

        public SalidaBodegaService(InventarioDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SalidaDeBodega>> ObtenerTodas()
        {
            return await _context.SalidaDeBodega
                .Include(s => s.SolicitanteObj)
                .Include(s => s.ResponsableEntregaObj)
                .Include(s => s.ProyectoObj)
                .ToListAsync();
        }

        public async Task<SalidaDeBodega> ObtenerPorId(int id)
        {
            return await _context.SalidaDeBodega
                .Include(s => s.SolicitanteObj)
                .Include(s => s.ResponsableEntregaObj)
                .Include(s => s.ProyectoObj)
                .Include(s => s.Detalles.Select(d => d.Producto))
                .FirstOrDefaultAsync(s => s.ID == id);
        }

        public async Task<List<DetalleSalidaDeBodega>> ObtenerDetallesPorSalida(int id)
        {
            return await _context.DetalleSalidaDeBodega
                .Where(d => d.SalidaDeBodegaID == id)
                .Include(d => d.Producto)
                .ToListAsync();
        }

        public async Task<bool> RegistrarSalidaConProductos(SalidaDeBodega salida, List<DetalleSalidaDeBodega> productos)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (productos == null || productos.Count == 0)
                        return false;

                    salida.Fecha = DateTime.Now;
                    _context.SalidaDeBodega.Add(salida);
                    await _context.SaveChangesAsync();

                    foreach (var detalle in productos)
                    {
                        var producto = await _context.Producto.FindAsync(detalle.ProductoID);
                        if (producto == null || producto.Cantidad < detalle.Cantidad)
                        {
                            transaction.Rollback();
                            return false;
                        }

                        producto.Cantidad -= detalle.Cantidad;
                        _context.Entry(producto).State = EntityState.Modified;

                        detalle.SalidaDeBodegaID = salida.ID;
                        _context.DetalleSalidaDeBodega.Add(detalle);
                    }

                    await _context.SaveChangesAsync();
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

        public async Task Eliminar(int id)
        {
            var salida = await _context.SalidaDeBodega.FindAsync(id);
            if (salida != null)
            {
                _context.SalidaDeBodega.Remove(salida);
                await _context.SaveChangesAsync();
            }
        }

        public async Task EliminarDetalles(int salidaId)
        {
            var detalles = _context.DetalleSalidaDeBodega.Where(d => d.SalidaDeBodegaID == salidaId);
            _context.DetalleSalidaDeBodega.RemoveRange(detalles);
            await _context.SaveChangesAsync();
        }

        public async Task RevertirStock(int salidaId)
        {
            var detalles = await _context.DetalleSalidaDeBodega
                .Where(d => d.SalidaDeBodegaID == salidaId)
                .Include(d => d.Producto)
                .ToListAsync();

            foreach (var detalle in detalles)
            {
                if (detalle.Producto != null)
                {
                    detalle.Producto.Cantidad += detalle.Cantidad;
                    _context.Entry(detalle.Producto).State = EntityState.Modified;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<object> ObtenerDatosResumenSalidas(int mes, int año)
        {
            var resumen = new
            {
                proyectosMasSolicitados = await _context.SalidaDeBodega
                    .Where(s => s.Fecha.HasValue && s.Fecha.Value.Month == mes && s.Fecha.Value.Year == año)
                    .GroupBy(s => s.ProyectoAsignado)
                    .OrderByDescending(g => g.Count())
                    .Select(g => new { Proyecto = g.Key, Total = g.Count() })
                    .FirstOrDefaultAsync(),

                empleadosMasSolicitantes = await _context.SalidaDeBodega
                    .Where(s => s.Fecha.HasValue && s.Fecha.Value.Month == mes && s.Fecha.Value.Year == año)
                    .GroupBy(s => s.Solicitante)
                    .OrderByDescending(g => g.Count())
                    .Select(g => new { Empleado = g.Key, Total = g.Count() })
                    .FirstOrDefaultAsync(),

                materialesMasSolicitados = await _context.DetalleSalidaDeBodega
                    .Where(d => d.SalidaDeBodega.Fecha.HasValue &&
                                d.SalidaDeBodega.Fecha.Value.Month == mes &&
                                d.SalidaDeBodega.Fecha.Value.Year == año)
                    .Include(d => d.Producto)
                    .GroupBy(d => d.Producto.NombreTecnico)
                    .OrderByDescending(g => g.Sum(d => d.Cantidad))
                    .Select(g => new { Material = g.Key, TotalCantidad = g.Sum(d => d.Cantidad) })
                    .Take(3)
                    .ToListAsync()
            };

            return resumen;
        }
    }
}
