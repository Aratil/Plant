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
            long precip = 0;
            IList<QuickType.Specimen> waterLovingSpecimens = new List<QuickType.Specimen>();

            //download JSON data.
            //a webClient giv es us access to data on internet

            using (WebClient webClient = new WebClient())
            {
                string weatherAPIkey = System.IO.File.ReadAllText("wetherAPI.txt");
                string weatherData = webClient.DownloadString("https://api.weatherbit.io/v2.0/current?&city=Cincinnati&country=USA&key=" + weatherAPIkey);


                QuickTypeWeather.Weather weather = QuickTypeWeather.Weather.FromJson(weatherData);
                QuickTypeWeather.Datum[] allWeatherData = weather.Data;

                foreach (QuickTypeWeather.Datum datum in allWeatherData)
                {
                    precip = datum.Precip;
                }

                string plantData = webClient.DownloadString("http://www.plantplaces.com/perl/mobile/viewplantsjsonarray.pl?WetTolerant=on");
                QuickTypePlant.Plant[] allPlants = QuickTypePlant.Plant.FromJson(plantData);

                IDictionary<long, QuickTypePlant.Plant> plantDictionary = new Dictionary<long, QuickTypePlant.Plant>();
                foreach (QuickTypePlant.Plant plant in allPlants)
                {
                    plantDictionary.Add(plant.Id, plant);
                }

                string jsonData = webClient.DownloadString("https://www.plantplaces.com/perl/mobile/viewspecimenlocations.pl?Lat=39.14455075&Lng=-84.5093939666667&Range=0.5&Source=location&Version=2");
                QuickType.Welcome welcome = QuickType.Welcome.FromJson(jsonData);
                List<QuickType.Specimen> allSpecimens = welcome.Specimens;




                foreach (QuickType.Specimen specimen in allSpecimens)
                {
                    Console.WriteLine(specimen);
                    if (plantDictionary.ContainsKey(specimen.PlantId))
                    {
                        waterLovingSpecimens.Add(specimen);

                    }

                }



                if (precip < 1)
                {
                    ViewData["WeatherMessage"] = "Not much precip; water these water-loving plants.";
                    // make the specimen data available to our web page.
                    ViewData["allSpecimens"] = waterLovingSpecimens;
                }
                else
                {
                    ViewData["WeatherMessage"] = "Lots of rain, these plants will love it!";
                    ViewData["allSpecimens"] = waterLovingSpecimens;
                }
            }
         
        }
        
    }
}
