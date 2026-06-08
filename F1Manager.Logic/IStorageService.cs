using System.Collections.Generic;

namespace F1Manager.Logic
{
    /// <summary>
    /// SOLID - Dependency Inversion Principle (DIP): 
    /// Absztrakciót hozunk létre az adatkezeléshez, hogy a UI ne függjön a konkrét JSON megvalósítástól.
    /// </summary>
    public interface IStorageService
    {
        List<Versenyzo> Versenyzok { get; set; }
        List<Csapat> Csapatok { get; set; }
        List<Bajnoksag> Bajnoksagok { get; set; }
        List<FutamEredmeny> Eredmenyek { get; set; }

        void LoadAll();
        void SaveAll();
    }
}
