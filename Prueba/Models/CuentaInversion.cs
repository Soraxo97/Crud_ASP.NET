using System;
using System.Collections.Generic;

namespace Prueba.Models
{
    public partial class CuentaInversion
    {
        public int Id { get; set; }
        public int? IdCliente { get; set; }
        public string? NombreCuenta { get; set; }
        public virtual Cliente? IdClienteNavigation { get; set; }
    }
}
