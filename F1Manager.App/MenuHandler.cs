using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using F1Manager.Logic;

namespace F1Manager.App
{
    /// <summary>
    /// A felhasználói felület menüit és interakcióit kezelő osztály.
    /// Mostantól professzionális, keretezett táblázatokat is tud rajzolni.
    /// </summary>
    public class MenuHandler
    {
        private readonly IStorageService _storage;

        public MenuHandler(IStorageService storage)
        {
            _storage = storage;
        }

        #region Táblázat Rajzoló Motor

        /// <summary>
        /// Kirajzol egy professzionális, keretezett táblázatot a megadott adatokkal.
        /// </summary>
        public void DrawTable(string[] headers, List<string[]> rows)
        {
            if (headers == null || headers.Length == 0) return;

            // 1. Oszlopszélességek meghatározása
            int[] columnWidths = new int[headers.Length];
            for (int i = 0; i < headers.Length; i++)
            {
                columnWidths[i] = headers[i].Length;
                foreach (var row in rows)
                {
                    if (row[i].Length > columnWidths[i])
                        columnWidths[i] = row[i].Length;
                }
                columnWidths[i] += 2; // Padding
            }

            // 2. Felső keret
            Console.Write("╔");
            for (int i = 0; i < columnWidths.Length; i++)
            {
                Console.Write(new string('═', columnWidths[i]));
                if (i < columnWidths.Length - 1) Console.Write("╦");
            }
            Console.WriteLine("╗");

            // 3. Fejléc
            Console.Write("║");
            for (int i = 0; i < headers.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($" {headers[i].PadRight(columnWidths[i] - 1)}");
                Console.ResetColor();
                Console.Write("║");
            }
            Console.WriteLine();

            // 4. Fejléc alatti elválasztó
            Console.Write("╠");
            for (int i = 0; i < columnWidths.Length; i++)
            {
                Console.Write(new string('═', columnWidths[i]));
                if (i < columnWidths.Length - 1) Console.Write("╬");
            }
            Console.WriteLine("╣");

            // 5. Adat sorok
            if (rows.Count == 0)
            {
                Console.Write("║");
                string emptyMsg = "(Nincs adat)";
                Console.Write($" {emptyMsg.PadRight(columnWidths.Sum() + columnWidths.Length - 2)} ");
                Console.WriteLine("║");
            }
            else
            {
                foreach (var row in rows)
                {
                    Console.Write("║");
                    for (int i = 0; i < row.Length; i++)
                    {
                        Console.Write($" {row[i].PadRight(columnWidths[i] - 1)}║");
                    }
                    Console.WriteLine();
                }
            }

            // 6. Alsó keret
            Console.Write("╚");
            for (int i = 0; i < columnWidths.Length; i++)
            {
                Console.Write(new string('═', columnWidths[i]));
                if (i < columnWidths.Length - 1) Console.Write("╩");
            }
            Console.WriteLine("╝");
        }

        #endregion

        #region Menükezelő Metódusok

        public void DrawHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔" + new string('═', 44) + "╗");
            Console.WriteLine("║ " + title.PadRight(43) + "║");
            Console.WriteLine("╚" + new string('═', 44) + "╝");
            Console.ResetColor();
        }

        public void DriverMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                DrawHeader("VERSENYZŐK LISTÁJA");
                
                var headers = new[] { "Név", "Ország", "Pontszám" };
                var rows = _storage.Versenyzok.Select(v => new[] { v.Nev, v.Orszag, v.Pontszam.ToString() }).ToList();
                DrawTable(headers, rows);
                
                Console.WriteLine("\n[A] Új | [M] Módosítás | [T] Törlés | [V] Vissza");
                
                // Azonnali válasz billentyűleütésre
                char cmd = char.ToUpper(Console.ReadKey(true).KeyChar);
                if (cmd == 'V') back = true;
                else if (cmd == 'A') AddDriver();
                else if (cmd == 'M') UpdateDriver();
                else if (cmd == 'T') DeleteDriver();
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
                    Console.WriteLine($"\n>> {cs.Nev.ToUpper()}");
                    Console.ResetColor();
                    
                    var headers = new[] { "Csapattagok", "Ország" };
                    var tagok = _storage.Versenyzok
                        .Where(v => cs.VersenyzoGuids.Contains(v.Guid))
                        .Select(t => new[] { t.Nev, t.Orszag })
                        .ToList();
                    
                    DrawTable(headers, tagok);
                }
                
                Console.WriteLine("\n[A] Új csapat | [H] Versenyző hozzáadása | [T] Törlés | [V] Vissza");
                
                char cmd = char.ToUpper(Console.ReadKey(true).KeyChar);
                if (cmd == 'V') back = true;
                else if (cmd == 'A') AddTeam();
                else if (cmd == 'H') AssignDriverToTeam();
                else if (cmd == 'T') DeleteTeam();
            }
        }

        public void ChampionshipMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                DrawHeader("BAJNOKSÁGOK");
                
                var headers = new[] { "Év", "Bajnokság Megnevezése", "Csapatok" };
                var rows = _storage.Bajnoksagok
                    .Select(b => new[] { b.Evszam.ToString(), b.Megnevezes, b.CsapatGuids.Count.ToString() })
                    .ToList();
                
                DrawTable(headers, rows);
                
                Console.WriteLine("\n[A] Új szezon | [V] Vissza");
                
                char cmd = char.ToUpper(Console.ReadKey(true).KeyChar);
                if (cmd == 'V') back = true;
                else if (cmd == 'A') AddChampionship();
            }
        }

        public void ResultMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                DrawHeader("FUTAM EREDMÉNYEK");
                
                var headers = new[] { "Dátum", "Futam Neve" };
                var rows = _storage.Eredmenyek
                    .Select(er => new[] { er.Datum.ToString("yyyy.MM.dd"), er.FutamNeve })
                    .ToList();
                
                DrawTable(headers, rows);
                
                Console.WriteLine("\n[A] Új eredmény rögzítése | [V] Vissza");
                
                char cmd = char.ToUpper(Console.ReadKey(true).KeyChar);
                if (cmd == 'V') back = true;
                else if (cmd == 'A') AddResult();
            }
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Bekér egy szöveget a felhasználótól. 
        /// Ha üresen hagyja vagy 'ESC'-et ütne (itt most üres Enter), akkor null-al tér vissza (= Mégse).
        /// </summary>
        private string? PromptInput(string label)
        {
            Console.Write($"{label} (vagy üres Enter a megszakításhoz): ");
            string input = Console.ReadLine();
            return string.IsNullOrWhiteSpace(input) ? null : input.Trim();
        }

        private void AddDriver()
        {
            try
            {
                string? nev = PromptInput("Név");
                if (nev == null) return;

                string? orszag = PromptInput("Ország");
                if (orszag == null) return;

                Console.Write("Kezdő pontszám: "); 
                string pInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(pInput)) return;
                int pont = int.Parse(pInput);
                
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
            string? nev = PromptInput("Csapat neve");
            if (nev != null) _storage.Csapatok.Add(new Csapat(nev));
        }

        private void AssignDriverToTeam()
        {
            string? csNev = PromptInput("Melyik csapatba?");
            if (csNev == null) return;
            var cs = _storage.Csapatok.FirstOrDefault(x => x.Nev.Equals(csNev, StringComparison.OrdinalIgnoreCase));
            
            if (cs != null) 
            {
                string? vNev = PromptInput("Versenyző neve");
                if (vNev == null) return;
                var v = _storage.Versenyzok.FirstOrDefault(x => x.Nev.Equals(vNev, StringComparison.OrdinalIgnoreCase));
                if (v != null) cs.Hozzaad(v);
            }
            else { Console.WriteLine("Nem található csapat."); Thread.Sleep(1000); }
        }

        private void DeleteTeam()
        {
            string? nev = PromptInput("Törlendő csapat neve");
            if (nev != null) _storage.Csapatok.RemoveAll(x => x.Nev.Equals(nev, StringComparison.OrdinalIgnoreCase));
        }

        private void AddChampionship()
        {
            string? nev = PromptInput("Megnevezés");
            if (nev == null) return;
            
            Console.Write("Évszám: ");
            string eInput = Console.ReadLine();
            if (int.TryParse(eInput, out int ev)) _storage.Bajnoksagok.Add(new Bajnoksag(nev, ev));
        }

        private void AddResult()
        {
            string? nev = PromptInput("Futam neve");
            if (nev != null)
            {
                _storage.Eredmenyek.Add(new FutamEredmeny { FutamNeve = nev, Datum = DateTime.Now });
            }
        }

        #endregion
    }
}
