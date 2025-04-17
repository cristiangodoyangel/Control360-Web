namespace Inventario360.Web.Models
{
    public class AspNetUserTokens
    {
        public string UserId { get; set; } // Identificador del usuario
        public string LoginProvider { get; set; } // Proveedor de autenticación (Ej: Google, Facebook)
        public string Name { get; set; } // Nombre del token
        public string Value { get; set; } // Valor del token (puede ser NULL)
    }
}
