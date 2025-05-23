﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventario360.Web.Models
{
    public class FichaCamioneta
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string Patente { get; set; }

        [Required]
        [StringLength(50)]
        public string Marca { get; set; }

        [Required]
        [StringLength(50)]
        public string Modelo { get; set; }

        [Required]
        public int Año { get; set; }

        [Required]
        [StringLength(20)]
        public string Color { get; set; }

        [Required]
        public int Kilometraje { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; }

        // ✅ Primero el ID como int normal
        [Required]
        public int ResponsableID { get; set; }

        // ✅ Luego la navegación virtual
        [ForeignKey("ResponsableID")]
        public virtual Empleado Responsable { get; set; }

        public DateTime? FechaMantenimiento { get; set; }
        public DateTime? FechaPermisoCirculacion { get; set; }
        public DateTime? FechaSoap { get; set; }
        public DateTime? FechaAntivuelcos { get; set; }
        public DateTime? FechaLaminas { get; set; }
        public DateTime? FechaGPS { get; set; }

        [StringLength(50)]
        public string Acreditacion { get; set; }
    }
}
