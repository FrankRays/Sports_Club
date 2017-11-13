using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sport.API.Services;
using Sport.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sport.API.Controllers
{
    [Route("api/activities")]
    //[Authorize]
    public class ClientActivitiesController : Controller
    {
        private ISportRepository _sportRepository;

        public ClientActivitiesController(ISportRepository sportRepository)
        {
            _sportRepository = sportRepository;
        }

        [HttpPost("{activityId}/clientactivities")]
        public IActionResult CreatePointOfInterest(int activityId,
            [FromBody] ClientActivityForCreation clientActivity)
        {
            if (clientActivity == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!_sportRepository.ActivityExists(activityId))
            {
                return NotFound();
            }

            /*var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(
                c => c.PointsOfInterest).Max(p => p.Id);

            var finalPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };*/

            var finalClientActivity = Mapper.Map<Entities.ClientActivity>(clientActivity);

            _sportRepository.AddClientActivity(activityId, finalClientActivity);

            if (!_sportRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            var createdClientActivityToReturn = Mapper.Map<Model.ClientActivity>(finalClientActivity);

            return CreatedAtRoute("GetClientActivity", new
            { cityId = activityId, id = createdClientActivityToReturn.Id }, createdClientActivityToReturn);
        }
    }
}
