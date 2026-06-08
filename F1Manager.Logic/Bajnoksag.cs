using System;
using System.Collections.Generic;

namespace F1Manager.Logic
{
    /// <summary>
    /// Egy teljes bajnoki szezont reprezentáló osztály.
    /// </summary>
    public class Bajnoksag
    {
        public string Guid { get; set; } = System.Guid.NewGuid().ToString();
        
        /// <summary>
        /// A bajnokság megnevezése (pl. Formula 1 World Championship).
        /// </summary>
        public string Megnevezes { get; set; }
        
        /// <summary>
        /// A szezon évszáma.
        /// </summary>
        public int Evszam { get; set; }
        
        /// <summary>
        /// A bajnokságba benevezett csapatok azonosítói.
        /// </summary>
        public List<string> CsapatGuids { get; set; } = new List<string>();

        public Bajnoksag() 
        { 
        }

        public Bajnoksag(string nev, int ev) 
        { 
            this.Megnevezes = nev; 
            this.Evszam = ev; 
        }

        public override string ToString() 
        {
            return $"[{Evszam}] {Megnevezes}";
        }
    }
}
