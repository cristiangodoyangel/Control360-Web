using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using Inventario360.Web.Services;
using Unity.AspNet.Mvc;

namespace Inventario360.Web
{
    public static class DependencyConfig
    {
        public static void RegisterDependencies()
        {
            var container = new UnityContainer();

            // Registrar servicios aquí
            container.RegisterType<IProductoService, ProductoService>();
            container.RegisterType<ISalidaBodegaService, SalidaBodegaService>();
            container.RegisterType<ISolicitudService, SolicitudService>();
            container.RegisterType<IEmpleadoService, EmpleadoService>();
            container.RegisterType<IProyectoService, ProyectoService>();
            container.RegisterType<ICuentaService, CuentaService>();
            container.RegisterType<IReporteService, ReporteService>();
            container.RegisterType<IFichaEmpleadoService, FichaEmpleadoService>();
            container.RegisterType<IFichaCamionetaService, FichaCamionetaService>();
            container.RegisterType<IUsuarioService, UsuarioService>();
            container.RegisterType<IProveedorService, ProveedorService>();

            // Aplicar el contenedor a MVC
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
