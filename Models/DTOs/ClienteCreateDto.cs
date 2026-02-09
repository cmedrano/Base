namespace AvicolaApp.Models.DTOs
{
    public class ClienteCreateDto
    {
        public string NombreRazonSocial { get; set; } = string.Empty;
        public string Cuit { get; set; } = string.Empty;
        public string? Domicilio { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Celular { get; set; }
        public string? Fax { get; set; }
        public string? Localidad { get; set; }
        public string TipoCliente { get; set; } = "Minorista";
    }

    public class ClienteUpdateDto : ClienteCreateDto
    {
        public int Id { get; set; }
    }

    public class ClienteListDto
    {
        public int Id { get; set; }
        public string NombreRazonSocial { get; set; } = string.Empty;
        public string Cuit { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Localidad { get; set; }
        public string? Celular { get; set; }
        public string? Telefono { get; set; }
        public decimal SaldoCuentaCorriente { get; set; }
        public string TipoCliente { get; set; } = string.Empty;
    }
}