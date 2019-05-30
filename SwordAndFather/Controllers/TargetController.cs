using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SwordAndFather.Data;
using SwordAndFather.Models;

namespace SwordAndFather.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TargetController : ControllerBase
    {
        readonly ITargetRepository _repo;

        public TargetController(ITargetRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("register")]
        public ActionResult AddTarget(CreateTargetRequest createRequest)
        {
            var newTarget = _repo.AddTarget(
                createRequest.Name,
                createRequest.Location,
                createRequest.FitnessLevel,
                createRequest.UserId);

            return Created($"/api/target/{newTarget.Id}", newTarget);

        }
    }
}