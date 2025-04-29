namespace Inventario360.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Productoes",
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Productoes");
        }
    }
}
