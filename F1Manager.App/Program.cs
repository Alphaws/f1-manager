using System;
using System.Linq;
using System.Threading;
using F1Manager.Logic;

namespace F1Manager.App
{
    /// <summary>
    /// Az alkalmazás fő belépési pontja és a felhasználói felület kezelője.
    /// </summary>
    class Program
    {
        // Az adatkezelő példányosítása
        static StorageManager storage = new StorageManager();

        static void Main(string[] args)
        {
            // Adatok betöltése indításkor
            storage.LoadAll();
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                DrawHeader("FORMULA-1 MANAGER v2.2");
                
                Console.WriteLine("1. Versenyzők kezelése (CRUD)");
                Console.WriteLine("2. Csapatok kezelése (CRUD + Összeállítás)");
                Console.WriteLine("3. Bajnokságok kezelése");
                Console.WriteLine("4. Futam eredmények rögzítése");
                Console.WriteLine("5. Adatok mentése");
                Console.WriteLine("0. Kilépés");
                
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nVálasztás: ");
                Console.ResetColor();

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": 
                        DriverMenu(); 
                        break;
                    case "2": 
                        TeamMenu(); 
                        break;
                    case "3": 
                        ChampionshipMenu(); 
                        break;
                    case "4": 
                        ResultMenu(); 
                        break;
                    case "5": 
                        storage.SaveAll(); 
                        Console.WriteLine("\nSikeres mentés!"); 
                        Thread.Sleep(1000); 
                        break;
                    case "0": 
                        exit = true; 
                        storage.SaveAll(); // Kilépés előtt automatikus mentés
                        break;
                }
            }
        }

        #region Menükezelő Metódusok

        /// <summary>
        /// Versenyzők listázása és szerkesztése.
        /// </summary>
        static void DriverMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                DrawHeader("VERSENYZŐK LISTÁJA");
                
                if (!storage.Versenyzok.Any()) 
                {
                    Console.WriteLine("(Még nincsenek rögzített versenyzők)");
                }
                
                foreach (var v in storage.Versenyzok) 
                {
                    Console.WriteLine($"- {v}");
                }
                
                Console.WriteLine("\n[A] Új | [M] Módosítás | [T] Törlés | [V] Vissza");
                Console.Write("\nParancs: ");
                
                string cmd = Console.ReadLine()?.ToUpper();
                
                if (cmd == "V") back = true;
                else if (cmd == "A")
                {
                    AddDriver();
                }
                else if (cmd == "M")
                {
                    UpdateDriver();
                }
                else if (cmd == "T")
                {
                    DeleteDriver();
                }
            }
        }

        /// <summary>
        /// Csapatok kezelése és versenyzők hozzárendelése.
        /// </summary>
        static void TeamMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                DrawHeader("CSAPATOK ÉS VERSENYZŐK");
                
                foreach (var cs in storage.Csapatok) 
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n[ {cs.Nev.ToUpper()} ]");
                    Console.ResetColor();
                    
                    var tagok = storage.Versenyzok.Where(v => cs.VersenyzoGuids.Contains(v.Guid));
                    
                    if (!tagok.Any()) 
                    {
                        Console.WriteLine("  (Nincsenek versenyzők a csapatban)");
                    }
                    
                    foreach (var t in tagok) 
                    {
                        Console.WriteLine($"  - {t.Nev} ({t.Orszag})");
                    }
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

        static void ChampionshipMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                DrawHeader("BAJNOKSÁGOK");
                
                foreach (var b in storage.Bajnoksagok) 
                {
                    Console.WriteLine($"- {b} (Benevezett csapatok: {b.CsapatGuids.Count})");
                }
                
                Console.WriteLine("\n[A] Új szezon | [H] Csapat nevezése | [V] Vissza");
                Console.Write("\nParancs: ");
                
                string cmd = Console.ReadLine()?.ToUpper();
                if (cmd == "V") back = true;
                else if (cmd == "A") AddChampionship();
                // További funkciók ide építhetők...
            }
        }

        static void ResultMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                DrawHeader("FUTAM EREDMÉNYEK");
                
                foreach (var er in storage.Eredmenyek) 
                {
                    Console.WriteLine($"- {er}");
                }
                
                Console.WriteLine("\n[A] Új eredmény rögzítése | [V] Vissza");
                Console.Write("\nParancs: ");
                
                string cmd = Console.ReadLine()?.ToUpper();
                if (cmd == "V") back = true;
                else if (cmd == "A") AddResult();
            }
        }

        #endregion

        #region Segédmetódusok (Logika és UI)

        static void DrawHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔" + new string('═', 42) + "╗");
            Console.WriteLine("║ " + title.PadRight(41) + "║");
            Console.WriteLine("╚" + new string('═', 42) + "╝");
            Console.ResetColor();
        }

        static void AddDriver()
        {
            try
            {
                Console.Write("Név: "); string nev = Console.ReadLine();
                Console.Write("Ország: "); string orszag = Console.ReadLine();
                Console.Write("Kezdő pontszám: "); 
                int pont = int.Parse(Console.ReadLine() ?? "0");
                
                storage.Versenyzok.Add(new Versenyzo(nev, orszag, pont));
                Console.WriteLine("\nSikeres felvétel!");
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nHiba: {ex.Message}");
                Thread.Sleep(2000);
            }
        }

        static void UpdateDriver()
        {
            Console.Write("Kit módosítsunk? (Név): "); 
            string nev = Console.ReadLine();
            var v = storage.Versenyzok.FirstOrDefault(x => x.Nev == nev);
            
            if (v != null) 
            {
                Console.Write($"Új név [{v.Nev}]: "); 
                string uNev = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(uNev)) v.Nev = uNev;
                
                Console.Write($"Új ország [{v.Orszag}]: "); 
                string uOrszag = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(uOrszag)) v.Orszag = uOrszag;
                
                Console.WriteLine("\nAdatok frissítve!");
            }
            else
            {
                Console.WriteLine("\nNem található ilyen versenyző.");
            }
            Thread.Sleep(1000);
        }

        static void DeleteDriver()
        {
            Console.Write("Törlendő név: "); 
            string nev = Console.ReadLine();
            int deleted = storage.Versenyzok.RemoveAll(x => x.Nev == nev);
            
            if (deleted > 0) Console.WriteLine("\nSikeres törlés!");
            else Console.WriteLine("\nNincs találat.");
            Thread.Sleep(500);
        }

        static void AddTeam()
        {
            Console.Write("Csapat neve: "); 
            string nev = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nev))
            {
                storage.Csapatok.Add(new Csapat(nev));
            }
        }

        static void AssignDriverToTeam()
        {
            Console.Write("Csapat neve: "); string csNev = Console.ReadLine();
            var cs = storage.Csapatok.FirstOrDefault(x => x.Nev == csNev);
            
            if (cs != null) 
            {
                Console.Write("Versenyző neve: "); string vNev = Console.ReadLine();
                var v = storage.Versenyzok.FirstOrDefault(x => x.Nev == vNev);
                
                if (v != null) 
                {
                    if (!cs.VersenyzoGuids.Contains(v.Guid))
                    {
                        cs.VersenyzoGuids.Add(v.Guid);
                        Console.WriteLine("\nSikeres hozzáadás!");
                    }
                    else
                    {
                        Console.WriteLine("\nA versenyző már a csapat tagja.");
                    }
                }
            }
            Thread.Sleep(1000);
        }

        static void DeleteTeam()
        {
            Console.Write("Törlendő csapat neve: "); 
            string nev = Console.ReadLine();
            storage.Csapatok.RemoveAll(x => x.Nev == nev);
        }

        static void AddChampionship()
        {
            Console.Write("Megnevezés: "); string nev = Console.ReadLine();
            Console.Write("Évszám: "); 
            if (int.TryParse(Console.ReadLine(), out int ev))
            {
                storage.Bajnoksagok.Add(new Bajnoksag(nev, ev));
            }
        }

        static void AddResult()
        {
            Console.Write("Futam neve: "); string nev = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nev))
            {
                storage.Eredmenyek.Add(new FutamEredmeny 
                { 
                    FutamNeve = nev, 
                    Datum = DateTime.Now 
                });
                Console.WriteLine("\nFutam rögzítve!");
            }
            Thread.Sleep(500);
        }

        #endregion
    }
}
