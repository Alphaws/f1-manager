using System;
using F1Manager.Logic;

namespace F1Manager.App
{
    /// <summary>
    /// Az alkalmazás fő belépési pontja.
    /// SOLID - SRP: Csak az alkalmazás indításáért és a főmenü vezérléséért felel.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Függőségek inicializálása
            IStorageService storage = new StorageManager();
            MenuHandler ui = new MenuHandler(storage);

            storage.LoadAll();
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                ui.DrawHeader("FORMULA-1 MANAGER v2.3");
                
                Console.WriteLine("1. Versenyzők kezelése");
                Console.WriteLine("2. Csapatok kezelése");
                Console.WriteLine("3. Bajnokságok kezelése");
                Console.WriteLine("4. Futam eredmények");
                Console.WriteLine("5. Adatok mentése");
                Console.WriteLine("0. Kilépés");
                
                Console.Write("\nVálasztás: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ui.DriverMenu(); break;
                    case "2": ui.TeamMenu(); break;
                    case "3": ui.ChampionshipMenu(); break;
                    case "4": ui.ResultMenu(); break;
                    case "5": storage.SaveAll(); break;
                    case "0": exit = true; storage.SaveAll(); break;
                }
            }
        }
    }
}
