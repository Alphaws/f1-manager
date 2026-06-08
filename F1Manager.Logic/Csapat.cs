using System;
using System.Collections.Generic;

namespace F1Manager.Logic
{
    /// <summary>
    /// Egy F1-es csapatot reprezentáló osztály.
    /// </summary>
    public class Csapat
    {
        /// <summary>
        /// Egyedi azonosító a kapcsolatokhoz.
        /// </summary>
        public string Guid { get; set; } = System.Guid.NewGuid().ToString();
        
        /// <summary>
        /// A csapat neve.
        /// </summary>
        public string Nev { get; set; }
        
        /// <summary>
        /// A csapathoz tartozó versenyzők azonosítóinak listája.
        /// </summary>
        public List<string> VersenyzoGuids { get; set; } = new List<string>();

        /// <summary>
        /// Üres konstruktor a JSON-höz.
        /// </summary>
        public Csapat() 
        { 
        }

        /// <summary>
        /// Új csapat létrehozása névvel.
        /// </summary>
        public Csapat(string nev) 
        { 
            this.Nev = nev; 
        }

        public override string ToString() 
        {
            return $"{Nev} (Létszám: {VersenyzoGuids.Count})";
        }
    }
}
