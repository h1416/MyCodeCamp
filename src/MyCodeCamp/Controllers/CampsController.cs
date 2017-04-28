using Microsoft.AspNetCore.Mvc;
using MyCodeCamp.Data;
using MyCodeCamp.Data.Entities;
using System.Threading.Tasks;

namespace MyCodeCamp.Controllers
{
    [Route("api/[controller]")]
    public class CampsController : Controller
    {
        private ICampRepository _repo;

        public CampsController(ICampRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                var camps = _repo.GetAllCamps();

                return Ok(camps);
            }
            catch
            {
            }

            return BadRequest();
        }

        [HttpGet("{id}", Name = "CampGet")]
        public IActionResult Get(int id, bool includeSpeakers = false)
        {
            try
            {
                Camp camp = null;

                if (includeSpeakers) camp = _repo.GetCampWithSpeakers(id);
                else camp = _repo.GetCamp(id);

                if (camp == null) return NotFound($"Camp {id} was not found");

                return Ok(camp);

            }
            catch
            {                
            }

            return BadRequest();
        }

        public async Task<IActionResult> Post(Camp model)
        {
            try
            {
                _repo.Add(model);

                if (await _repo.SaveAllAsync())
                {
                    return Created(newUri, model);
                }
            }
            catch (System.Exception)
            {
            }

            return BadRequest();
        }        
    }
}
