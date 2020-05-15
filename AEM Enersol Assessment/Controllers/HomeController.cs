using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AEM_Enersol_Assessment.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace AEM_Enersol_Assessment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, Data.AEMContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Menu()
        {
            return View();
        }

        //My code start here
        private readonly Data.AEMContext _context;

        [HttpPost]
        public async Task<IActionResult> LoginAPI(Login login)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/Home/Index");
            }

            string url = "http://test-demo.aem-enersol.com/api/Account/Login"; // Login API URL
            string username = login.UserName;
            string password = login.Password;
            var formVars = new Dictionary<string, string>();
            formVars.Add(username, password);
            
            string Serialized = JsonConvert.SerializeObject(login);
            using (var client = new HttpClient())
            {
                var content = new StringContent(Serialized, UnicodeEncoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                var contents = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    contents = contents.Replace("\"", "");

                    HttpContext.Session.SetString("Token", contents);
                    TempData["Token"] = "My Token: " + HttpContext.Session.GetString("Token");
                    return RedirectToAction("Menu", "Home");
                }
                else
                {
                   
                    TempData["Failed"] = "Error: " + response.ReasonPhrase;
                    return RedirectToAction("Index", "Home");

                }
            }
      
        }


        [HttpPost]
        public async Task<IActionResult> GetPlatfromWellActual()
        {
            string url = "http://test-demo.aem-enersol.com/api/PlatformWell/GetPlatformWellActual";
            bool result = await GetPlatfromWell(url);

            if (result)
            {
                return RedirectToAction("Menu", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> GetPlatfromWellDummy()
        {
            string url = "http://test-demo.aem-enersol.com/api/PlatformWell/GetPlatformWellDummy";
            bool result = await GetPlatfromWell(url);

            if (result)
            {
                return RedirectToAction("Menu", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        private async Task<bool> GetPlatfromWell(string url)
        {
            
            string accessToken = HttpContext.Session.GetString("Token");

            // Use the access token to call a protected web API.
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.GetAsync(url);
                var contents = await response.Content.ReadAsStringAsync();
                var jsonContent = JsonConvert.DeserializeObject<List<Platform>>(contents);

                if (response.IsSuccessStatusCode)
                {

                    await AddData(jsonContent);
                    return true;
                }
                else
                {

                    TempData["Failed"] = "Error: " + response.ReasonPhrase;
                    return false;

                }
            }
        }

        private async Task AddData(List<Platform> Platforms)
        {
            if (!ModelState.IsValid)
            {
                return;
            }

            foreach (Platform platform in Platforms)
            {
                var id = platform.ID;

                if (!PlatformExists(id))
                {
                    _context.Platform.Add(platform);
                }
                else
                {
                    var dataPlatform = await _context.Platform.FirstOrDefaultAsync(m => m.ID == id);
                    dataPlatform.Latitude = platform.Latitude;
                    dataPlatform.Longitude = platform.Longitude;
                    dataPlatform.CreatedAt = Convert.ToDateTime(platform.CreatedAt);
                    dataPlatform.UpdatedAt = Convert.ToDateTime(platform.UpdatedAt);

                    _context.Attach(dataPlatform).State = EntityState.Modified;

                    var dataWells = _context.Well.Where(m => m.PlatformID == id).ToList();

                    int count = dataWells.Count();

                    for (int i = 0; i < count; i++)
                    {
                        if (!WellExists(dataWells[i].ID))
                        {
                            _context.Well.Add(dataWells[i]);
                        }
                        else
                        {
                            dataWells[i].PlatformID = platform.Well[i].PlatformID;
                            dataWells[i].UniqueName = platform.Well[i].UniqueName;
                            dataWells[i].Latitude = platform.Well[i].Latitude;
                            dataWells[i].Longitude = platform.Well[i].Longitude;
                            dataWells[i].CreatedAt = Convert.ToDateTime(platform.Well[i].CreatedAt);
                            dataWells[i].UpdatedAt = Convert.ToDateTime(platform.Well[i].UpdatedAt);

                            _context.Attach(dataWells[i]).State = EntityState.Modified;

                        }

                    }


                }
                
            }
            await _context.SaveChangesAsync();

        }


        private bool PlatformExists(int id)
        {
            return _context.Platform.Any(e => e.ID == id);
        }

        private bool WellExists(int id)
        {
            return _context.Well.Any(e => e.ID == id);
        }

        //My code end here

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}
