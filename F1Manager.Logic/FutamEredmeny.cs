using System;
using System.Collections.Generic;

namespace F1Manager.Logic
{
    /// <summary>
    /// Egy konkrét futam végeredményét tároló osztály.
    /// </summary>
    public class FutamEredmeny
    {
        public string Guid { get; set; } = System.Guid.NewGuid().ToString();
        
        /// <summary>
        /// Melyik bajnoksághoz tartozik a futam.
        /// </summary>
        public string BajnoksagGuid { get; set; }
        
        /// <summary>
        /// A futam helyszíne vagy neve (pl. Hungarian Grand Prix).
        /// </summary>
        public string FutamNeve { get; set; }
        
        /// <summary>
        /// A futam megrendezésének időpontja.
        /// </summary>
        public DateTime Datum { get; set; }
        
        /// <summary>
        /// A versenyzők helyezései és pontjai ezen a futamon.
        /// </summary>
        public List<VersenyzoEredmeny> Helyezesek { get; set; } = new List<VersenyzoEredmeny>();

        public override string ToString() 
        {
            return $"{Datum:yyyy.MM.dd} - {FutamNeve}";
        }
    }
}
