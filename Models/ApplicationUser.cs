using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventario360.Web.Models
{
    [Table("AspNetUsers")] // 👈 Muy importante: mapea a la tabla real
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100)]
        public string NombreCompleto { get; set; }
    }
}
