using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvicolaApp.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre o razón social es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Razón Social")]
        public string NombreRazonSocial { get; set; }

        [Required(ErrorMessage = "El CUIT es obligatorio")]
        [StringLength(20)]
        public string Cuit { get; set; }

        [StringLength(255)]
        [Display(Name = "Domicilio")]
        public string? Domicilio { get; set; }

        [StringLength(200)]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [StringLength(50)]
        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [StringLength(50)]
        [Display(Name = "Celular")]
        public string? Celular { get; set; }

        [StringLength(50)]
        [Display(Name = "Fax")]
        public string? Fax { get; set; }

        [StringLength(100)]
        public string? Localidad { get; set; }

        [StringLength(50)]
        [Display(Name = "Tipo de Cliente")]
        public string? TipoCliente { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Saldo Cta. Cte.")]
        public decimal SaldoCuentaCorriente { get; set; } = 0;

        [Display(Name = "Fecha de Registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;
    }
}