using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TarefasLuby.V1.Models;
using TarefasLuby.V1.Repositories.Contracts;

namespace TarefasLuby.V1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly UserManager<AppUser> _userManager;
        public TaskController(ITaskRepository taskRepository, UserManager<AppUser> userManager)
        {
            _taskRepository = taskRepository;
            _userManager = userManager;
        }

        [Authorize("Bearer")]
        [HttpPost("save")]
        public ActionResult SaveTask([FromBody]Task task)
        {
            var appUser = _userManager.GetUserAsync(HttpContext.User).Result;
            return Ok(_taskRepository.SaveTask(task, appUser));
        }

        [Authorize("Bearer")]
        [HttpPut("update")]
        public ActionResult UpdateTask([FromBody] Task task)
        {
            var appUser = _userManager.GetUserAsync(HttpContext.User).Result;
            return Ok(_taskRepository.UpdateTask(task, appUser));
        }

        [Authorize("Bearer")]
        [HttpPut("conclude/{id}")]
        public ActionResult Conclude([FromRoute]int id)
        {
            _taskRepository.Conclude(id);
            return Ok();
        }

        [Authorize("Bearer")]
        [HttpDelete("remove/{id}")]
        public ActionResult Remove([FromRoute] int id)
        {
            _taskRepository.Remove(id);
            return Ok();
        }

        [Authorize("Bearer")]
        [HttpGet("")]
        public ActionResult GetTask()
        {
            var appUser = _userManager.GetUserAsync(HttpContext.User).Result;
            return Ok(_taskRepository.GetTask(appUser));
        }
    }
}
