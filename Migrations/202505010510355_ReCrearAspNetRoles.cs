namespace Inventario360.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReCrearAspNetRoles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Camionetas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Patente = c.String(nullable: false, maxLength: 20),
                        Marca = c.String(nullable: false, maxLength: 50),
                        Modelo = c.String(nullable: false, maxLength: 50),
                        Año = c.Int(nullable: false),
                        Color = c.String(nullable: false, maxLength: 20),
                        Kilometraje = c.Int(nullable: false),
                        Estado = c.String(nullable: false, maxLength: 20),
                        ResponsableID = c.Int(),
                        FechaMantenimiento = c.DateTime(),
                        FechaPermisoCirculacion = c.DateTime(),
                        FechaSoap = c.DateTime(),
                        FechaAntivuelcos = c.DateTime(),
                        FechaLaminas = c.DateTime(),
                        FechaGPS = c.DateTime(),
                        Acreditacion = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Empleado", t => t.ResponsableID)
                .Index(t => t.ResponsableID);
            
            CreateTable(
                "dbo.Empleado",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(maxLength: 255),
                        Cargo = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.DetalleSalidaDeBodega",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SalidaDeBodegaID = c.Int(nullable: false),
                        ProductoID = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Producto", t => t.ProductoID, cascadeDelete: true)
                .ForeignKey("dbo.SalidaDeBodega", t => t.SalidaDeBodegaID, cascadeDelete: true)
                .Index(t => t.SalidaDeBodegaID)
                .Index(t => t.ProductoID);
            
            CreateTable(
                "dbo.Producto",
                c => new
                    {
                        ITEM = c.Int(nullable: false, identity: true),
                        Cantidad = c.Int(),
                        NombreTecnico = c.String(nullable: false, maxLength: 255),
                        Medida = c.String(maxLength: 50),
                        UnidadMedida = c.String(maxLength: 50),
                        Marca = c.String(maxLength: 255),
                        Descripcion = c.String(),
                        Imagen = c.String(maxLength: 255),
                        Proveedor = c.Int(),
                        Ubicacion = c.String(maxLength: 255),
                        Estado = c.String(maxLength: 50),
                        Categoria = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.ITEM);
            
            CreateTable(
                "dbo.SalidaDeBodega",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(),
                        Solicitante = c.Int(nullable: false),
                        ResponsableEntrega = c.Int(nullable: false),
                        ProyectoAsignado = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Proyecto", t => t.ProyectoAsignado)
                .ForeignKey("dbo.Empleado", t => t.ResponsableEntrega)
                .ForeignKey("dbo.Empleado", t => t.Solicitante)
                .Index(t => t.Solicitante)
                .Index(t => t.ResponsableEntrega)
                .Index(t => t.ProyectoAsignado);
            
            CreateTable(
                "dbo.Proyecto",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(maxLength: 255),
                        Descripcion = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.FichaCamionetas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Patente = c.String(nullable: false, maxLength: 20),
                        Marca = c.String(nullable: false, maxLength: 50),
                        Modelo = c.String(nullable: false, maxLength: 50),
                        Año = c.Int(nullable: false),
                        Color = c.String(nullable: false, maxLength: 20),
                        Kilometraje = c.Int(nullable: false),
                        Estado = c.String(nullable: false, maxLength: 20),
                        ResponsableID = c.Int(nullable: false),
                        FechaMantenimiento = c.DateTime(),
                        FechaPermisoCirculacion = c.DateTime(),
                        FechaSoap = c.DateTime(),
                        FechaAntivuelcos = c.DateTime(),
                        FechaLaminas = c.DateTime(),
                        FechaGPS = c.DateTime(),
                        Acreditacion = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Empleado", t => t.ResponsableID, cascadeDelete: true)
                .Index(t => t.ResponsableID);
            
            CreateTable(
                "dbo.FichaEmpleado",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EmpleadoID = c.Int(nullable: false),
                        FechaIngreso = c.DateTime(nullable: false),
                        FechaTerminoContrato = c.DateTime(),
                        FechaVencimientoExamen = c.DateTime(),
                        CursoAltura = c.Boolean(nullable: false),
                        Acreditaciones = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Empleado", t => t.EmpleadoID, cascadeDelete: true)
                .Index(t => t.EmpleadoID);
            
            CreateTable(
                "dbo.Proveedor",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(maxLength: 255),
                        Contacto = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.SolicitudDeMaterials",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ITEM = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        NombreTecnico = c.String(),
                        Medida = c.String(),
                        UnidadMedida = c.String(),
                        Marca = c.String(),
                        Descripcion = c.String(),
                        Imagen = c.String(),
                        PosibleProveedor = c.String(),
                        Solicitante = c.String(),
                        Fecha = c.DateTime(nullable: false),
                        Estado = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        NombreCompleto = c.String(nullable: false, maxLength: 150),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.FichaEmpleado", "EmpleadoID", "dbo.Empleado");
            DropForeignKey("dbo.FichaCamionetas", "ResponsableID", "dbo.Empleado");
            DropForeignKey("dbo.SalidaDeBodega", "Solicitante", "dbo.Empleado");
            DropForeignKey("dbo.SalidaDeBodega", "ResponsableEntrega", "dbo.Empleado");
            DropForeignKey("dbo.SalidaDeBodega", "ProyectoAsignado", "dbo.Proyecto");
            DropForeignKey("dbo.DetalleSalidaDeBodega", "SalidaDeBodegaID", "dbo.SalidaDeBodega");
            DropForeignKey("dbo.DetalleSalidaDeBodega", "ProductoID", "dbo.Producto");
            DropForeignKey("dbo.Camionetas", "ResponsableID", "dbo.Empleado");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.FichaEmpleado", new[] { "EmpleadoID" });
            DropIndex("dbo.FichaCamionetas", new[] { "ResponsableID" });
            DropIndex("dbo.SalidaDeBodega", new[] { "ProyectoAsignado" });
            DropIndex("dbo.SalidaDeBodega", new[] { "ResponsableEntrega" });
            DropIndex("dbo.SalidaDeBodega", new[] { "Solicitante" });
            DropIndex("dbo.DetalleSalidaDeBodega", new[] { "ProductoID" });
            DropIndex("dbo.DetalleSalidaDeBodega", new[] { "SalidaDeBodegaID" });
            DropIndex("dbo.Camionetas", new[] { "ResponsableID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.SolicitudDeMaterials");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Proveedor");
            DropTable("dbo.FichaEmpleado");
            DropTable("dbo.FichaCamionetas");
            DropTable("dbo.Proyecto");
            DropTable("dbo.SalidaDeBodega");
            DropTable("dbo.Producto");
            DropTable("dbo.DetalleSalidaDeBodega");
            DropTable("dbo.Empleado");
            DropTable("dbo.Camionetas");
        }
    }
}
