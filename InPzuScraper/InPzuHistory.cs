using Newtonsoft.Json;

namespace InPzuScraper
{
    public class InPzuHistory
    {
        public class Dok
        {
            public int dokId { get; set; }
            public string dokLinkId { get; set; }
            public string dataDodania { get; set; }
            public string typ { get; set; }
            public int lfsId { get; set; }
        }

        public class Dokumenty
        {
            public int dokId { get; set; }
            public string dokLinkId { get; set; }
            public string dataDodania { get; set; }
            public string typ { get; set; }
            public int lfsId { get; set; }
        }

        public class History
        {
            public IEnumerable<HistoryEntry> Entries { get; init; }

            public History(string content)
            {
                Entries = JsonConvert.DeserializeObject<IEnumerable<HistoryEntry>>(content) ?? throw new Exception($"Empty or incorrect history payload: {content}");
            }
        }

        public class HistoryEntry
        {
            public string typ { get; set; }
            public string numer { get; set; }
            public int zlecId { get; set; }
            public int zlecAtId { get; set; }
            public string status { get; set; }
            public string typRejestru { get; set; }
            public string produktId { get; set; }
            public string nazwa { get; set; }
            public string produktNazwaSkrot { get; set; }
            public string linkId { get; set; }
            public string rejestrId { get; set; }
            public string funduszId { get; set; }
            public int umowaId { get; set; }
            public string dataZlec { get; set; }
            public string dataWyceny { get; set; }
            public string dataProc { get; set; }
            public string dataRealizacji { get; set; }
            public string miara { get; set; }
            public int wartosc { get; set; }
            public string waluta { get; set; }
            public List<Dokumenty> dokumenty { get; set; }
            public Dok dok { get; set; }
            public List<Zleceniodawcy> zleceniodawcy { get; set; }
            public string zRejId { get; set; }
            public List<Trn> trn { get; set; }
            public int wartoscTrn { get; set; }
            public string opcja { get; set; }
        }

        public class Trn
        {
            public int trnId { get; set; }
            public string kod { get; set; }
            public string funduszId { get; set; }
            public string rejestrId { get; set; }
            public double liczbaJU { get; set; }
            public double ju { get; set; }
            public string typJU { get; set; }
            public string waluta { get; set; }
            public int wartoscBrutto { get; set; }
            public int wartoscNetto { get; set; }
            public int oplaty { get; set; }
            public int wartoscJU { get; set; }
            public int podatek { get; set; }
            public int sumaOplat { get; set; }
            public int kwotaPotr { get; set; }
            public double cena { get; set; }
            public string rodzajOperacji { get; set; }
            public string powod { get; set; }
            public string zrodlo { get; set; }
            public string dataZlecenia { get; set; }
            public string dataWyceny { get; set; }
            public string dataProc { get; set; }
            public double saldoJU { get; set; }
            public double saldo { get; set; }
            public int procId { get; set; }
            public int brutto { get; set; }
            public int netto { get; set; }

            [JsonProperty("_funduszId.nazwa")]
            public string _funduszIdnazwa { get; set; }
            public string nazwa { get; set; }
            public string zRejId { get; set; }
            public string subRejId { get; set; }
        }

        public class Zleceniodawcy
        {
            public string imie { get; set; }
            public string nazwisko { get; set; }
            public string rodzajOsoby { get; set; }
            public int zleceniodawcaId { get; set; }
        }


    }
}
