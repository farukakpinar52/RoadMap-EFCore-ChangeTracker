using System;
using System.Collections.Generic;

namespace EntityStates.DAL.Entities;

public partial class Parca
{
    public int Id { get; set; }

    public string ParcaAdi { get; set; } = null!;

    public int? UrunId { get; set; }

    public virtual Urun? Urun { get; set; }

    public virtual ICollection<Urun> Uruns { get; set; } = new List<Urun>();
}
