using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace plant1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            String myName = "Arati Lama";
            int age = 31;
            ViewData["MyName"] = myName;
            ViewData["age"] = age;

            //download JSON data.
            //a webClient giv es us access to data on internet

            using(WebClient webClient = new WebClient())
            {
                string jsonData = webClient.DownloadString("https://www.plantplaces.com/perl/mobile/viewspecimenlocations.pl?Lat=39.14455075&Lng=-84.5093939666667&Range=0.5&Source=location&Version=2");
                QuickType.Welcome welcome = QuickType.Welcome.FromJson(jsonData);
                List<QuickType.Specimen> allSpecimens = welcome.Specimens;
                ViewData["allSpecimens"] = allSpecimens;
                foreach (QuickType.Specimen specimen in allSpecimens)
                {
                    Console.WriteLine(specimen);
                }
            }
        }
    }
}
