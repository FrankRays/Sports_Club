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
    [Authorize]
    public class ClientActivitiesController : Controller
    {
        private ISportRepository _sportRepository;

        public ClientActivitiesController(ISportRepository sportRepository)
        {
            _sportRepository = sportRepository;
        }

        [HttpGet("clientactivities/{clientActivityId}", Name = "GetClientActivity")]
        public IActionResult GetClientActivity(int clientActivityId)
        {
            var activity = _sportRepository.GetClientActivity(clientActivityId);

            if (activity == null)
            {
                return NotFound();
            }

            var activityResult = Mapper.Map<Model.ClientActivity>(activity);
            return Ok(activityResult);
        }

        [HttpPost("{activityId}/clientactivities")]
        [Authorize(Roles = "Client")]
        public IActionResult CreateClientActivity(int activityId,
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

            var clientId = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;

            if(_sportRepository.ClientActivityExists(clientId, activityId))
            {
                return BadRequest("This user has already registered to this activity");
            }

            var finalClientActivity = Mapper.Map<Entities.ClientActivity>(clientActivity);
            finalClientActivity.ClientId = clientId;

            _sportRepository.AddClientActivity(activityId, finalClientActivity);

            if (!_sportRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            var createdClientActivityToReturn = Mapper.Map<Model.ClientActivity>(finalClientActivity);

            return CreatedAtRoute("GetClientActivity", new
            { /*ActivityId = activityId,*/ id = createdClientActivityToReturn.Id }, createdClientActivityToReturn);
        }

        //[HttpDelete("{activityId}/clientactivities/{clientActivityId}")]
        [HttpDelete("{activityId}/clientactivities")]
        [Authorize(Roles = "Client")]
        public IActionResult DeleteClientActivity(int activityId /*int clientActivityId*/)
        {
            var ActivityEntity = _sportRepository.GetActivity(activityId);
            IEnumerable<Entities.ClientActivity> ClientActivities = ActivityEntity.ClientActivities;

            var clientId = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            var clientActivityEntity = new Entities.ClientActivity();

            foreach (var item in ClientActivities)
            {
                if (item.ClientId == clientId)
                    clientActivityEntity = _sportRepository.GetClientActivity(item.Id);
            }

            //var clientActivityEntity = _sportRepository.GetClientActivity(clientActivityId);
            //var clientId = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;

            if (!(clientId == clientActivityEntity.ClientId))
            {
                return Unauthorized();
            }

            if (clientActivityEntity == null)
            {
                return NotFound();
            }

            _sportRepository.DeleteClientActivity(clientActivityEntity);

            if (!_sportRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }
    }
}
