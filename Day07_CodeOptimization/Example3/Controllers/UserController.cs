using Code_Optimization_Example_3_solution.Models;
using Code_Optimization_Example_3_solution.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Code_Optimization_Example_3_solution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserService userService;

        public UserController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpGet()]
        public ActionResult<IEnumerable<UserViewModel>> GetUsers()
        {
            var users = userService
                            .GetUser();
                            

            return Ok(users);
        }

//        "IEnumerable tüm veriyi memory'e çeker, sonra işlem yapar. IQueryable ise işlemi database'de yapar, sadece sonucu getirir. Performance açısından IQueryable çok daha hızlıdır çünkü az veri transfer eder."
//Basit Örnekle Anlat:
//"1 milyon user'ımız var, sadece 18 yaşındakileri istiyoruz:

//IEnumerable: 1 milyon kaydı çek, sonra filtrele
//IQueryable: Database'ye 'sadece 18 yaşındakileri getir' de"

        [HttpPost()]
        public ActionResult<bool> CreateUser([FromBody] CreateUserRequestModel user)
        {
            var isUserCreated = userService.CreateUser(user);
            return Ok(isUserCreated);
        }
    }
}
