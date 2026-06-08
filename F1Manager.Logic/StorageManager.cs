using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace F1Manager.Logic
{
    /// <summary>
    /// Az adatok fájlrendszerbe való mentéséért és betöltéséért felelős osztály.
    /// </summary>
    public class StorageManager
    {
        // Fájlnevek konstansai
        private const string DriversFile = "drivers.json";
        private const string TeamsFile = "teams.json";
        private const string ChampionshipsFile = "championships.json";
        private const string ResultsFile = "results.json";

        // Memóriában tárolt listák
        public List<Versenyzo> Versenyzok { get; set; } = new List<Versenyzo>();
        public List<Csapat> Csapatok { get; set; } = new List<Csapat>();
        public List<Bajnoksag> Bajnoksagok { get; set; } = new List<Bajnoksag>();
        public List<FutamEredmeny> Eredmenyek { get; set; } = new List<FutamEredmeny>();

        /// <summary>
        /// Betölti az összes adatot a JSON fájlokból.
        /// </summary>
        public void LoadAll()
        {
            Versenyzok = Load<Versenyzo>(DriversFile);
            Csapatok = Load<Csapat>(TeamsFile);
            Bajnoksagok = Load<Bajnoksag>(ChampionshipsFile);
            Eredmenyek = Load<FutamEredmeny>(ResultsFile);
        }

        /// <summary>
        /// Elmenti a memóriában lévő listákat JSON fájlokba.
        /// </summary>
        public void SaveAll()
        {
            Save(DriversFile, Versenyzok);
            Save(TeamsFile, Csapatok);
            Save(ChampionshipsFile, Bajnoksagok);
            Save(ResultsFile, Eredmenyek);
        }

        /// <summary>
        /// Generikus betöltő metódus.
        /// </summary>
        private List<T> Load<T>(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return new List<T>();
            }

            try
            {
                string json = File.ReadAllText(fileName);
                return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            catch (Exception)
            {
                // Hiba esetén (pl. sérült fájl) üres listával térünk vissza
                return new List<T>();
            }
        }

        /// <summary>
        /// Generikus mentő metódus formázott JSON kimenettel.
        /// </summary>
        private void Save<T>(string fileName, List<T> data)
        {
            var options = new JsonSerializerOptions 
            { 
                WriteIndented = true // Szép, olvasható formátum
            };
            
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(fileName, json);
        }
    }
}
