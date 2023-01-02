using HackatonGBABY.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;

namespace HackatonGBABY.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ShowResults(string search)
        {
            ////var searchQuery = Request.QueryString["search"];

            // inputdefault.text
            // var qr = new QueryString(search);
            //var searchQuery = HttpContext.Request.Q["search"];
            string cx = "723c9686491fe420d";
            string apiKey = "AIzaSyDCMU3cRdEA_eC0buKV0Dm6ANX9vR5Tz_k";
            var request = WebRequest.Create("https://www.googleapis.com/customsearch/v1?key=" + apiKey + "&cx=" + cx + "&q=" + search);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseString = reader.ReadToEnd();
            dynamic jsonData = JsonConvert.DeserializeObject(responseString);
            var results = new List<Resualt>();

            List<string> names = new List<string> { "murdery", "ru", "js", "apple" };
            int caunt = 0;
            var isValid = true;
            foreach (var item in jsonData.items)
            {

                foreach (var name in names)
                {
                    if (item.title != null && item.link != null && item.snippet != null)
                    {
                        if (item.title.ToString().ToLower().Contains(name)
                            || item.link.ToString().ToLower().Contains(name)
                            || item.snippet.ToString().ToLower().Contains(name))
                        {
                            caunt++;

                        }




                    }

                }




                results.Add(new Resualt
                {
                    Title = item.title,
                    Link = item.link,
                    Snippet = item.snippet,
                    IsValid = caunt > 0 ? false : true,

                });
            }
            return View(results.ToList());
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}