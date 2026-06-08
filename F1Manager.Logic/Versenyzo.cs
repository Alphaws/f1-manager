using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace F1Manager.Logic
{
    /// <summary>
    /// Egy versenyző adatait tároló osztály.
    /// PDF Compliant: nev, orszag, pontszam adattagok.
    /// </summary>
    public class Versenyzo
    {
        // JSON azonosító (nem PDF követelmény, de a DB-hez kell)
        public string Guid { get; set; } = System.Guid.NewGuid().ToString();

        // 1. nev, orszag és pontszam adattagok
        private string nev;
        private string orszag;
        private int pontszam;

        public string Nev { get => nev; set => nev = value; }
        public string Orszag { get => orszag; set => orszag = value; }

        /// <summary>
        /// 7. Setter csak 0 és 500 között, egyébként F1Exception.
        /// </summary>
        public int Pontszam
        {
            get => pontszam;
            set
            {
                if (value < 0 || value > 500)
                {
                    throw new F1Exception("Érvénytelen pontszám!");
                }
                pontszam = value;
            }
        }

        public Versenyzo() { }

        /// <summary>
        /// 2. Konstruktor (nev, orszag, pontszam sorrendben).
        /// 3. pontszam alapértelmezett értéke 0.
        /// </summary>
        public Versenyzo(string nev, string orszag, int pontszam = 0)
        {
            this.nev = nev;
            this.orszag = orszag;
            this.Pontszam = pontszam; // A settert hívjuk a validáció miatt
        }

        /// <summary>
        /// 8. ToString felülírás: {nev} ({orszag}) – {pontszam} pont.
        /// </summary>
        public override string ToString()
        {
            return $"{nev} ({orszag}) – {pontszam} pont";
        }

        /// <summary>
        /// VIZSGA KÖVETELMÉNY: Equals és GetHashCode felülírása.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is Versenyzo masik)
            {
                return this.nev.ToLower() == masik.nev.ToLower() &&
                       this.orszag.ToLower() == masik.orszag.ToLower();
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(nev?.ToLower(), orszag?.ToLower());
        }
    }
}
