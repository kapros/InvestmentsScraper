using System.Text;

namespace InvestementsTracker.Services
{
    public class JokeService : IJokeService
    {
        private readonly List<string> _jokes = new()
        {
            "Stirlitz szedł nocą przez las. Nagle zobaczył na drzewie świecące oczy.\r\n- Sowa - pomyślał Stirlitz.\r\n- Sam jesteś sowa - pomyślał Bormann.",
            "Gestapo obstawiło wszystkie wyjścia, ale Stirlitz ich przechytrzył. Uciekł przez wejście.",
            "- Stirlitz, co jest lepsze: radio czy gazeta? - zapytał podejrzliwie Mueller.\r\n- Gazeta, w radio nie zawiniesz śledzia - odparł spokojnie Stirlitz.",
            "W kawiarni \"Elefant\" Stirlitz miał się spotkać z łącznikiem. Nie ustalono niestety żadnego znaku rozpoznawczego.\r\nNa szczęście łącznikowi zwisały spod marynarki szelki spadochronu.",
            "Stirlitz zaatakował z nienacka. Znienacko bronił się tak jak umiał. A Umiał to też był nie lada zawodnik...",
            "Stirlitz szedł ulicami Berlina, coś jednak zdradzało w nim szpiega: może czapka-uszanka, może walonki, a może ciągnący się za nim spadochron?",
            "Stirlitz, spacerując nad brzegiem jeziora, ujrzał ludzi z wędkami.\r\n- Wędkarze - pomyślał Stirlitz.\r\n- Pułkownik Isajew - pomyśleli wędkarze.",
            "Stirlitz ukradkiem karmił niemieckie dzieci.\r\nOd ukradka dzieci puchły i umierały.",
            "Naprzeciw Stirlitza szły trzy umalowane kobiety.\r\n- Kurwy - pomyślał Stirlitz.\r\n- Pułkownik Isajew - pomyślały prostytutki.",
            "Stirlitz wszedł do gabinetu i ujrzał Mullera leżącego na podłodze i nie dającego oznak życia.\r\n- Otruty - pomyślał Stirlitz przyglądając się rączce siekiery wystającej z piersi.",
            "Stirlitz posłał Müllera do diabła. Następnego dnia Diabła odwiedziło Gestapo.",

        };
        public string GetJoke()
        {
            var randomValue = Random.Shared.Next(0, _jokes.Count);
            var randomJoke = _jokes[randomValue];
            var plainTextBytes = Encoding.UTF8.GetBytes(randomJoke);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
