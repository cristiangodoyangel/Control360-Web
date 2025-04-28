using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using Inventario360.Web.Services;
using Inventario360.Web.Services.Implementaciones;
using Inventario360.Web.Data;
using System.Data.Entity.Core.Metadata.Edm;
using Inventario360.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using Unity.Injection;
using Unity.Lifetime;

namespace Inventario360.Web
{
    public static class DependencyConfig
    {
        public static void RegisterDependencies()
        {
            var container = new UnityContainer();

            // Registro de servicios e interfaces
            container.RegisterType<IProductoService, ProductoService>();
            container.RegisterType<IProveedorService, ProveedorService>();
            container.RegisterType<ISalidaBodegaService, SalidaBodegaService>();
            container.RegisterType<IEmpleadoService, EmpleadoService>();
            container.RegisterType<IProyectoService, ProyectoService>();
            container.RegisterType<ISolicitudService, SolicitudService>();



            container.RegisterType<InventarioDbContext, InventarioDbContext>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            container.RegisterType<DbContext, InventarioDbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new InjectionConstructor(typeof(InventarioDbContext)));
            container.RegisterType<ApplicationUserManager>();
            container.RegisterType<ApplicationSignInManager>();
        }

    }
}
