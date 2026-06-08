using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace F1Manager.Logic
{
    /// <summary>
    /// Egy F1-es csapatot reprezentáló osztály.
    /// PDF Compliant: neve és versenyzok lista.
    /// </summary>
    public class Csapat
    {
        public string Guid { get; set; } = System.Guid.NewGuid().ToString();

        // 1. neve és versenyzok lista
        public string Nev { get; set; }
        private List<Versenyzo> versenyzok;

        // JSON segítség (hogy ne legyen körkörös hivatkozás a fájlban)
        [JsonIgnore]
        public List<Versenyzo> Versenyzok => versenyzok;

        // DB-hez szükséges GUID lista
        public List<string> VersenyzoGuids { get; set; } = new List<string>();

        public Csapat() 
        { 
            this.versenyzok = new List<Versenyzo>(); 
        }

        /// <summary>
        /// 2. Konstruktor (nev paraméter, lista inicializálása).
        /// </summary>
        public Csapat(string nev)
        {
            this.Nev = nev;
            this.versenyzok = new List<Versenyzo>();
        }

        /// <summary>
        /// 3. hozzaad metódus.
        /// VIZSGA KÖVETELMÉNY: Saját F1Exception: „Nem versenyző!”.
        /// </summary>
        public void Hozzaad(object obj)
        {
            if (obj is not Versenyzo v)
            {
                throw new F1Exception("Nem versenyző!");
            }
            versenyzok.Add(v);
            if (!VersenyzoGuids.Contains(v.Guid)) VersenyzoGuids.Add(v.Guid);
        }

        /// <summary>
        /// 4. pontszam_osszesen metódus.
        /// </summary>
        public int PontszamOsszesen()
        {
            return versenyzok.Sum(v => v.Pontszam);
        }

        public override string ToString()
        {
            return $"{Nev} (Összpont: {PontszamOsszesen()})";
        }
    }
}
