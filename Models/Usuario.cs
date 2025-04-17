using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace Inventario360.Web.Models
{
    public class Usuario : IdentityUser
    {
        [Required]
        [StringLength(255)]
        public string NombreCompleto { get; set; }
    }
}
