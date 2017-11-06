using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sport.Client.Services;
using Sport.Client.ViewModels;
using Sport.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sport.Client.Controllers
{
    public class ActivityController : Controller
    {
        private readonly ISportHttpClient _sportHttpClient;

        public ActivityController(ISportHttpClient sportHttpClient)
        {
            _sportHttpClient = sportHttpClient;
        }

        public async Task<IActionResult> Index()
        {
            var httpClient = await _sportHttpClient.GetClient();
            var response = await httpClient.GetAsync("http://localhost:9877/api/activities/").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var activitiesAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var sportIndexViewModel = new SportIndexViewModel(
                    JsonConvert.DeserializeObject<IList<Activity>>(activitiesAsString).ToList());

                return View(sportIndexViewModel);
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        public IActionResult AddActivity()
        {
            ViewBag.Title = "Naujas užsiėmimas";
            return View(new ActivityForCreationAndUpdate());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddActivity(ActivityForCreationAndUpdate model)
        {
            /*if (!ModelState.IsValid)
            {
                return View();
            }*/

            var httpClient = await _sportHttpClient.GetClient();

            var serializedActivity = JsonConvert.SerializeObject(model);

            var response = await httpClient.PostAsync(
                $"http://localhost:9877/api/activities/",
                new StringContent(serializedActivity, System.Text.Encoding.Unicode, "application/json"))
                .ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }
    }
}
