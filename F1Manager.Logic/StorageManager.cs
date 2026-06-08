using System.Text.Json;

namespace F1Manager.Logic
{
    public class StorageManager
    {
        private const string DriversFile = "drivers.json";
        private const string TeamsFile = "teams.json";
        private const string ChampionshipsFile = "championships.json";

        public List<Versenyzo> Versenyzok { get; set; } = new();
        public List<Csapat> Csapatok { get; set; } = new();
        public List<Bajnoksag> Bajnoksagok { get; set; } = new();

        public void LoadAll()
        {
            Versenyzok = Load<Versenyzo>(DriversFile);
            Csapatok = Load<Csapat>(TeamsFile);
            Bajnoksagok = Load<Bajnoksag>(ChampionshipsFile);
        }

        public void SaveAll()
        {
            Save(DriversFile, Versenyzok);
            Save(TeamsFile, Csapatok);
            Save(ChampionshipsFile, Bajnoksagok);
        }

        private List<T> Load<T>(string fileName)
        {
            if (!File.Exists(fileName)) return new List<T>();
            string json = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }

        private void Save<T>(string fileName, List<T> data)
        {
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileName, json);
        }
    }
}
