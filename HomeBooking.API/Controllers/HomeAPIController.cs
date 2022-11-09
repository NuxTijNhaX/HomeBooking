using HomeBooking.API.Data;
using HomeBooking.API.Logging;
using HomeBooking.API.Models;
using HomeBooking.API.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace HomeBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController] // supply validate or something powerful
    public class HomeAPIController : ControllerBase
    {
        private readonly ILogging _logger;
        public HomeAPIController(ILogging logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetHomes")]
        [ProducesResponseType(200)]
        public ActionResult<IEnumerable<HomeDTO>> GetHomes()
        {
            _logger.Log(LoggingStatusEnum.Information, "Get All Home");
            return Ok(HomeDataStore.homeList);
        }

        [HttpGet("{id:int}", Name = "GetHome")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HomeDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<HomeDTO> GetHome(int id)
        {
            var home = HomeDataStore.homeList.FirstOrDefault(home => home.Id == id);

            if (home == null)
            {
                _logger.Log(LoggingStatusEnum.Error, $"Not Found Home Id = {id}");
                return NotFound();
            }

            return Ok(home);
        }

        [HttpPost(Name = "CreateHome")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<HomeDTO> CreateHome([FromBody] HomeDTO home)
        {
            // Code flow doesn't reach ModelState if [ApiController] is enabled
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            // Custom Validate
            if (HomeDataStore.homeList.FirstOrDefault(h => h.Name.ToLower() == home.Name.ToLower()) != null)
            {
                ModelState.AddModelError("Value", "Home already exists!");
                return BadRequest(ModelState);
            }

            if (home == null)
            {
                return BadRequest();
            }

            if (home.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            home.Id = HomeDataStore.homeList.OrderByDescending(o => o.Id).FirstOrDefault().Id + 1;
            HomeDataStore.homeList.Add(home);

            //return Ok(home);
            return CreatedAtRoute(nameof(GetHome), new { id = home.Id }, home);
        }

        [HttpDelete("{id:int}", Name = "DeleteHome")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHome(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }

            var home = HomeDataStore.homeList.FirstOrDefault(h => h.Id == id);
            if (home == null)
            {
                return NotFound();
            }

            HomeDataStore.homeList.Remove(home);

            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateHome")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHome(int id, [FromBody] HomeDTO home)
        {
            if(home == null || id != home.Id)
            {
                return BadRequest();
            }

            var homeDb = HomeDataStore.homeList.FirstOrDefault(h => h.Id == id);

            if(homeDb == null)
            {
                return NotFound();
            }

            homeDb.Name = home.Name;
            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialHome")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePartialHome(int id, JsonPatchDocument<HomeDTO> pathDoc)
        {
            if (pathDoc == null || id == 0)
            {
                return BadRequest();
            }

            var homeDb = HomeDataStore.homeList.FirstOrDefault(h => h.Id == id);

            if (homeDb == null)
            {
                return NotFound();
            }

            pathDoc.ApplyTo(homeDb, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
