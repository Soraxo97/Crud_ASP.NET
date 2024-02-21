using System;
using System.Collections.Generic;

namespace Prueba.Models;

public partial class Cliente
{
    public int Id { get; set; }

    public string? Rut { get; set; }

    public string? Nombre { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public virtual ICollection<CuentaInversion> CuentaInversions { get; set; } = new List<CuentaInversion>();
}
