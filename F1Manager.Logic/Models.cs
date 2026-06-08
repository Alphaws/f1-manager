using System;

namespace F1Manager.Logic
{
    public class F1Exception : Exception
    {
        public F1Exception() { }
        public F1Exception(string message) : base(message) { }
    }

    public class Versenyzo
    {
        public string Guid { get; set; } = System.Guid.NewGuid().ToString();
        public string Nev { get; set; }
        public string Orszag { get; set; }
        private int pontszam;

        public int Pontszam
        {
            get => pontszam;
            set => pontszam = (value < 0 || value > 1000) ? throw new F1Exception("Érvénytelen pontszám!") : value;
        }

        public Versenyzo() { }
        public Versenyzo(string nev, string orszag, int pont = 0) 
        { 
            Nev = nev; 
            Orszag = orszag; 
            Pontszam = pont; 
        }

        public override string ToString() => $"{Nev} ({Orszag}) - {Pontszam} pont";
        
        public override bool Equals(object? obj) => obj is Versenyzo v && v.Nev == Nev && v.Orszag == Orszag;
        public override int GetHashCode() => HashCode.Combine(Nev, Orszag);
    }

    public class Csapat
    {
        public string Guid { get; set; } = System.Guid.NewGuid().ToString();
        public string Nev { get; set; }
        public List<string> VersenyzoGuids { get; set; } = new();

        public Csapat() { }
        public Csapat(string nev) { Nev = nev; }

        public override string ToString() => $"{Nev} (Létszám: {VersenyzoGuids.Count})";
    }

    public class Bajnoksag
    {
        public string Guid { get; set; } = System.Guid.NewGuid().ToString();
        public string Megnevezes { get; set; }
        public int Evszam { get; set; }
        public List<string> CsapatGuids { get; set; } = new();

        public Bajnoksag() { }
        public Bajnoksag(string nev, int ev) { Megnevezes = nev; Evszam = ev; }

        public override string ToString() => $"[{Evszam}] {Megnevezes}";
    }
}
