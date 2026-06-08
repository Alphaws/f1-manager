using System;
using System.Collections.Generic;
using System.Linq;

namespace F1Manager.Logic
{
    /// <summary>
    /// Egy teljes bajnoki szezont reprezentáló osztály.
    /// PDF Compliant: csapatok lista, hozzaad_csapat, legjobb_csapat, statisztika.
    /// </summary>
    public class Bajnoksag
    {
        public string Guid { get; set; } = System.Guid.NewGuid().ToString();
        public string Megnevezes { get; set; }
        public int Evszam { get; set; }

        // 1. csapatok nevű lista
        private List<Csapat> csapatok;

        public List<string> CsapatGuids { get; set; } = new List<string>();

        /// <summary>
        /// 2. A konstruktor inicializálja ezt a listát üresen!
        /// </summary>
        public Bajnoksag()
        {
            this.csapatok = new List<Csapat>();
        }

        public Bajnoksag(string nev, int ev)
        {
            this.Megnevezes = nev;
            this.Evszam = ev;
            this.csapatok = new List<Csapat>();
        }

        /// <summary>
        /// 3. hozzaad_csapat metódus.
        /// VIZSGA KÖVETELMÉNY: F1Exception: „Nem csapat!”.
        /// </summary>
        public void HozzaadCsapat(object obj)
        {
            if (obj is not Csapat cs)
            {
                throw new F1Exception("Nem csapat!");
            }
            csapatok.Add(cs);
            if (!CsapatGuids.Contains(cs.Guid)) CsapatGuids.Add(cs.Guid);
        }

        /// <summary>
        /// 4. legjobb_csapat metódus.
        /// Visszaadja azt a csapatot, amelynek a legnagyobb az összpontszáma.
        /// </summary>
        public Csapat? LegjobbCsapat()
        {
            if (!csapatok.Any()) return null;
            return csapatok.OrderByDescending(cs => cs.PontszamOsszesen()).First();
        }

        /// <summary>
        /// 5. statisztika metódus.
        /// Hány versenyző van országonként (kisbetűs országnevekkel).
        /// </summary>
        public Dictionary<string, int> Statisztika()
        {
            var stat = new Dictionary<string, int>();
            foreach (var cs in csapatok)
            {
                foreach (var v in cs.Versenyzok)
                {
                    string o = v.Orszag.ToLower();
                    if (stat.ContainsKey(o)) stat[o]++;
                    else stat[o] = 1;
                }
            }
            return stat;
        }

        public override string ToString()
        {
            return $"[{Evszam}] {Megnevezes}";
        }
    }
}
