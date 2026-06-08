using System;

namespace F1Manager.Logic
{
    /// <summary>
    /// A Formula-1 alkalmazás egyedi kivételosztálya.
    /// Segít megkülönböztetni a saját üzleti hibaüzeneteket a rendszerhibáktól.
    /// </summary>
    public class F1Exception : Exception
    {
        /// <summary>
        /// Alapértelmezett konstruktor.
        /// </summary>
        public F1Exception() 
        { 
        }

        /// <summary>
        /// Konstruktor egyedi hibaüzenettel.
        /// </summary>
        /// <param name="message">A hiba leírása.</param>
        public F1Exception(string message) : base(message) 
        { 
        }
    }
}
