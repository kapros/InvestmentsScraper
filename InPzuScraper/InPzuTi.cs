using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPzuScraper
{
    public class AdresK
    {
        public string typAdresu { get; set; }
        public string krajId { get; set; }
        public string _krajId { get; set; }
        public string kodPocztowy { get; set; }
        public string miasto { get; set; }
        public string ulica { get; set; }
        public string nrDomu { get; set; }
        public string nrLokalu { get; set; }
    }

    public class Dystr
    {
        public int dystrId { get; set; }
        public string nazwa { get; set; }
        public string krajId { get; set; }
        public string kodPocztowy { get; set; }
        public string miasto { get; set; }
        public string ulica { get; set; }
    }

    public class Fundusze
    {
        public string funduszId { get; set; }
        public string rejestrId { get; set; }
        public int udzial { get; set; }
    }

    public class Pozycje
    {
        public string funduszId { get; set; }
        public string rejestrId { get; set; }
        public int udzial { get; set; }
    }

    public class Produkty
    {
        public string grProduktId { get; set; }
        public string produktId { get; set; }
        public bool akt { get; set; }
        public List<Umowy> umowy { get; set; }
        public int wplaty { get; set; }
        public int wyplaty { get; set; }
        public double wartosc { get; set; }
        public double wynik { get; set; }
        public int sumaNabyc { get; set; }
        public int sumaWplat { get; set; }
        public string typJu { get; set; }
        public string typUmowy { get; set; }
    }

    public class Rej
    {
        public string rejestrId { get; set; }
        public string funduszId { get; set; }
        public int ucId { get; set; }
        public string zRejId { get; set; }
        public List<Zlecenium> zlecenia { get; set; }
        public string typRejestru { get; set; }
        public string produktId { get; set; }
        public string produktNazwa { get; set; }
        public string nazwa { get; set; }
        public double liczbaJU { get; set; }
        public string typJu { get; set; }
        public double cena { get; set; }
        public double wartosc { get; set; }
        public string waluta { get; set; }
        public string dataWyceny { get; set; }
        public string dataOtwarcia { get; set; }
        public int rejAkt { get; set; }
        public AdresK adresK { get; set; }
        public bool blokada { get; set; }
        public string mediumPotwierdzenia { get; set; }
        public int pokId { get; set; }
        public string pokNazwa { get; set; }
        public Dystr dystr { get; set; }
        public string rachunekWplat { get; set; }
        public List<object> agr { get; set; }
        public string rejestrLinkId { get; set; }
        public int dystrId { get; set; }
        public string dystrNazwa { get; set; }
    }

    public class AccountResults
    {
        public List<Produkty> produkty { get; set; }
        public int wplaty { get; set; }
        public int wyplaty { get; set; }
        public double wartosc { get; set; }
        public double wynik { get; set; }
        public int sumaOstWplat { get; set; }
        public bool uzupelnioneDane { get; set; }
        public bool zweryfikowany { get; set; }
        public bool wymaganaWeryfikacja { get; set; }
        public string wideoStan { get; set; }
        public bool weryfikacjaPlatnosc { get; set; }
    }

    public class Strategia
    {
        public int strategiaId { get; set; }
        public string kodStrategii { get; set; }
        public string nazwa { get; set; }
        public int offset { get; set; }
        public string waluta { get; set; }
        public string typStrategii { get; set; }
        public bool pub { get; set; }
        public List<Pozycje> pozycje { get; set; }
        public List<Fundusze> fundusze { get; set; }
    }

    public class UmowaPrzeds
    {
    }

    public class Umowy
    {
        public bool akt { get; set; }
        public int umowaId { get; set; }
        public string linkId { get; set; }
        public string typUmowy { get; set; }
        public string sposobWysylkiPotw { get; set; }
        public string typPotwierdzenia { get; set; }
        public string produktId { get; set; }
        public string rejestrId { get; set; }
        public string typJu { get; set; }
        public int parasolId { get; set; }
        public int pokId { get; set; }
        public string pokNazwa { get; set; }
        public UmowaPrzeds umowaPrzeds { get; set; }
        public Dystr dystr { get; set; }
        public List<Rej> rej { get; set; }
        public Strategia strategia { get; set; }
        public AdresK adresK { get; set; }
        public string waluta { get; set; }
        public double wartosc { get; set; }
        public int wplaty { get; set; }
        public int wyplaty { get; set; }
        public double wynik { get; set; }
        public int sumaNabyc { get; set; }
        public int sumaWplat { get; set; }
    }

    public class Zlecenium
    {
        public string kod { get; set; }
        public bool akt { get; set; }
        public string powod { get; set; }
    }


}
