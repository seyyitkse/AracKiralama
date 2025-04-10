using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RentCar.Dto.LocationDtos;

namespace RentCar.WebUI.Controllers
{
    public class DefaultController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DefaultController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();

            // Alınacak lokasyonlar için API çağrısı
            var responseMessage = await client.GetAsync("https://localhost:44308/api/Locations");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultLocationDto>>(jsonData);

                var values2 = values.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.LocationID.ToString()
                }).ToList();

                ViewBag.v = values2;
            }
            else
            {
                ViewBag.v = new List<SelectListItem>(); // Avoid null if the API call fails
            }

            // Teslim edilecek lokasyonlar için API çağrısı
            var returnResponseMessage = await client.GetAsync("https://localhost:44308/api/Locations");
            if (returnResponseMessage.IsSuccessStatusCode)
            {
                var jsonData = await returnResponseMessage.Content.ReadAsStringAsync();
                var returnValues = JsonConvert.DeserializeObject<List<ResultLocationDto>>(jsonData);

                var returnValues2 = returnValues.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.LocationID.ToString()
                }).ToList();

                ViewBag.ReturnLocations = returnValues2;
            }
            else
            {
                ViewBag.ReturnLocations = new List<SelectListItem>(); // Avoid null if the API call fails
            }

            return View();
        }

        [HttpPost]
        public IActionResult Index(string book_pick_date, string book_off_date, string time_pick, string time_off, string locationID, string returnLocationID)
        {
            // Tarih ve saat formatlarını işleme
            DateTime pickDate = DateTime.ParseExact(book_pick_date, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime offDate = DateTime.ParseExact(book_off_date, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);

            TempData["bookpickdate"] = pickDate.ToString("yyyy-MM-dd"); // ISO formatı
            TempData["bookoffdate"] = offDate.ToString("yyyy-MM-dd");
            TempData["timepick"] = time_pick; // Flatpickr doğrudan uygun formatta gönderiyor
            TempData["timeoff"] = time_off;
            TempData["locationID"] = locationID;
            TempData["returnLocationID"] = returnLocationID; // Teslim edilecek lokasyon bilgisi

            return RedirectToAction("Index", "RentACarList");
        }
    }
}
