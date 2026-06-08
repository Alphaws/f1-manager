using System;
using System.Linq;
using System.Threading;
using F1Manager.Logic;

namespace F1Manager.App
{
    /// <summary>
    /// A felhasználói felület menüit és interakcióit kezelő osztály.
    /// SOLID - SRP: Csak a UI megjelenítésért és a felhasználói bemenetért felel.
    /// </summary>
    public class MenuHandler
    {
        private readonly IStorageService _storage;

        public MenuHandler(IStorageService storage)
        {
            _storage = storage;
        }

        public void DrawHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔" + new string('═', 42) + "╗");
            Console.WriteLine("║ " + title.PadRight(41) + "║");
            Console.WriteLine("╚" + new string('═', 42) + "╝");
            Console.ResetColor();
        }

        public void DriverMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                DrawHeader("VERSENYZŐK LISTÁJA");
                
                if (!_storage.Versenyzok.Any()) Console.WriteLine("(Még nincsenek rögzített versenyzők)");
                foreach (var v in _storage.Versenyzok) Console.WriteLine($"- {v}");
                
                Console.WriteLine("\n[A] Új | [M] Módosítás | [T] Törlés | [V] Vissza");
                Console.Write("\nParancs: ");
                
                string cmd = Console.ReadLine()?.ToUpper();
                if (cmd == "V") back = true;
                else if (cmd == "A") AddDriver();
                else if (cmd == "M") UpdateDriver();
                else if (cmd == "T") DeleteDriver();
            }
        }

        public void TeamMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                DrawHeader("CSAPATOK ÉS VERSENYZŐK");
                
                foreach (var cs in _storage.Csapatok) 
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n[ {cs.Nev.ToUpper()} ]");
                    Console.ResetColor();
                    
                    var tagok = _storage.Versenyzok.Where(v => cs.VersenyzoGuids.Contains(v.Guid));
                    if (!tagok.Any()) Console.WriteLine("  (Nincsenek versenyzők a csapatban)");
                    foreach (var t in tagok) Console.WriteLine($"  - {t.Nev} ({t.Orszag})");
                }
                
                Console.WriteLine("\n[A] Új csapat | [H] Versenyző hozzáadása | [T] Törlés | [V] Vissza");
                Console.Write("\nParancs: ");
                
                string cmd = Console.ReadLine()?.ToUpper();
                if (cmd == "V") back = true;
                else if (cmd == "A") AddTeam();
                else if (cmd == "H") AssignDriverToTeam();
                else if (cmd == "T") DeleteTeam();
            }
        }

        public void ChampionshipMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                DrawHeader("BAJNOKSÁGOK");
                foreach (var b in _storage.Bajnoksagok) Console.WriteLine($"- {b} (Benevezett csapatok: {b.CsapatGuids.Count})");
                
                Console.WriteLine("\n[A] Új szezon | [V] Vissza");
                Console.Write("\nParancs: ");
                
                string cmd = Console.ReadLine()?.ToUpper();
                if (cmd == "V") back = true;
                else if (cmd == "A") AddChampionship();
            }
        }

        public void ResultMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                DrawHeader("FUTAM EREDMÉNYEK");
                foreach (var er in _storage.Eredmenyek) Console.WriteLine($"- {er}");
                
                Console.WriteLine("\n[A] Új eredmény rögzítése | [V] Vissza");
                Console.Write("\nParancs: ");
                
                string cmd = Console.ReadLine()?.ToUpper();
                if (cmd == "V") back = true;
                else if (cmd == "A") AddResult();
            }
        }

        #region Private Helpers

        private void AddDriver()
        {
            try
            {
                Console.Write("Név: "); string nev = Console.ReadLine();
                Console.Write("Ország: "); string orszag = Console.ReadLine();
                Console.Write("Kezdő pontszám: "); 
                int pont = int.Parse(Console.ReadLine() ?? "0");
                
                _storage.Versenyzok.Add(new Versenyzo(nev, orszag, pont));
                Console.WriteLine("\nSikeres felvétel!");
                Thread.Sleep(500);
            }
            catch (Exception ex) { Console.WriteLine($"\nHiba: {ex.Message}"); Thread.Sleep(2000); }
        }

        private void UpdateDriver()
        {
            Console.Write("Kit módosítsunk? (Név): "); string nev = Console.ReadLine();
            var v = _storage.Versenyzok.FirstOrDefault(x => x.Nev == nev);
            if (v != null) 
            {
                Console.Write($"Új név [{v.Nev}]: "); string uNev = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(uNev)) v.Nev = uNev;
                Console.Write($"Új ország [{v.Orszag}]: "); string uOrszag = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(uOrszag)) v.Orszag = uOrszag;
                Console.WriteLine("\nAdatok frissítve!");
            }
            Thread.Sleep(1000);
        }

        private void DeleteDriver()
        {
            Console.Write("Törlendő név: "); string nev = Console.ReadLine();
            _storage.Versenyzok.RemoveAll(x => x.Nev == nev);
        }

        private void AddTeam()
        {
            Console.Write("Csapat neve: "); string nev = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nev)) _storage.Csapatok.Add(new Csapat(nev));
        }

        private void AssignDriverToTeam()
        {
            Console.Write("Csapat neve: "); string csNev = Console.ReadLine();
            var cs = _storage.Csapatok.FirstOrDefault(x => x.Nev == csNev);
            if (cs != null) 
            {
                Console.Write("Versenyző neve: "); string vNev = Console.ReadLine();
                var v = _storage.Versenyzok.FirstOrDefault(x => x.Nev == vNev);
                if (v != null) cs.Hozzaad(v);
            }
            Thread.Sleep(1000);
        }

        private void DeleteTeam()
        {
            Console.Write("Törlendő csapat neve: "); string nev = Console.ReadLine();
            _storage.Csapatok.RemoveAll(x => x.Nev == nev);
        }

        private void AddChampionship()
        {
            Console.Write("Megnevezés: "); string nev = Console.ReadLine();
            Console.Write("Évszám: "); 
            if (int.TryParse(Console.ReadLine(), out int ev)) _storage.Bajnoksagok.Add(new Bajnoksag(nev, ev));
        }

        private void AddResult()
        {
            Console.Write("Futam neve: "); string nev = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nev)) _storage.Eredmenyek.Add(new FutamEredmeny { FutamNeve = nev, Datum = DateTime.Now });
        }

        #endregion
    }
}
