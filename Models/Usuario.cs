using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace Inventario360.Web.Models
{
    public class Usuario : IdentityUser
    {
        [Required]
        [StringLength(150)]
        public string NombreCompleto { get; set; }
    }
}
