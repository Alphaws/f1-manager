using System;

namespace F1Manager.Logic
{
    /// <summary>
    /// Egy versenyző adatait tároló osztály.
    /// </summary>
    public class Versenyzo
    {
        // Publikus tulajdonságok
        public string Guid { get; set; } = System.Guid.NewGuid().ToString();
        public string Nev { get; set; }
        public string Orszag { get; set; }
        
        // Privát adattag a validált pontszámhoz
        private int pontszam;

        /// <summary>
        /// A versenyző összesített pontszáma (0-1000 között).
        /// </summary>
        public int Pontszam
        {
            get 
            { 
                return pontszam; 
            }
            set 
            {
                if (value < 0 || value > 1000)
                {
                    throw new F1Exception("Érvénytelen pontszám! (0 és 1000 között kell lennie)");
                }
                pontszam = value;
            }
        }

        /// <summary>
        /// Üres konstruktor a JSON deszerializációhoz.
        /// </summary>
        public Versenyzo() 
        { 
        }

        /// <summary>
        /// Paraméteres konstruktor új versenyző létrehozásához.
        /// </summary>
        public Versenyzo(string nev, string orszag, int pont = 0) 
        { 
            this.Nev = nev; 
            this.Orszag = orszag; 
            this.Pontszam = pont; 
        }

        /// <summary>
        /// Szöveges megjelenítés a listákhoz.
        /// </summary>
        public override string ToString() 
        {
            return $"{Nev} ({Orszag}) - {Pontszam} pont";
        }
        
        /// <summary>
        /// Egyenlőség vizsgálata név és ország alapján.
        /// </summary>
        public override bool Equals(object? obj) 
        {
            if (obj is Versenyzo v)
            {
                return v.Nev == this.Nev && v.Orszag == this.Orszag;
            }
            return false;
        }

        /// <summary>
        /// Hash kód generálása az Equals-hoz.
        /// </summary>
        public override int GetHashCode() 
        {
            return HashCode.Combine(Nev, Orszag);
        }
    }
}
