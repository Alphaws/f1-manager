using System;
using System.Collections.Generic;

namespace F1Manager.Logic
{
    public class Csapat
    {
        public string Guid { get; set; } = System.Guid.NewGuid().ToString();
        public string Nev { get; set; }
        public List<string> VersenyzoGuids { get; set; } = new();

        public Csapat() { }
        public Csapat(string nev) { Nev = nev; }

        public override string ToString() => $"{Nev} (Létszám: {VersenyzoGuids.Count})";
    }
}
