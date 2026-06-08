using System;

namespace F1Manager.Logic
{
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
}
