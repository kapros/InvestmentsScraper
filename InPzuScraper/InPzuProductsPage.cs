using Microsoft.Playwright;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPzuScraper
{
    public class InPzuProductsPage
    {
        private const string URL = "https://inpzu.pl/tfi/moje-produkty/fi";
        private readonly IPage _page;

        public InPzuProductsPage(IPage page)
        {
            _page = page;
        }

        public async Task<AccountResults> GoTo()
        {
            var response = await _page.RunAndWaitForResponseAsync(async () =>
              { 
                  await _page.GotoAsync(URL);
              }
            , x => x.Request.Method == "POST" && (x.Request.PostData?.Contains("KLIENT-PRODUKTY-UMOWY")).GetValueOrDefault());
            var body = await response.TextAsync();
            return JsonConvert.DeserializeObject<AccountResults>(body);
        }
    }
}
