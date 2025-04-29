namespace Inventario360.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActualizarModeloConIdentity : DbMigration
    {
        public override void Up()
        {
           
            
          
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
            DropTable("dbo.DetalleSalidaDeBodega");
            DropTable("dbo.Empleado");
            DropTable("dbo.Camionetas");
            RenameTable(name: "dbo.Producto", newName: "Productoes");
        }
    }
}
