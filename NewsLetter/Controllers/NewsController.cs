using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using NewsLetter.Authorization;
using NewsLetter.Models;
using NewsLetter.Models.DataTransferObjects;
using NewsLetter.Services;
using FluentValidation.Results;
using System.Formats.Asn1;

namespace NewsLetter.Controllers
{
    [ApiController]
    [Route("/api/news")]
    public class NewsController : Controller
    {
        private readonly NewsService newsService;
        private String authorizationVal = null;

        public NewsController()
        {
            this.newsService = new NewsService();
        }

        [HttpPost]
        [RoleAccess("dimosiografos", "epimelitis")]
        public IActionResult CreateNews ([FromBody] News news)
        {
            if (news == null)
                return BadRequest();
            //elegxos twn stoxeiwn tou topic gia adeia properties
            var validator = new NewsValidator();
            ValidationResult results = validator.Validate(news);
            if (!results.IsValid)
            {
                foreach (var failure in results.Errors)
                {
                    var message = ("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                    var statusCode = System.Net.HttpStatusCode.BadRequest;
                    return StatusCode((int)statusCode, message);
                }
            }

            newsService.CreateNews(news);
            return Ok();
        }

        [HttpGet]
        public List<NewsMod> GetAllNews(){
            return newsService.GetAllNews();
        }

        [HttpPatch]
        [Route("/api/news/update/{id}")]
        [RoleAccess("dimosiografos", "epimelitis")]
        public IActionResult UpdateNews(int id, [FromBody] News news)
        {
           newsService.UpdateNews(id, news);
           return Ok();
        }

        [HttpPatch]
        [Route("/api/news/approve/{id}")]
        [RoleAccess("epimelitis")]
        public IActionResult approveNews(int id){
                if (newsService.approveNews(id))
                return Ok();
                else return BadRequest();
        }

        [HttpPatch]
        [Route("/api/news/submit/{id}")]
        [RoleAccess("dimosiografos", "epimelitis")]
        public IActionResult submitNews(int id){
                if (newsService.submitNews(id))
                return Ok();
                else return BadRequest();
        }

        [HttpPatch]
        [Route("/api/news/publish/{id}")]
        [RoleAccess("epimelitis")]
        public IActionResult publishNews(int id){
                if (newsService.publishNews(id))
                return Ok();
                else return BadRequest();
        }

        [HttpPatch]
        [Route("/api/news/reject/{id}")]
        [RoleAccess("epimelitis")]
        public IActionResult rejectNews(int id){
                if (newsService.rejectNews(id))
                return Ok();
                else return BadRequest();
        }

       
        [HttpGet]
        [Route("/api/news/search")]
        public List<News> SearchForNews([FromQuery] string[] searchw)
        {
            if(searchw.Length < 1) {
                return new List<News>();
            }
            return newsService.SearchNews(searchw);   
        }

        [HttpGet]
        [Route("/api/news/{newsId}")]
        [RoleAccess("dimosiografos", "epimelitis")]
        public IActionResult GetNews(int newsId)
        {
            if (newsId <= 0)
                return BadRequest();

            var statusCode = System.Net.HttpStatusCode.OK;
            return StatusCode((int)statusCode, newsService.GetNews(newsId));
        }

    }
}