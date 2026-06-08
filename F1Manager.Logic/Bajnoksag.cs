using System;
using System.Collections.Generic;

namespace F1Manager.Logic
{
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
