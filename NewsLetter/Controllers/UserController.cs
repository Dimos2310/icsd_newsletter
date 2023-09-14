using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using NewsLetter.Authorization;
using NewsLetter.Models;
using NewsLetter.Services;


namespace NewsLetter.Controllers
{
    [ApiController]
    [Route("/api/user")]
    public class UserController : Controller
    {
        private readonly UserServices userServices;
        private String authorizationVal = null;

        public string LoggedInUser => User.Identity.Name;

        public UserController()
        {
            this.userServices = new UserServices();
            
        }

    
        [HttpPost]
        public IActionResult Register([FromBody] User user)
        {
            //elegxos gia egkuri eisodo twn stoixeiwn tou xristi
            if (user == null) {
               return BadRequest();
            }
            if (user.Username == null || user.Password == null) {
                return BadRequest();
            }
            //dimiourgia tou xristi
            userServices.RegisterUser(user);
            return Ok();
        }

        [HttpGet]
        public String Login([FromBody] User user)
        {
            return userServices.LoginUser(user);
        }

        [HttpPatch]
        [RoleAccess("dimosiografos", "epimelitis")]
        public IActionResult UpdateUser([FromBody] User user)
        {
            userServices.UpdateUser(user);
            return Ok();
        }

        [HttpPatch]
        [Route("/api/user/updatepassword")]
        [RoleAccess("dimosiografos", "epimelitis", "admin")]
        public IActionResult UpdatePassword(String oldp, String newp)
        {
            userServices.changePassword(oldp, newp);
            return Ok();
        }


        [HttpPatch]
        [Route("/api/user/setrole")]
        [RoleAccess("admin")]
        public IActionResult SetRole(int id, String role)
        {
            if (id <=0 )
                return BadRequest();
            if (role != "dimosiografos" && role != "epimelitis")
                return BadRequest("Wrong role");
            userServices.setRole(id, role);
            return Ok();;
        }


        [HttpPatch]
        [RoleAccess("admin")]
        [Route("/api/user/enable/{id}")]
        public IActionResult EnableUser(int id)
        {   
            if (id <=0 )
                return BadRequest("Wrong id");
            userServices.SetUserActive(id);
            return Ok();
        }

        [HttpPatch]
        [RoleAccess("admin")]
        [Route("/api/user/disable/{id}")]
        public IActionResult DisableUser(int id)
        {
            if (id <=0 )
                return BadRequest();
            userServices.SetUserInActive(id);
            return Ok();
        }

        [HttpGet]
        [Route("getusers")]
        [RoleAccess("admin")]
        public List<User> GetUsers()
        {
            var users = userServices.GetAllUsers();
            return users;
        }

        [HttpPatch]
        [Route("godmode/{id}")]
        public IActionResult Makeadmin(int id)
        {
            var result = userServices.MakeAdmin(id);
            userServices.SetUserActive(id);
            if (result == "OK")
                return Ok();
            else return NotFound();
        }

        
    }
}
