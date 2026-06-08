using System;
using System.Linq;
using F1Manager.Logic;

namespace F1Manager.App
{
    class Program
    {
        static StorageManager storage = new();

        static void Main(string[] args)
        {
            storage.LoadAll();
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("╔══════════════════════════════════════════╗");
                Console.WriteLine("║        FORMULA-1 MANAGER v2.1            ║");
                Console.WriteLine("╚══════════════════════════════════════════╝");
                Console.ResetColor();
                Console.WriteLine("1. Versenyzők kezelése (CRUD)");
                Console.WriteLine("2. Csapatok kezelése (CRUD + Összeállítás)");
                Console.WriteLine("3. Bajnokságok kezelése");
                Console.WriteLine("4. Futam eredmények rögzítése");
                Console.WriteLine("5. Adatok mentése");
                Console.WriteLine("0. Kilépés");
                Console.Write("\nVálasztás: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": DriverMenu(); break;
                    case "2": TeamMenu(); break;
                    case "3": ChampionshipMenu(); break;
                    case "4": ResultMenu(); break;
                    case "5": storage.SaveAll(); Console.WriteLine("Mentve!"); Thread.Sleep(1000); break;
                    case "0": exit = true; storage.SaveAll(); break;
                }
            }
        }

        static void DriverMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== VERSENYZŐK LISTÁJA ===");
                if (!storage.Versenyzok.Any()) Console.WriteLine("(Üres)");
                foreach (var v in storage.Versenyzok) Console.WriteLine($"- {v}");
                
                Console.WriteLine("\n[A] Új | [M] Módosítás | [T] Törlés | [V] Vissza");
                string cmd = Console.ReadLine()?.ToUpper();
                if (cmd == "V") back = true;
                else if (cmd == "A")
                {
                    Console.Write("Név: "); string nev = Console.ReadLine();
                    Console.Write("Ország: "); string orszag = Console.ReadLine();
                    Console.Write("Pont: "); int pont = int.Parse(Console.ReadLine() ?? "0");
                    storage.Versenyzok.Add(new Versenyzo(nev, orszag, pont));
                }
                else if (cmd == "M")
                {
                    Console.Write("Melyik nevet módosítsuk? "); string nev = Console.ReadLine();
                    var v = storage.Versenyzok.FirstOrDefault(x => x.Nev == nev);
                    if (v != null) {
                        Console.Write($"Új név ({v.Nev}): "); v.Nev = Console.ReadLine();
                        Console.Write($"Új ország ({v.Orszag}): "); v.Orszag = Console.ReadLine();
                    }
                }
                else if (cmd == "T")
                {
                    Console.Write("Törlendő név: "); string nev = Console.ReadLine();
                    storage.Versenyzok.RemoveAll(x => x.Nev == nev);
                }
            }
        }

        static void TeamMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== CSAPATOK ÉS VERSENYZŐK ===");
                foreach (var cs in storage.Csapatok) 
                {
                    Console.WriteLine($"\n[ {cs.Nev.ToUpper()} ]");
                    var tagok = storage.Versenyzok.Where(v => cs.VersenyzoGuids.Contains(v.Guid));
                    if (!tagok.Any()) Console.WriteLine("  (Nincsenek versenyzők)");
                    foreach (var t in tagok) Console.WriteLine($"  - {t.Nev} ({t.Orszag})");
                }
                
                Console.WriteLine("\n[A] Új csapat | [H] Versenyző hozzáadása | [T] Csapat törlése | [V] Vissza");
                string cmd = Console.ReadLine()?.ToUpper();
                if (cmd == "V") back = true;
                else if (cmd == "A")
                {
                    Console.Write("Csapat neve: "); storage.Csapatok.Add(new Csapat(Console.ReadLine()));
                }
                else if (cmd == "H")
                {
                    Console.Write("Hova? (Csapat név): "); string csNev = Console.ReadLine();
                    var cs = storage.Csapatok.FirstOrDefault(x => x.Nev == csNev);
                    if (cs != null) {
                        Console.Write("Kit? (Versenyző név): "); string vNev = Console.ReadLine();
                        var v = storage.Versenyzok.FirstOrDefault(x => x.Nev == vNev);
                        if (v != null && !cs.VersenyzoGuids.Contains(v.Guid)) cs.VersenyzoGuids.Add(v.Guid);
                    }
                }
                else if (cmd == "T")
                {
                    Console.Write("Törlendő csapat: "); string nev = Console.ReadLine();
                    storage.Csapatok.RemoveAll(x => x.Nev == nev);
                }
            }
        }

        static void ChampionshipMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== BAJNOKSÁGOK ===");
                foreach (var b in storage.Bajnoksagok) {
                    Console.WriteLine($"- {b} (Csapatok száma: {b.CsapatGuids.Count})");
                }
                
                Console.WriteLine("\n[A] Új | [H] Csapat nevezése | [V] Vissza");
                string cmd = Console.ReadLine()?.ToUpper();
                if (cmd == "V") back = true;
                else if (cmd == "A")
                {
                    Console.Write("Név: "); string nev = Console.ReadLine();
                    Console.Write("Év: "); int ev = int.Parse(Console.ReadLine() ?? "2026");
                    storage.Bajnoksagok.Add(new Bajnoksag(nev, ev));
                }
                else if (cmd == "H")
                {
                    Console.Write("Melyik bajnokságba? "); string bNev = Console.ReadLine();
                    var b = storage.Bajnoksagok.FirstOrDefault(x => x.Megnevezes == bNev);
                    if (b != null) {
                        Console.Write("Melyik csapatot? "); string csNev = Console.ReadLine();
                        var cs = storage.Csapatok.FirstOrDefault(x => x.Nev == csNev);
                        if (cs != null && !b.CsapatGuids.Contains(cs.Guid)) b.CsapatGuids.Add(cs.Guid);
                    }
                }
            }
        }

        static void ResultMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("=== FUTAM EREDMÉNYEK ===");
                foreach (var er in storage.Eredmenyek) Console.WriteLine($"- {er}");
                
                Console.WriteLine("\n[A] Új eredmény | [R] Részletek megtekintése | [V] Vissza");
                string cmd = Console.ReadLine()?.ToUpper();
                if (cmd == "V") back = true;
                else if (cmd == "A")
                {
                    Console.Write("Futam neve: "); string nev = Console.ReadLine();
                    storage.Eredmenyek.Add(new FutamEredmeny { FutamNeve = nev, Datum = DateTime.Now });
                    Console.WriteLine("Futam rögzítve!"); Thread.Sleep(500);
                }
            }
        }
    }
}
