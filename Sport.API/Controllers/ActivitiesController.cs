using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sport.API.Entities;
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
    public class ActivitiesController : Controller
    {
        private ISportRepository _sportRepository;

        public ActivitiesController(ISportRepository sportRepository)
        {
            _sportRepository = sportRepository;
        }

        [HttpGet()]
        public IActionResult GetActivities()
        {
            var activityEntities = _sportRepository.GetActivities();
            var results = Mapper.Map<IEnumerable<Model.Activity>>(activityEntities);

            return Ok(results);
        }

        [HttpGet("trainer")]
        [Authorize(Roles = "Trainer")]
        public IActionResult GetTrainerActivities()
        {
            var trainerId = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            var activityEntities = _sportRepository.GetTrainerActivities(trainerId);
            var results = Mapper.Map<IEnumerable<Model.Activity>>(activityEntities);

            return Ok(results);
        }

        [HttpGet("{activityId}", Name = "GetActivity")]
        public IActionResult GetActivity(int activityId)
        {
            var activity = _sportRepository.GetActivity(activityId);

            if (activity == null)
            {
                return NotFound();
            }

            var activityResult = Mapper.Map<Model.Activity>(activity);
            return Ok(activityResult);
        }

        [HttpPost()]
        [Authorize(Roles = "Trainer")]
        public IActionResult CreateActivity([FromBody] ActivityForCreationAndUpdate activity)
        {
            if (activity == null)
            {
                return NotFound();
            }

            var finalActivity = Mapper.Map<Entities.Activity>(activity);

            var trainerId = User.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            finalActivity.TrainerId = trainerId;

            _sportRepository.AddActivity(finalActivity);

            if (!_sportRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            var createdActivityToReturn = Mapper.Map<Model.Activity>(finalActivity);

            return CreatedAtRoute("GetActivity", 
                new { activityId = createdActivityToReturn.Id }, 
                createdActivityToReturn);
        }

        [HttpPut("{activityId}")]
        [Authorize(Roles = "Trainer")]
        public IActionResult UpdateActivity(int activityId,
            [FromBody] ActivityForCreationAndUpdate activity)
        {
            if (activity == null)
            {
                return BadRequest();
            }

            var activityEntity = _sportRepository.GetActivity(activityId);
            if (activityEntity == null)
            {
                return NotFound();
            }

            Mapper.Map(activity, activityEntity);

            if (!_sportRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [HttpDelete("{activityId}")]
        [Authorize(Roles = "Trainer")]
        public IActionResult DeleteActivity(int activityId)
        {
            var activityEntity = _sportRepository.GetActivity(activityId);

            if (activityEntity == null)
            {
                return NotFound();
            }

            _sportRepository.DeleteActivity(activityEntity);

            if (!_sportRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }
    }
}
