using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using Sport.Client.Services;
using Sport.Client.ViewModels;
using Sport.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sport.Client.Controllers
{
    [Authorize]
    public class ActivityController : Controller
    {
        private readonly ISportHttpClient _sportHttpClient;
        public string port = "http://localhost:44396/";

        public ActivityController(ISportHttpClient sportHttpClient)
        {
            _sportHttpClient = sportHttpClient;
        }

        public async Task<IActionResult> Index()
        {
            await WriteOutIdentityInformation();

            var httpClient = await _sportHttpClient.GetClient();
            var response = await httpClient.GetAsync($"{port}/api/activities/").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var activitiesAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var sportIndexViewModel = new SportIndexViewModel(
                    JsonConvert.DeserializeObject<IList<Activity>>(activitiesAsString).ToList());

                return View(sportIndexViewModel);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return RedirectToAction("AccessDenied", "Authorization");
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
            if (!ModelState.IsValid)
            {
                return View();
            }

            var httpClient = await _sportHttpClient.GetClient();

            var serializedActivity = JsonConvert.SerializeObject(model);

            var response = await httpClient.PostAsync(
                $"{port}/api/activities/",
                new StringContent(serializedActivity, System.Text.Encoding.Unicode, "application/json"))
                .ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        public async Task<IActionResult> EditActivity(int id)
        {
            var httpClient = await _sportHttpClient.GetClient();

            var response = await httpClient.GetAsync($"{port}/api/activities/{id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var activityAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var deserializedActivity = JsonConvert.DeserializeObject<Activity>(activityAsString);

                var editActivity = new Activity()
                {
                    Id = deserializedActivity.Id,
                    Name = deserializedActivity.Name,
                    Beginning = deserializedActivity.Beginning,
                    Ending = deserializedActivity.Ending
                };

                return View(editActivity);
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditActivity(Activity activity)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var activityForUpdate = new ActivityForCreationAndUpdate()
            {
                Name = activity.Name,
                Beginning = activity.Beginning,
                Ending = activity.Ending
            };

            var serializedActivityForUpdate = JsonConvert.SerializeObject(activityForUpdate);

            var httpClient = await _sportHttpClient.GetClient();

            var response = await httpClient.PutAsync(
                $"{port}/api/activities/{activity.Id}",
                new StringContent(serializedActivityForUpdate, System.Text.Encoding.Unicode, "application/json"))
                .ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        public async Task<IActionResult> DeleteActivity(int id)
        {
            var httpClient = await _sportHttpClient.GetClient();

            var response = await httpClient.DeleteAsync($"{port}/api/activities/{id}").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        public async Task Logout()
        {
            await HttpContext.Authentication.SignOutAsync("Cookies");
            await HttpContext.Authentication.SignOutAsync("oidc");
        }

        public async Task WriteOutIdentityInformation()
        {
            //get the saved identity token
            var identityToken = await HttpContext.Authentication
                .GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            //write it out
            Debug.WriteLine($"Identity token: {identityToken}");

            // write out the user claims
            foreach (var claim in User.Claims)
            {
                Debug.WriteLine($"Claim type: { claim.Type} - claim value: { claim.Value}");
            }
        }

        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> GetTrainerActivities()
        {
            var httpClient = await _sportHttpClient.GetClient();
            var response = await httpClient.GetAsync($"{port}/api/activities/trainer").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var activitiesAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var sportIndexViewModel = new SportIndexViewModel(
                JsonConvert.DeserializeObject<IList<Activity>>(activitiesAsString).ToList());

                return View(sportIndexViewModel);
            }

            throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }

        /*public IActionResult AddClientActivity(int id)
        {
            ViewBag.Title = "Naujas užsiėmimas";
            return (id, new ClientActivityForCreation());
        }*/

        //[Authorize(Roles = "Client")]

        public async Task<IActionResult> AddClientActivity(int id)
        {
            ClientActivityForCreation model = new ClientActivityForCreation();

            if (!ModelState.IsValid)
            {
                return View();
            }

            var httpClient = await _sportHttpClient.GetClient();

            var serializedClientActivity = JsonConvert.SerializeObject(model);

            var response = await httpClient.PostAsync(
                $"{port}/api/activities/{id}/clientactivities",
                new StringContent(serializedClientActivity, System.Text.Encoding.Unicode, "application/json"))
                .ConfigureAwait(false);

                return RedirectToAction("Index");
            

            //throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
        }
    }
}
