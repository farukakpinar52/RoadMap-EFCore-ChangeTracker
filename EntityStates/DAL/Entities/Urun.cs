using System;
using System.Collections.Generic;

namespace EntityStates.DAL.Entities;

public partial class Urun
{
    public int Id { get; set; }

    public string UrunAdi { get; set; } = null!;

    public float Fiyat { get; set; }

    public virtual ICollection<Parca> Parcas { get; set; } = new List<Parca>();

    public virtual ICollection<Parca> ParcasNavigation { get; set; } = new List<Parca>();
}
