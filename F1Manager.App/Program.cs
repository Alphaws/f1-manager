using System;
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
                Console.WriteLine("║        FORMULA-1 MANAGER v2.0            ║");
                Console.WriteLine("╚══════════════════════════════════════════╝");
                Console.ResetColor();
                Console.WriteLine("1. Versenyzők kezelése");
                Console.WriteLine("2. Csapatok kezelése");
                Console.WriteLine("3. Bajnokságok kezelése");
                Console.WriteLine("4. Adatok mentése");
                Console.WriteLine("0. Kilépés");
                Console.Write("\nVálasztás: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": DriverMenu(); break;
                    case "2": TeamMenu(); break;
                    case "3": ChampionshipMenu(); break;
                    case "4": storage.SaveAll(); Console.WriteLine("Mentve!"); Thread.Sleep(1000); break;
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
                Console.WriteLine("--- VERSENYZŐK KEZELÉSE ---");
                foreach (var v in storage.Versenyzok) Console.WriteLine($"- {v}");
                Console.WriteLine("\n[A] Új versenyző | [M] Módosítás | [T] Törlés | [V] Vissza");
                
                string cmd = Console.ReadLine()?.ToUpper();
                if (cmd == "V") back = true;
                if (cmd == "A")
                {
                    Console.Write("Név: "); string nev = Console.ReadLine();
                    Console.Write("Ország: "); string orszag = Console.ReadLine();
                    Console.Write("Kezdő pontszám: "); int pont = int.Parse(Console.ReadLine() ?? "0");
                    storage.Versenyzok.Add(new Versenyzo(nev, orszag, pont));
                }
                if (cmd == "T")
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
                Console.WriteLine("--- CSAPATOK KEZELÉSE ---");
                foreach (var cs in storage.Csapatok) Console.WriteLine($"- {cs}");
                Console.WriteLine("\n[A] Új csapat | [H] Versenyző hozzáadása | [V] Vissza");
                
                string cmd = Console.ReadLine()?.ToUpper();
                if (cmd == "V") back = true;
                if (cmd == "A")
                {
                    Console.Write("Csapat név: "); 
                    storage.Csapatok.Add(new Csapat(Console.ReadLine()));
                }
                if (cmd == "H")
                {
                    Console.Write("Melyik csapatba? "); string csNev = Console.ReadLine();
                    var cs = storage.Csapatok.FirstOrDefault(x => x.Nev == csNev);
                    if (cs != null)
                    {
                        Console.Write("Versenyző neve: "); string vNev = Console.ReadLine();
                        var v = storage.Versenyzok.FirstOrDefault(x => x.Nev == vNev);
                        if (v != null) cs.VersenyzoGuids.Add(v.Guid);
                    }
                }
            }
        }

        static void ChampionshipMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("--- BAJNOKSÁGOK ---");
                foreach (var b in storage.Bajnoksagok) Console.WriteLine($"- {b}");
                Console.WriteLine("\n[A] Új bajnokság | [V] Vissza");
                
                string cmd = Console.ReadLine()?.ToUpper();
                if (cmd == "V") back = true;
                if (cmd == "A")
                {
                    Console.Write("Megnevezés: "); string nev = Console.ReadLine();
                    Console.Write("Évszám: "); int ev = int.Parse(Console.ReadLine());
                    storage.Bajnoksagok.Add(new Bajnoksag(nev, ev));
                }
            }
        }
    }
}
