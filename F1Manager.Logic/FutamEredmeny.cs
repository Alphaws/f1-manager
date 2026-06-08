using System;
using System.Collections.Generic;

namespace F1Manager.Logic
{
    public class FutamEredmeny
    {
        public string Guid { get; set; } = System.Guid.NewGuid().ToString();
        public string BajnoksagGuid { get; set; }
        public string FutamNeve { get; set; }
        public DateTime Datum { get; set; }
        public List<VersenyzoEredmeny> Helyezesek { get; set; } = new();

        public override string ToString() => $"{Datum:yyyy.MM.dd} - {FutamNeve}";
    }
}
