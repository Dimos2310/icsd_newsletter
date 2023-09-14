using System.Diagnostics;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using NewsLetter.Authorization;
using NewsLetter.Models;
using NewsLetter.Services;

namespace NewsLetter.Controllers
{

    [ApiController]
    [Route("/api/topic")]
    public class TopicController : Controller
    {
        private readonly TopicService topicService;
        private string authorizationVal = null;
        public string LoggedInUser => User.Identity.Name;

        public TopicController()
        {
            this.topicService = new TopicService();
        }

        [HttpGet]
        [Route("/api/topic/{topicId}")]
        [RoleAccess("dimosiografos", "epimelitis")]
        public IActionResult GetTopic(int topicId)
        {
            if (topicId <= 0)
                return BadRequest();

            var statusCode = System.Net.HttpStatusCode.OK;
            return StatusCode((int)statusCode, topicService.GetTopic(topicId));
        }

        [HttpGet]
        public List<TopicMod> GetTopics()
        {
            return topicService.GetTopics();
        }


        [HttpPost]
        //[AuthorizationHeaderFilter]
        [RoleAccess("dimosiografos", "epimelitis")]
        public IActionResult NewTopic([FromBody] Topic topic)
        {
            if (topic == null)
                return BadRequest();
            //elegxos twn stoxeiwn tou topic gia adeia properties
            var validator = new TopicValidator();
            ValidationResult results = validator.Validate(topic);
            if (!results.IsValid)
            {
                foreach (var failure in results.Errors)
                {
                    var message = ("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                    var statusCode = System.Net.HttpStatusCode.BadRequest;
                    return StatusCode((int)statusCode, message);
                }
            }

           topicService.CreateTopic(topic, LoggedInUser);
            return Ok();
        }

        [HttpPut]
        [Route("/api/topic/{topicId}")]
        [RoleAccess("dimosiografos", "epimelitis")]
        public IActionResult ChangeTopic(int topicId,[FromBody] Topic topic)
        {
            if (topic == null)
                return BadRequest();

            Topic topic_for_change = topicService.GetTopic(topicId);
            if (topic_for_change == null){
                return StatusCode(404, "Topic Not Found");
            }

            if (topic.Name != null){
                topic_for_change.Name = topic.Name;
            }
            if (topic.ParentTopic != null){
                topic_for_change.ParentTopic = topic.ParentTopic;   
            }

            topicService.updateTopic(topic_for_change);
            return Ok();
        }

        [HttpPut]
        [Route("/api/topic/approve/{topicId}")]
        [RoleAccess("epimelitis")]
        public IActionResult ApproveTopic(int topicId)
        {
            if (topicId <= 0)
                return BadRequest();

            Topic topic_for_change = topicService.GetTopic(topicId);
            if (topic_for_change == null){
                return StatusCode(404, "Topic Not Found");
            }

            topic_for_change.Status = TopicStatus.ACCEPTED.ToString();
            
            topicService.updateTopic(topic_for_change);
            return Ok();
        }

         [HttpPut]
        [Route("/api/topic/reject/{topicId}")]
        [RoleAccess("epimelitis")]
        public IActionResult RejectTopic(int topicId)
        {
            if (topicId <= 0)
                return BadRequest();

            Topic topic_for_change = topicService.GetTopic(topicId);
            if (topic_for_change == null){
                return StatusCode(404, "Topic Not Found");
            }

            if (topic_for_change.Status != TopicStatus.CREATED.ToString()){
                return StatusCode(404, "To topic den exei katallilo status gia aporripsi");
           }
  
            topicService.RemoveTopic(topic_for_change.Id);
            return Ok();
        }



        [HttpDelete]
        [Route("/api/topic/{topicId}")]
        [RoleAccess("epimelitis")]
        public IActionResult RemoveTopic(int topicId)
        {
            if (topicId <= 0)
                return BadRequest();
            topicService.RemoveTopic(topicId);
            return Ok();
        }
    }
}